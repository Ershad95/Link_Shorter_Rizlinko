using LinkShorter.Models;

namespace LinkShorter.Services.StatisticalService;

public interface IStatisticalService
{
    bool Insert(Report report);
    bool Update(Report report);
    bool Delete(Report report);

    Report PrepareStatistic(int linkId, string ip);

    IList<Report> GetReportsByLinkId(int linkid);
    IList<Report> GetReportsByshortUrl(string shorturl);
}
