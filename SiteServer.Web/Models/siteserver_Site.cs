namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Site
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string SiteName { get; set; }

        [StringLength(50)]
        public string SiteDir { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(18)]
        public string IsRoot { get; set; }

        public int? ParentId { get; set; }

        public int? Taxis { get; set; }

        [Column(TypeName = "ntext")]
        public string SettingsXml { get; set; }
    }
}
