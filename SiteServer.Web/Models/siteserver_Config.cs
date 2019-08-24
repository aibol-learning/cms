namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Config
    {
        public int Id { get; set; }

        [StringLength(18)]
        public string IsInitialized { get; set; }

        [StringLength(50)]
        public string DatabaseVersion { get; set; }

        public DateTime? UpdateDate { get; set; }

        [Column(TypeName = "ntext")]
        public string SystemConfig { get; set; }
    }
}
