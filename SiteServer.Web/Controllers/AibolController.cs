using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using SiteServer.API.Models;

namespace SiteServer.API.Controllers
{

    [RoutePrefix("aibol")]
    public class AibolController : ApiController
    {
        private Db db = new Db();

        #region 信息员,支部接口

        [HttpPost, Route("Upload")]
        public IHttpActionResult Upload()
        {
            string type = HttpContext.Current.Request["type"];
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (type == "author")
            {
                if (!files[0].FileName.StartsWith("信息员录入模板"))
                {
                    return Json(new { code = -1, msg = "请上传正确模板" });
                }

                using (ExcelPackage excelPackage = new ExcelPackage(files[0].InputStream))
                {
                    var ws = excelPackage.Workbook.Worksheets.First();
                    for (int i = 2; i <= ws.Dimension.Rows; i++)
                    {
                        var code = ws.Cells[i, 1].Value.ToString();
                        var name = ws.Cells[i, 2].Value.ToString();

                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || db.Authors.Any(o => o.Code == code))
                        {
                            continue;
                        }
                        else
                        {
                            db.Authors.Add(new Author() { Code = code, Name = name });
                        }
                    }
                }

            }
            else if (type == "department")
            {
                if (!files[0].FileName.StartsWith("支部录入模板"))
                {
                    return Json(new { code = -1, msg = "请上传正确模板" });
                }

                using (ExcelPackage excelPackage = new ExcelPackage(files[0].InputStream))
                {
                    var ws = excelPackage.Workbook.Worksheets.First();
                    for (int i = 2; i <= ws.Dimension.Rows; i++)
                    {
                        var name = ws.Cells[i, 1].Value.ToString();

                        if (string.IsNullOrEmpty(name) || db.Departments.Any(o => o.Name == name))
                        {
                            continue;
                        }
                        else
                        {
                            db.Departments.Add(new Department() { Name = name });
                        }
                    }
                }
            }

            var count = db.SaveChanges();

            return Json(new { code = 200, msg = $"新增{count}条" });
        }

        [HttpGet, Route("GetAuthors")]
        public IHttpActionResult GetAuthors()
        {

            string search = HttpContext.Current.Request["search"] ?? string.Empty;
            var page = Convert.ToInt32(HttpContext.Current.Request["page"]);

            var count = db.Authors.Count(o => o.Name.Contains(search));

            var pagination = Math.Ceiling((double)count / 10) > page;

            var list = db.Authors.Where(o => o.Name.Contains(search) || o.Code.Contains(search)).OrderBy(o => o.Name).Skip(10 * (page - 1)).Take(10).ToList()
                .Select(o => new { id = $"{o.Name}({o.Code})", text = $"{o.Name}({o.Code})" });

            var re = new
            {
                results = list,
                pagination = pagination
            };

            return Json(re);
        }

