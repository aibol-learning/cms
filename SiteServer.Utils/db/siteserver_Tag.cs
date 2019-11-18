namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Tag
    {
        public int Id { get; set; }

        public int? SiteId { get; set; }

        [StringLength(2000)]
        public string ContentIdCollection { get; set; }

        [StringLength(255)]
        public string Tag { get; set; }

        public int? UseNum { get; set; }
    }
}
