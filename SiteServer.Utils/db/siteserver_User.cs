namespace SiteServer.Utils.db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class siteserver_User
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        [StringLength(50)]
        public string PasswordFormat { get; set; }

        [StringLength(128)]
        public string PasswordSalt { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastResetPasswordDate { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public int? CountOfLogin { get; set; }

        public int? CountOfFailedLogin { get; set; }

        public int? GroupId { get; set; }

        [StringLength(18)]
        public string IsChecked { get; set; }

        [StringLength(18)]
        public string IsLockedOut { get; set; }

        [StringLength(255)]
        public string DisplayName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(200)]
        public string AvatarUrl { get; set; }

        [StringLength(50)]
        public string Gender { get; set; }

        [StringLength(50)]
        public string Birthday { get; set; }

        [StringLength(255)]
        public string WeiXin { get; set; }

        [StringLength(50)]
        public string Qq { get; set; }

        [StringLength(255)]
        public string WeiBo { get; set; }

        [Column(TypeName = "ntext")]
        public string Bio { get; set; }

        [Column(TypeName = "ntext")]
        public string SettingsXml { get; set; }
    }
}
