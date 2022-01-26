using LinkShorter.Models;
using LinkShorter.Services.LinkService;
using LinkShorter.Services.StatisticalService;
using Microsoft.AspNetCore.Mvc;
using QRCode.Services;
using ShorterLink_RestApi.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LinkShorter.Controllers;
[ApiExplorerSettings(IgnoreApi = true)]

public class HomeController : Controller
{
    private readonly ILinkService _linkService;
    private readonly IStatisticalService _statisticalService;
    /// Ignore every controller method in FooBarController in documentation
    public HomeController(ILinkService linkService, IStatisticalService statisticalService)
    {
        _linkService = linkService;
        _statisticalService = statisticalService;
    }
    [Route("/{url?}")]
    [HttpGet]
    [Route("/ریزلینکو")]
    [Route("/ریز_لینکو")]

    public IActionResult Index(string url)
    {

        ViewData["Title"] = "صفحه اصلی";
        if (string.IsNullOrEmpty(url) || url=="index.html")
            return View(new InputLinkModel());
        else
        {
            var link = _linkService.GetLinkByShortLink(url);
            if (link == null) throw new NullReferenceException();
            if (string.IsNullOrEmpty(link.Password))
                return PrepareRedirect(link);
            else
                return RedirectToAction("Password", new InputLinkModel() { Id = link.Id });
        }
    }
    private IActionResult PrepareRedirect(Link link)
    {
        var preUrl = "";
        if (!link.OrginalUrl.Contains("http"))
            preUrl += "http://";
        if (!link.OrginalUrl.Contains("www."))
            preUrl += "www.";

        //var ip = Request.HttpContext.Connection.RemoteIpAddress.Address;
        var report = _statisticalService.PrepareStatistic(link.Id, "");
        _statisticalService.Insert(report);


        return Redirect($"{preUrl}{link.OrginalUrl}");
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/ریزلینکو")]
    [Route("/ریز_لینکو")]
    public IActionResult Index(InputLinkModel model)
    {
        if (ModelState.IsValid)
        {
            var url = _linkService.GenerateShortUrl(new CreateShortLinkModel() { OrginalUrl = model.OrginalUrl, Pass = model.Password, ShortUrl = model.ShortUrl });
            if (string.IsNullOrEmpty(url))
            {
                ModelState.AddModelError(nameof(model.ShortUrl), "لینک کوتاه وارد شده،قبلا در سیستم ثبت شده");
                return View(model);
            }
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            

            model.ShortUrl = $"{host}/{url}";


            //---------Generate Qrcode-------------
            using QrCodeServices qrcode = new();
            model.QrCode = qrcode.CreateQRCodeOnByteArray(model.ShortUrl);

        }
        return View(model);
    }


    [Route("/Pass/{id}")]
    [HttpGet]
    public IActionResult Password(int id)
    {
        return View(new InputLinkModel() { Id = id });
    }

    [HttpPost]
    [Route("/Pass/{id}")]
    [ValidateAntiForgeryToken]
    public IActionResult Password(InputLinkModel inputLinkModel)
    {
        var link = _linkService.GetLinkById(inputLinkModel.Id);
        if (inputLinkModel.Password != link.Password)
            inputLinkModel.Error = "رمزعبور صحیح نیست";

        if (!string.IsNullOrEmpty(inputLinkModel.Error))
            return View(inputLinkModel);
        else
            return PrepareRedirect(link);

    }


    [HttpGet]
    [Route("/بازدید")]
    public IActionResult Statistics()
    {
        StatisticalModel model = new StatisticalModel();
        return View(model);
    }

    
    [HttpPost]
    [Route("/بازدید")]
    [ValidateAntiForgeryToken]
    public IActionResult Statistics(StatisticalModel model)
    {
        if (ModelState.IsValid)
        {
            model.ShortUrl = model.ShortUrl.Trim();
            if (model.ShortUrl.Contains('/'))
                model.ShortUrl = model.ShortUrl
                    .Substring(model.ShortUrl
                    .LastIndexOf('/'))
                    .Replace("/",string.Empty);
            
            if(string.IsNullOrEmpty(_linkService.GetOrginalLinkByShortLink(model.ShortUrl)))
            {
                ModelState.AddModelError(nameof(model.ShortUrl), "لینک کوتاه مورد نظر یافت نشد");
                return View(model);
            }
            var link = _linkService.GetLinkByShortLink(model.ShortUrl);
            if (link == null)
            {
                ModelState.AddModelError(nameof(model.ShortUrl), "لینک کوتاه مورد نظر یافت نشد");
                return View(model);
            }
            var reports = _statisticalService.GetReportsByLinkId(link.Id);
            //var dates = reports
            //    .OrderBy(x => x.VisitedDateTime)
            //    .Select(x => x.VisitedDateTime)
            //    .Distinct();
            //string stringDates = "";
            //var countlist = new List<int>();
            //foreach (var date in dates)
            //{
            //    countlist.Add(reports.Count(x => x.VisitedDateTime == date));
            //    stringDates += $"{date.ToString("yyyy/mm/dddd")}, ";
            //}

            model = new StatisticalModel
            {
                ShowChart =true,
                ShortUrl = model.ShortUrl,
                TotalCount = reports.Count(),
                //DailyReportModel = new DailyReportModel()
                //{
                //    VisitedCount = countlist,
                //    Label = stringDates
                //},
                Os = new OsModel()
                {
                    Windows = reports.Count(x => x.Os == "Windows"),
                    Android = reports.Count(x => x.Os == "Android"),
                    ioS = reports.Count(x => x.Os == "iOS")
                },
                Device = new DeviceModel()
                {
                    Desktop = reports.Count(x => x.Device == "Desktop"),
                    Mobile = reports.Count(x => x.Device == "Mobile"),
                    Tablet = reports.Count(x => x.Device == "Tablet"),
                },
                Browser = new BrowserModel()
                {
                    Chrome = reports.Count(x => x.Browser == "Chrome"),
                    Edge = reports.Count(x => x.Browser == "Edge"),
                    Firefox = reports.Count(x => x.Browser == "Firefox"),
                    Safari = reports.Count(x => x.Browser == "Safari")
                }
            };


            return View(model);
        }
        return View(model);
    }

    [Route("وب سرویس_ریزلینکو")]
    [Route("وب سرویس")]
    [HttpGet]
    public IActionResult WebService()
    {
        return View();
    }

    [HttpGet]
    [Route("/پلاگین_ریزلینکو")]
    [Route("/پلاگین")]
    public IActionResult Plugin()
    {
        return View();
    }

    [HttpGet]
    [Route("/وب هوک_ریزلینکو")]
    [Route("/وب_هوک")]
    public IActionResult WebHook()
    {
        return View();
    }
}
