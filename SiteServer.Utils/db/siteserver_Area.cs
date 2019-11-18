namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Area
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string AreaName { get; set; }

        public int? ParentId { get; set; }

        [StringLength(255)]
        public string ParentsPath { get; set; }

        public int? ParentsCount { get; set; }

        public int? ChildrenCount { get; set; }

        [StringLength(18)]
        public string IsLastNode { get; set; }

        public int? Taxis { get; set; }

        public int? CountOfAdmin { get; set; }
    }
}
