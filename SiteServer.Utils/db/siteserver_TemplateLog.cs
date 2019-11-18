namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_TemplateLog
    {
        public int Id { get; set; }

        public int? TemplateId { get; set; }

        public int? SiteId { get; set; }

        public DateTime? AddDate { get; set; }

        [StringLength(255)]
        public string AddUserName { get; set; }

        public int? ContentLength { get; set; }

        [Column(TypeName = "ntext")]
        public string TemplateContent { get; set; }
    }
}
