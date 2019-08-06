namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Plugin
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string PluginId { get; set; }

        [StringLength(18)]
        public string IsDisabled { get; set; }

        public int? Taxis { get; set; }
    }
}
