namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_UserGroup
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string GroupName { get; set; }

        [StringLength(200)]
        public string AdminName { get; set; }
    }
}
