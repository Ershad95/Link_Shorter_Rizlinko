using System;
using System.Collections.Generic;

#nullable disable

namespace LinkShorter.Models
{
    public partial class Link
    {
        

        public int Id { get; set; }
        public string OrginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool Active { get; set; }
        public bool UseCustomLink { get; set; }

        //public virtual ICollection<Report> Reports { get; set; }
    }
}
