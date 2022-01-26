using LinkShorter.Models;
using LinkShorter.Services.LinkService;
using LinkShorter.Services.StatisticalService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using QRCode.Services;
using ShorterLink_RestApi.Models;

namespace ShorterLink_RestApi.Controllers;
[Route("api/v1")]
[ApiController]
[EnableCors("AllowAnyOrigin")]
public class RislinkoAPi : ControllerBase
{
    readonly ILinkService _linkService;
    readonly IStatisticalService _statisticalService;
    public RislinkoAPi(ILinkService linkService, IStatisticalService statisticalService)
    {
        _statisticalService = statisticalService;
        _linkService = linkService;
    }

    /// <summary>
    /// Call this Method for Get Short link
    /// </summary>
    /// <param name="createShortLinkModel"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateShortLink([FromBody] CreateShortLinkModel createShortLinkModel)
    {
        if (ModelState.IsValid)
        {
            var link = _linkService.GenerateShortUrl(createShortLinkModel);
            if (string.IsNullOrEmpty(link) && !string.IsNullOrEmpty(createShortLinkModel.ShortUrl))
                return Problem("عبارت مورد نظر برای لینک کوتاه قبلا استفاده شده!");
            else if (string.IsNullOrEmpty(link))
                return Problem("مشکلی در ساخت لینک کوتاه وجود دارد،مجدد امتحان کنید");
            var host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            //---------Generate Qrcode-------------
            using QrCodeServices qrcode = new();
            qrcode.CreateQRCodeOnBitmap($"{host}/{link}", width: 100, height: 100).Save($"wwwroot/Qrcodes/{link}.png");

            return Ok(new { ShortLink = $"{host}/{link}", createShortLinkModel.OrginalUrl, Password = createShortLinkModel.Pass, QrcodeLink = $"{host}/Qrcodes/{link}.png" });
        }
        return ValidationProblem();
    }
    /// <summary>
    /// Call this Method and Get Statistical of Your link
    /// </summary>
    /// <param name="url">your Short Url</param>
    /// <returns></returns>
    [HttpGet]
    //[ResponseCache(Duration = 3600 * 24, Location = ResponseCacheLocation.Client, VaryByQueryKeys = new[] { "ShortUrl" })]
    public IActionResult Report([FromQuery] string ShortUrl)
    {
        if (string.IsNullOrEmpty(ShortUrl))
            return Problem("لینک کوتاه وارد نشده است");

        ShortUrl = ShortUrl.Trim();
        if (ShortUrl.Contains('/'))
            ShortUrl = ShortUrl
                .Substring(ShortUrl
                .LastIndexOf('/'))
                .Replace("/", string.Empty);
        var link = _linkService.GetLinkByShortLink(ShortUrl);
        if (link == null)
        {
            return ValidationProblem("لینک کوتاه مورد نظر یافت نشد");
        }

        var reports = _statisticalService.GetReportsByshortUrl(ShortUrl);

        return Ok(new StatisticalModel
        {
            TotalCount = reports.Count,
            Os = new OsModel
            {
                Windows = reports.Count(x => x.Os == "Windows"),
                Android = reports.Count(x => x.Os == "Android"),
                ioS = reports.Count(x => x.Os == "iOS")
            },
            Device = new DeviceModel
            {
                Desktop = reports.Count(x => x.Device == "Desktop"),
                Mobile = reports.Count(x => x.Device == "Mobile"),
                Tablet = reports.Count(x => x.Device == "Tablet"),
            },
            Browser = new BrowserModel
            {
                Chrome = reports.Count(x => x.Browser == "Chrome"),
                Edge = reports.Count(x => x.Browser == "Edge"),
                Firefox = reports.Count(x => x.Browser == "Firefox"),
                Safari = reports.Count(x => x.Browser == "Safari")
            },
            ShortUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{ShortUrl}"
        });
    }
}
