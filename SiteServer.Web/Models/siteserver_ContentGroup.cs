namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_ContentGroup
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string GroupName { get; set; }

        public int? SiteId { get; set; }

        public int? Taxis { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }
    }
}
