namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_UserLog
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string IpAddress { get; set; }

        public DateTime? AddDate { get; set; }

        [StringLength(255)]
        public string Action { get; set; }

        [StringLength(255)]
        public string Summary { get; set; }
    }
}
