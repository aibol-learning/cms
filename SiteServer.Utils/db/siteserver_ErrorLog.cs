namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_ErrorLog
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(200)]
        public string PluginId { get; set; }

        [StringLength(255)]
        public string Message { get; set; }

        [Column(TypeName = "ntext")]
        public string Stacktrace { get; set; }

        [Column(TypeName = "ntext")]
        public string Summary { get; set; }

        public DateTime? AddDate { get; set; }
    }
}
