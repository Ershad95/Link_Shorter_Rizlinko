using System;
using System.Collections.Generic;

#nullable disable

namespace LinkShorter.Models
{
    public partial class Report
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public DateTime VisitedDateTime { get; set; }
        public string Os { get; set; }
        public string Browser { get; set; }
        public string Device { get; set; }
        public string Ip { get; set; }

        public virtual Link Link { get; set; }
    }
}
