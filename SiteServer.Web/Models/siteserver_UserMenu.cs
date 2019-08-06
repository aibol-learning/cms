namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_UserMenu
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SystemId { get; set; }

        [StringLength(200)]
        public string GroupIdCollection { get; set; }

        public bool? IsDisabled { get; set; }

        public int? ParentId { get; set; }

        public int? Taxis { get; set; }

        [StringLength(50)]
        public string Text { get; set; }

        [StringLength(50)]
        public string IconClass { get; set; }

        [StringLength(200)]
        public string Href { get; set; }

        [StringLength(50)]
        public string Target { get; set; }
    }
}
