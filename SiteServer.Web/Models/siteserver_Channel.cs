namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Channel
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string ChannelName { get; set; }

        public int? SiteId { get; set; }

        [StringLength(50)]
        public string ContentModelPluginId { get; set; }

        [StringLength(255)]
        public string ContentRelatedPluginIds { get; set; }

        public int? ParentId { get; set; }

        [StringLength(255)]
        public string ParentsPath { get; set; }

        public int? ParentsCount { get; set; }

        public int? ChildrenCount { get; set; }

        [StringLength(18)]
        public string IsLastNode { get; set; }

        [StringLength(255)]
        public string IndexName { get; set; }

        [StringLength(255)]
        public string GroupNameCollection { get; set; }

        public int? Taxis { get; set; }

        public DateTime? AddDate { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [StringLength(200)]
        public string FilePath { get; set; }

        [StringLength(200)]
        public string ChannelFilePathRule { get; set; }

        [StringLength(200)]
        public string ContentFilePathRule { get; set; }

        [StringLength(200)]
        public string LinkUrl { get; set; }

        [StringLength(200)]
        public string LinkType { get; set; }

        public int? ChannelTemplateId { get; set; }

        public int? ContentTemplateId { get; set; }

        [StringLength(255)]
        public string Keywords { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Column(TypeName = "ntext")]
        public string ExtendValues { get; set; }
    }
}
