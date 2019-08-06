namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_AdministratorsInRoles
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string RoleName { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }
    }
}
