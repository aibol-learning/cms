namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Template
    {
        public int Id { get; set; }

        public int? SiteId { get; set; }

        [StringLength(50)]
        public string TemplateName { get; set; }

        [StringLength(50)]
        public string TemplateType { get; set; }

        [StringLength(50)]
        public string RelatedFileName { get; set; }

        [StringLength(50)]
        public string CreatedFileFullName { get; set; }

        [StringLength(50)]
        public string CreatedFileExtName { get; set; }

        [StringLength(50)]
        public string Charset { get; set; }

        [StringLength(18)]
        public string IsDefault { get; set; }
    }
}
