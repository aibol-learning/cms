namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Keyword
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Keyword { get; set; }

        [StringLength(50)]
        public string Alternative { get; set; }

        [StringLength(50)]
        public string Grade { get; set; }
    }
}
