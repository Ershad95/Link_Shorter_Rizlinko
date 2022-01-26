using System.ComponentModel.DataAnnotations;

namespace LinkShorter.Models;

public class InputLinkModel
{
    [MaxLength(1000,ErrorMessage ="لینک مورد نر بیش از 1000 کاراکتر می باشد")]
    [Required(ErrorMessage = "لطفا لینک اصلی را وارد کنید")]
    [Display(Name ="لینک اصلی")]
    
    public string OrginalUrl { get; set; }
    public byte[] QrCode { get; set; }
    [Display(Name = "لینک کوتاه")]
    public string ShortUrl { get; set; }
    [Display(Name = "پسورد")]
    public string Password { get; set; }

    public string Error { get; set;  }
    public int Id { get; set;  }
}
