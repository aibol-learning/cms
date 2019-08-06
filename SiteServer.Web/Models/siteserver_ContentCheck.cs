namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_ContentCheck
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        public int? SiteId { get; set; }

        public int? ChannelId { get; set; }

        public int? ContentId { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(18)]
        public string IsChecked { get; set; }

        public int? CheckedLevel { get; set; }

        public DateTime? CheckDate { get; set; }

        [StringLength(255)]
        public string Reasons { get; set; }
    }
}
