namespace SiteServer.API.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_TableStyle
    {
        public int Id { get; set; }

        public int? RelatedIdentity { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        [StringLength(50)]
        public string AttributeName { get; set; }

        public int? Taxis { get; set; }

        [StringLength(255)]
        public string DisplayName { get; set; }

        [StringLength(255)]
        public string HelpText { get; set; }

        [StringLength(18)]
        public string IsVisibleInList { get; set; }

        [StringLength(50)]
        public string InputType { get; set; }

        [StringLength(255)]
        public string DefaultValue { get; set; }

        [StringLength(18)]
        public string IsHorizontal { get; set; }

        [Column(TypeName = "ntext")]
        public string ExtendValues { get; set; }
    }
}
