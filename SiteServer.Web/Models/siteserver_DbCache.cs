namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_DbCache
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string CacheKey { get; set; }

        [StringLength(500)]
        public string CacheValue { get; set; }

        public DateTime? AddDate { get; set; }
    }
}
