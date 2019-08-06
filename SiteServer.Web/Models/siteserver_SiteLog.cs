namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_SiteLog
    {
        public int Id { get; set; }

        public int? SiteId { get; set; }

        public int? ChannelId { get; set; }

        public int? ContentId { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        public DateTime? AddDate { get; set; }

        [StringLength(255)]
        public string Action { get; set; }

        [StringLength(255)]
        public string Summary { get; set; }
    }
}
