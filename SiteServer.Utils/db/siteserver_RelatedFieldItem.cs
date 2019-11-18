namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_RelatedFieldItem
    {
        public int Id { get; set; }

        public int? RelatedFieldId { get; set; }

        [StringLength(255)]
        public string ItemName { get; set; }

        [StringLength(255)]
        public string ItemValue { get; set; }

        public int? ParentId { get; set; }

        public int? Taxis { get; set; }
    }
}
