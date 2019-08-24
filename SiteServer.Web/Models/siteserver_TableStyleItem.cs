namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_TableStyleItem
    {
        public int Id { get; set; }

        public int? TableStyleId { get; set; }

        [StringLength(255)]
        public string ItemTitle { get; set; }

        [StringLength(255)]
        public string ItemValue { get; set; }

        [StringLength(18)]
        public string IsSelected { get; set; }
    }
}
