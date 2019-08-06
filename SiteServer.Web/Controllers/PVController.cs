using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiteServer.API.Models;

namespace SiteServer.API.Controllers
{
    [RoutePrefix("aibol")]
    public class PVController : ApiController
    {

        private Db db = new Db();

        [Route("GetPVs")]
        public IHttpActionResult GetPVs()
        {
            return Json(db.PVs);
        }

        [Route("GetAdmins")]
        public IHttpActionResult GetAdmins()
        {
            return Json(db.siteserver_Administrator);
        }
    }
}
