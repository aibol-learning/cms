namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_Administrator
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string SSOId { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PasswordFormat { get; set; }

        [StringLength(128)]
        public string PasswordSalt { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public int? CountOfLogin { get; set; }

        public int? CountOfFailedLogin { get; set; }

        [StringLength(255)]
        public string CreatorUserName { get; set; }

        [StringLength(18)]
        public string IsLockedOut { get; set; }

        [Column(TypeName = "ntext")]
        public string SiteIdCollection { get; set; }

        public int? SiteId { get; set; }

        public int? DepartmentId { get; set; }

        public int? AreaId { get; set; }

        [StringLength(50)]
        public string DisplayName { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(200)]
        public string AvatarUrl { get; set; }
    }
}
