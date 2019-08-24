namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Role
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string CreatorUserName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }
    }
}
