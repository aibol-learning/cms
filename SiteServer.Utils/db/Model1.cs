using System.Configuration;

namespace SiteServer.Utils.db
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        protected static string connString = ConfigurationManager.AppSettings["ConnectionString"];

        public Model1()
            : base(connString)
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Authors> Authors { get; set; }
        public virtual DbSet<Departments> Departments { get; set; }
        public virtual DbSet<PVs> PVs { get; set; }
        public virtual DbSet<siteserver_AccessToken> siteserver_AccessToken { get; set; }
        public virtual DbSet<siteserver_Administrator> siteserver_Administrator { get; set; }
        public virtual DbSet<siteserver_AdministratorsInRoles> siteserver_AdministratorsInRoles { get; set; }
        public virtual DbSet<siteserver_Area> siteserver_Area { get; set; }
        public virtual DbSet<siteserver_Channel> siteserver_Channel { get; set; }
        public virtual DbSet<siteserver_ChannelGroup> siteserver_ChannelGroup { get; set; }
        public virtual DbSet<siteserver_Config> siteserver_Config { get; set; }
        public virtual DbSet<siteserver_Content_1> siteserver_Content_1 { get; set; }
        public virtual DbSet<siteserver_Content_5> siteserver_Content_5 { get; set; }
        public virtual DbSet<siteserver_ContentCheck> siteserver_ContentCheck { get; set; }
        public virtual DbSet<siteserver_ContentGroup> siteserver_ContentGroup { get; set; }
        public virtual DbSet<siteserver_DbCache> siteserver_DbCache { get; set; }
        public virtual DbSet<siteserver_Department> siteserver_Department { get; set; }
        public virtual DbSet<siteserver_ErrorLog> siteserver_ErrorLog { get; set; }
        public virtual DbSet<siteserver_Keyword> siteserver_Keyword { get; set; }
        public virtual DbSet<siteserver_Log> siteserver_Log { get; set; }
        public virtual DbSet<siteserver_PermissionsInRoles> siteserver_PermissionsInRoles { get; set; }
        public virtual DbSet<siteserver_Plugin> siteserver_Plugin { get; set; }
        public virtual DbSet<siteserver_PluginConfig> siteserver_PluginConfig { get; set; }
        public virtual DbSet<siteserver_RelatedField> siteserver_RelatedField { get; set; }
        public virtual DbSet<siteserver_RelatedFieldItem> siteserver_RelatedFieldItem { get; set; }
        public virtual DbSet<siteserver_Role> siteserver_Role { get; set; }
        public virtual DbSet<siteserver_Site> siteserver_Site { get; set; }
        public virtual DbSet<siteserver_SiteLog> siteserver_SiteLog { get; set; }
        public virtual DbSet<siteserver_SitePermissions> siteserver_SitePermissions { get; set; }
        public virtual DbSet<siteserver_Special> siteserver_Special { get; set; }
        public virtual DbSet<siteserver_TableStyle> siteserver_TableStyle { get; set; }
        public virtual DbSet<siteserver_TableStyleItem> siteserver_TableStyleItem { get; set; }
        public virtual DbSet<siteserver_Tag> siteserver_Tag { get; set; }
        public virtual DbSet<siteserver_Template> siteserver_Template { get; set; }
        public virtual DbSet<siteserver_TemplateLog> siteserver_TemplateLog { get; set; }
        public virtual DbSet<siteserver_TemplateMatch> siteserver_TemplateMatch { get; set; }
        public virtual DbSet<siteserver_User> siteserver_User { get; set; }
        public virtual DbSet<siteserver_UserGroup> siteserver_UserGroup { get; set; }
        public virtual DbSet<siteserver_UserLog> siteserver_UserLog { get; set; }
        public virtual DbSet<siteserver_UserMenu> siteserver_UserMenu { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}