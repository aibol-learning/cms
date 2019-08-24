namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_RelatedField
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        public int? SiteId { get; set; }

        public int? TotalLevel { get; set; }

        [StringLength(255)]
        public string Prefixes { get; set; }

        [StringLength(255)]
        public string Suffixes { get; set; }
    }
}
