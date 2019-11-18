namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_AccessToken
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(200)]
        public string Token { get; set; }

        [StringLength(200)]
        public string AdminName { get; set; }

        [StringLength(200)]
        public string Scopes { get; set; }

        public int? RateLimit { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
