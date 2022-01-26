
using System.ComponentModel.DataAnnotations;

namespace ShorterLink_RestApi.Models;
/// <summary>
/// 
/// </summary>
public class CreateShortLinkModel
{

    /// <summary>
    /// Orginal Url (Long Url)
    /// </summary>
    [Required]
    [RegularExpression(pattern:@"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$",ErrorMessage ="لینک اصلی به درستی وارد نشده است")]
    public string OrginalUrl {  get; set; }
    /// <summary>
    /// Short Url of Orginal Url(Optional)
    /// </summary>
    public string ShortUrl {  get; set; }
    /// <summary>
    /// the Password For Visit Orginl Url(Optional)
    /// </summary>
    public string Pass {  get; set; }
  
}
