namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PVs
    {
        public string Id { get; set; }

        public int Count { get; set; }

        public string Site { get; set; }
    }
}