        [HttpGet, Route("GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            string search = HttpContext.Current.Request["search"] ?? string.Empty;
            var page = Convert.ToInt32(HttpContext.Current.Request["page"]);

            var count = db.Departments.Count(o => o.Name.Contains(search));

            var pagination = Math.Ceiling((double)count / 10) > page;

            var list = db.Departments.Where(o => o.Name.Contains(search)).OrderBy(o => o.Name).Skip(10 * (page - 1)).Take(10).ToList()
                .Select(o => new { id = $"{o.Name}", text = $"{o.Name}" });

            var re = new
            {
                results = list,
                pagination = pagination
            };

            return Json(re);
        }

        #endregion

        #region 访问量功能接口

        [HttpGet, Route("AddPV")]
        public IHttpActionResult AddPV()
        {

            string site = HttpContext.Current.Request["site"] ?? string.Empty;

            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var pv = db.PVs.FirstOrDefault(o => o.Site == site && o.Id == date);

            if (pv == null)
            {
                pv = new PV()
                {
                    Count = 0,
                    Id = date,
                    Site = site
                };

                db.PVs.Add(pv);
            }

            pv.Count++;

            db.SaveChanges();

            return Json(new { code = 200, data = pv.Count });
        }

        [HttpGet, Route("GetPV")]
        public IHttpActionResult GetPV()
        {

            string site = HttpContext.Current.Request["site"] ?? string.Empty;

            var date = DateTime.Now.ToString("yyyy-MM-dd");

            var pv = db.PVs.FirstOrDefault(o => o.Site == site && o.Id == date);
            if (pv == null)
            {
                pv = new PV()
                {
                    Count = 0,
                    Id = date,
                    Site = site
                };
                db.PVs.Add(pv);
            }
            db.SaveChanges();

            return Json(new { code = 200, data = pv.Count });
        }

        [HttpGet, Route("GetAvgPV")]
        public IHttpActionResult GetAvgPV()
        {
            string site = HttpContext.Current.Request["site"] ?? string.Empty;

            var pvs = db.PVs.Where(o => o.Site == site).ToList();

            var totalCount = pvs.Sum(o => o.Count);

            var firstDay = pvs.OrderBy(o => Convert.ToDateTime(o.Id)).FirstOrDefault();
            if (firstDay == null)
            {
                return Json(new { code = 200, data = 0 });
            }

            var firstDate = Convert.ToDateTime(firstDay.Id);

            var totalDays = (DateTime.Now - firstDate).Days;

            if (totalDays == 0)
            {
                return Json(new { code = 200, data = firstDay.Count });
            }

            return Json(new { code = 200, data = totalCount / totalDays });
        }


        #endregion

        #region 前10排名接口

        [HttpGet, Route("GetDepartmentTop10")]
        public IHttpActionResult GetDepartmentTop10()
        {
            var contents = GetContents();

            var re = contents.GroupBy(o => o.Author).Select(o => new { author = o.Key, count = o.Count() }).OrderByDescending(o => o.count).Take(10).ToList();

            return Json(re);
        }

        private List<Content> GetContents()
        {
            string siteId = HttpContext.Current.Request["siteId"];
            string startTime = HttpContext.Current.Request["startTime"];
            string endTime = HttpContext.Current.Request["endTime"];


            var query = db.Database.SqlQuery<Content>($"select * from [siteserver_Content_{siteId}]").AsQueryable();

            if (startTime != null && DateTime.TryParse(startTime, out var start))
            {
                query = query.Where(o => o.AddDate >= start);
            }

            if (endTime != null && DateTime.TryParse(endTime, out var end))
            {
                query = query.Where(o => o.AddDate <= end.AddDays(1).AddSeconds(-1));
            }

            var contents = query.ToList();
            return contents;
        }

        public class Content
        {
            public int Id { get; set; }
            public DateTime AddDate { get; set; }
            public string Author { get; set; }
            public string Source { get; set; }
        }

        [HttpGet, Route("GetAuthorTop10")]
        public IHttpActionResult GetAuthorTop10()
        {
            var contents = GetContents();

            var re = contents.GroupBy(o => o.Source).Select(o => new { author = o.Key, count = o.Count() }).OrderByDescending(o => o.count).Take(10).ToList();

            return Json(re);
        }

        #endregion

        #region 审核员选取

        [HttpGet, Route("GetCheckers")]
        public IHttpActionResult GetCheckers()
        {
            string lv = HttpContext.Current.Request["lv"];
            string search = HttpContext.Current.Request["search"] ?? string.Empty;
            var page = Convert.ToInt32(HttpContext.Current.Request["page"]);

            var roleName = "";

            switch (lv)
            {
                case "0": roleName = "支部书记"; break;
                case "1": roleName = "公司领导"; break;
                case "2": roleName = "政工部"; break;
            }

            var query = from admin in db.siteserver_Administrator
                        join r in db.siteserver_AdministratorsInRoles on admin.UserName equals r.UserName
                        where r.RoleName == roleName && admin.DisplayName.Contains(search)
                        select admin;

            var list = query.OrderBy(o => o.Id).Skip((page - 1) * 10).Take(10).ToList().Select(o => new { id = $"{o.SSOId}", text = $"{o.DisplayName}" });
            var count = query.Count();

            var pagination = Math.Ceiling((double)count / 10) > page;

            var re = new
            {
                results = list,
                pagination
            };

            return Json(re);

        }

        [HttpGet, Route("GetCheckerByContent")]
        public IHttpActionResult GetCheckerByContent()
        {

            string lv = HttpContext.Current.Request["lv"];
            string siteId = HttpContext.Current.Request["siteId"] ?? string.Empty;
            string channelId = HttpContext.Current.Request["channelId"] ?? string.Empty;
            string contentId = HttpContext.Current.Request["contentId"] ?? string.Empty;

            var query = db.Database.SqlQuery<CheckContent>($"select * from [siteserver_Content_{siteId}] where ChannelId = '{channelId}' and Id = '{contentId}' ").AsQueryable();

            var content = query.FirstOrDefault();

            var adminSub = "";
            switch (lv)
            {
                case "0": adminSub = content?.Lv1AdminSub; break;
                case "1": adminSub = content?.Lv2AdminSub; break;
                case "2": adminSub = content?.Lv3AdminSub; break;
            }

            var admin = db.siteserver_Administrator.FirstOrDefault(o => o.SSOId == adminSub);

            return Json(new { code = 200, data = new { id = admin?.SSOId, text = admin?.DisplayName } });
        }

        public class CheckContent : Content
        {
            public string Lv1AdminSub { get; set; }
            public string Lv2AdminSub { get; set; }
            public string Lv3AdminSub { get; set; }
        }

        [HttpGet, Route("GetAdminBySub")]
        public IHttpActionResult GetAdminBySub()
        {
            string sub = HttpContext.Current.Request["sub"];
            var admin = db.siteserver_Administrator.First(o => o.SSOId == sub);

            return Json(new { code = 200, data = new { id = admin.SSOId, text = admin.DisplayName } });

        }

        #endregion

        #region 批量导出

        [HttpGet, Route("GetExcel")]
        public HttpResponseMessage GetExcel()
        {
            var excel = new ExcelPackage();

            string siteId = HttpContext.Current.Request["siteId"] ?? string.Empty;
            string startTime = HttpContext.Current.Request["startTime"] ?? string.Empty;
            string endTime = HttpContext.Current.Request["endTime"] ?? string.Empty;

            var query = db.Database.SqlQuery<ExportContent>($"select * from [siteserver_Content_{siteId}]").AsQueryable();

            if (DateTime.TryParse(startTime, out var start))
            {
                query = query.Where(o => o.AddDate >= start);
            }
            if (DateTime.TryParse(endTime, out var end))
            {
                query = query.Where(o => o.AddDate <= end);
            }

            var list = query.ToList();

            //todo 按照模板写入excel

            var result = Request.CreateResponse(HttpStatusCode.OK);

            result.Content = new StreamContent(excel.Stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"内容列表{DateTime.Now.ToString("yyyyMMdd")}.xlsx"
            };

            return result;
        }

        public class ExportContent : Content
        {
            public DateTime AddDate { get; set; }
        }

        #endregion

    }
}
