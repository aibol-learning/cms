namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_TemplateMatch
    {
        public int Id { get; set; }

        public int? ChannelId { get; set; }

        public int? SiteId { get; set; }

        public int? ChannelTemplateId { get; set; }

        public int? ContentTemplateId { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        [StringLength(200)]
        public string ChannelFilePathRule { get; set; }

        [StringLength(200)]
        public string ContentFilePathRule { get; set; }
    }
}
