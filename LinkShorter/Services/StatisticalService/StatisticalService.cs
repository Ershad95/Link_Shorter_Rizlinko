
using LinkShorter.Models;
using LinkShorter.Repository;
using System.Runtime.InteropServices;
using Wangkanai.Detection.Services;

namespace LinkShorter.Services.StatisticalService;
public class StatisticalService : IStatisticalService
{
    private readonly IRepo<Report> _repo;
    private readonly IDetectionService _detectionService;
    public StatisticalService(IRepo<Report> repo,IDetectionService detectionService)
    {
        _detectionService = detectionService;
        _repo = repo;
    }
    public bool Delete(Report report)
    {
        return false;
    }

    public IList<Report> GetReportsByLinkId(int linkid)
    {
        return _repo.GetAllItems().Where(x => x.LinkId == linkid).ToList();
    }
    public Report PrepareStatistic(int linkId, string ip)
    {

        return new Report()
        {

            Browser = _detectionService.Browser.Name.ToString(),
            Os = _detectionService.Platform.Name.ToString(),
            Device = _detectionService.Device.Type.ToString(),
            LinkId = linkId,
            Ip = ip
        };
    }
    public bool Insert(Report report)
    {

        try
        {
            report.VisitedDateTime = DateTime.Now;
            _repo.InsertItem(report);
            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }

    public bool Update(Report report)
    {
        throw new NotImplementedException();
    }

    public IList<Report> GetReportsByshortUrl(string shorturl)
    {
        var statistic = _repo.GetAllItems().Where(x => x.Link.ShortUrl == shorturl);
        return statistic.ToList();
    }
}
