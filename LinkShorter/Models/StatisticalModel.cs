
using System.ComponentModel.DataAnnotations;

namespace LinkShorter.Models;
public class StatisticalModel
{

    public BrowserModel Browser { get; set; }
    public OsModel Os { get; set; }
    public DeviceModel Device { get; set; }
    [Display(Name = "لینک کوتاه")]
    [Required(ErrorMessage = "لطفا لینک کوتاه را وارد کنید")]
    public string ShortUrl { get; set; }
    public Int64 TotalCount { get; set; }

    public bool ShowChart { get; set; }

    public DailyReportModel DailyReportModel{ get; set; }
}
public class DailyReportModel
{
    public string Label { get; set; }
    public List<int> VisitedCount { get; set; }
}

