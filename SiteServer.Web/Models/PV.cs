﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Models
{
    public class PV
    {
        /// <summary>
        /// 日期 格式 yyyy-MM-dd + Site
        /// </summary>
        public string Id { get; set; }


        public string Site { get; set; }


        public int Count { get; set; }
    }
}