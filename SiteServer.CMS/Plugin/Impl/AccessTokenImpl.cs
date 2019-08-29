using System;
using SiteServer.Plugin;

namespace SiteServer.CMS.Plugin.Impl
{
    public class AccessTokenImpl : IAccessToken
    {
        private long _exp;

        /// <summary>
        /// sub in Identity Server
        /// </summary>
        public string sub { get; set; }


        /// <summary>
        /// clientid in Identity Server
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// exp in Identity Server
        /// </summary>
        public long exp
        {
            get => _exp;
            set
            {
                _exp = value;
                if (_exp > 1000)
                {
                    var startTime = new DateTime(1970, 1, 1);
                    ExpiresAt = startTime.ToLocalTime().AddSeconds(_exp);
                }
            }
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}