namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Content_5
    {
        public int Id { get; set; }

        public int? ChannelId { get; set; }

        public int? SiteId { get; set; }

        [StringLength(255)]
        public string AddUserName { get; set; }

        [StringLength(255)]
        public string LastEditUserName { get; set; }

        public DateTime? LastEditDate { get; set; }

        public int? AdminId { get; set; }

        public int? UserId { get; set; }

        public int? Taxis { get; set; }

        [StringLength(255)]
        public string GroupNameCollection { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        public int? SourceId { get; set; }

        public int? ReferenceId { get; set; }

        [StringLength(18)]
        public string IsChecked { get; set; }

        public int? CheckedLevel { get; set; }

        public int? Hits { get; set; }

        public int? HitsByDay { get; set; }

        public int? HitsByWeek { get; set; }

        public int? HitsByMonth { get; set; }

        public DateTime? LastHitsDate { get; set; }

        public int? Downloads { get; set; }

        [Column(TypeName = "ntext")]
        public string SettingsXml { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [StringLength(18)]
        public string IsTop { get; set; }

        [StringLength(18)]
        public string IsRecommend { get; set; }

        [StringLength(18)]
        public string IsHot { get; set; }

        [StringLength(18)]
        public string IsColor { get; set; }

        [StringLength(200)]
        public string LinkUrl { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? ShowTime { get; set; }

        [StringLength(255)]
        public string SubTitle { get; set; }

        [StringLength(200)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string VideoUrl { get; set; }

        [StringLength(200)]
        public string FileUrl { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Column(TypeName = "ntext")]
        public string Summary { get; set; }

        [StringLength(255)]
        public string Author { get; set; }

        [StringLength(255)]
        public string Source { get; set; }

        [StringLength(255)]
        public string Picturer { get; set; }

        [StringLength(255)]
        public string Lv1AdminSub { get; set; }

        [StringLength(255)]
        public string Lv2AdminSub { get; set; }

        [StringLength(255)]
        public string Lv3AdminSub { get; set; }
    }
}
