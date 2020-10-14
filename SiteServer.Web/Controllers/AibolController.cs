using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SiteServer.API.Models;
using SiteServer.CMS.Core;
using SiteServer.CMS.DataCache;
using SiteServer.CMS.Model;
using SiteServer.Utils;

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
            var page = Convert.ToInt32(HttpContext.Current.Request["page"] ?? "1");

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
            var page = Convert.ToInt32(HttpContext.Current.Request["page"] ?? "1");

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

            var pv = db.PVs.FirstOrDefault(o => o.Site == site && o.Id == date + site);

            if (pv == null)
            {
                pv = new PV()
                {
                    Count = 0,
                    Id = date + site,
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

            var pv = db.PVs.FirstOrDefault(o => o.Site == site && o.Id == date + site);
            if (pv == null)
            {
                pv = new PV()
                {
                    Count = 0,
                    Id = date + site,
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

            var firstDay = pvs.OrderBy(o => Convert.ToDateTime(o.Id.Substring(0, 10))).FirstOrDefault();
            if (firstDay == null)
            {
                return Json(new { code = 200, data = 0 });
            }

            var firstDate = Convert.ToDateTime(firstDay.Id.Substring(0, 10));

            var totalDays = (DateTime.Now - firstDate).Days;

            if (totalDays == 0)
            {
                return Json(new { code = 200, data = firstDay.Count });
            }

            return Json(new { code = 200, data = totalCount / totalDays });
        }


        [HttpGet, Route("GetTotalPV")]
        public IHttpActionResult GetTotalPV()
        {
            string site = HttpContext.Current.Request["site"] ?? string.Empty;

            var pvs = db.PVs.Where(o => o.Site == site).ToList();

            var totalCount = pvs.Sum(o => o.Count);

            return Json(new { code = 200, data = totalCount });
        }


        #endregion

        #region 前10排名接口

        [HttpGet, Route("GetDepartmentTop6")]
        public IHttpActionResult GetDepartmentTop6()
        {
            var contents = GetContents();

            var re = contents.GroupBy(o => o.Source).Select(o => new { department = o.Key, count = o.Count() }).OrderByDescending(o => o.count).Where(o => !string.IsNullOrEmpty(o.department)).Take(6).ToList();

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

            var contents = query.Where(o => o.ChannelId > 0 && o.IsChecked.ToLower() == "true").ToList();
            return contents;
        }

        public class Content
        {
            public int Id { get; set; }
            public DateTime AddDate { get; set; }
            public string Author { get; set; }
            public string Source { get; set; }

            public int ChannelId { get; set; }

            public string IsChecked { get; set; }
        }

        [HttpGet, Route("GetAuthorTop6")]
        public IHttpActionResult GetAuthorTop6()
        {
            var contents = GetContents();

            var list = contents.SelectMany(o => o.Author.Split(','));

            var re = list.GroupBy(o => o).Select(o => new { author = o.Key, count = o.Count() }).Where(o => !string.IsNullOrEmpty(o.author)).OrderByDescending(o => o.count).Take(6).ToList();

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

            var list = query.OrderBy(o => o.Id).ToList().Select(o => new { id = $"{o.SSOId}", text = $"{o.DisplayName}" });
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

            // workaround
            if (Guid.TryParse(adminSub, out var adminId))
            {
                var str1_adminId = adminId.ToString();
                var str2_adminId = adminId.ToString("N");

                var admin = db.siteserver_Administrator
                    .FirstOrDefault(o => o.SSOId == str1_adminId || o.SSOId == str2_adminId);

                return Json(new { code = 200, data = new { id = admin?.SSOId, text = admin?.DisplayName } });
            }
            else
            {
                return Json(new { code = 200, data = new { id = string.Empty, text = "not found" } });
            }
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
            var admin = db.siteserver_Administrator.FirstOrDefault(o => o.SSOId == sub);

            if (admin == null)
            {
                return Json(new { code = 200, data = new { id = string.Empty, text = string.Empty } });
            }

            return Json(new { code = 200, data = new { id = admin.SSOId, text = admin.DisplayName } });

        }

        #endregion

        #region 批量导出

        [HttpGet, Route("GetExcel")]
        public HttpResponseMessage GetExcel()
        {
            var excel = new ExcelPackage();

            GetContentList(out var list, out var siteId);

            var i = 1;
            var ws = excel.Workbook.Worksheets.Add("个人统计");


            ws.Cells[1, 1].Value = "信息员";
            ws.Cells[1, 2].Value = "工号";
            ws.Cells[1, 3].Value = "来源";
            ws.Cells[1, 4].Value = "文章标题";
            ws.Cells[1, 5].Value = "栏目名称";
            ws.Cells[1, 6].Value = "配图";
            ws.Cells[1, 7].Value = "发布日期";
            i++;

            var sid = Convert.ToInt32(siteId);
            var channels = db.siteserver_Channel.Where(o => o.SiteId == sid).ToList();

            foreach (var content in list)
            {
                foreach (var author in content.Author.Split(','))
                {
                    var index = author.IndexOf('(');
                    ws.Cells[i, 1].Value = string.IsNullOrEmpty(author) || index < 0 ? string.Empty : author.Substring(0, index);
                    ws.Cells[i, 2].Value = string.IsNullOrEmpty(author) || index < 0 ? string.Empty : author.Substring(index + 1, author.Length - index - 2);
                    ws.Cells[i, 3].Value = content.Source;
                    ws.Cells[i, 4].Value = content.Title;
                    ws.Cells[i, 5].Value = channels.FirstOrDefault(o => o.Id == content.ChannelId)?.ChannelName;
                    ws.Cells[i, 6].Value = content.Picturer;
                    ws.Cells[i, 7].Value = content.AddDate.ToString("yyyy/MM/dd");
                    i++;
                }
            }

            var result = Request.CreateResponse(HttpStatusCode.OK);


            MemoryStream memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);//没这句话就格式错

            result.Content = new StreamContent(memoryStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"内容列表{DateTime.Now.ToString("yyyyMMdd")}.xlsx"
            };

            return result;
        }

        [HttpGet, Route("GetExcelDepartment")]
        public HttpResponseMessage GetExcelDepartment()
        {
            var excel = new ExcelPackage();

            GetContentList(out var list, out var siteId);

            var i = 1;
            var ws = excel.Workbook.Worksheets.Add("支部统计");


            ws.Cells[1, 1].Value = "来源";
            ws.Cells[1, 2].Value = "数量";
            ws.Cells[1, 3].Value = "文章标题";
            i++;

            ws.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;//水平居中
            ws.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;//垂直居中
            ws.Cells.AutoFitColumns();

            foreach (var content in list.GroupBy(o => o.Source))
            {
                var titles = content.Select(o => o.Title).ToList();
                ws.Cells[i, 1].Value = content.Key;
                ws.Cells[i, 1, i + titles.Count - 1, 1].Merge = true;
                ws.Cells[i, 2].Value = content.Count();
                ws.Cells[i, 2, i + titles.Count - 1, 2].Merge = true;
                foreach (var title in titles)
                {
                    ws.Cells[i, 3].Value = title;
                    i++;
                }
            }

            var result = Request.CreateResponse(HttpStatusCode.OK);


            MemoryStream memoryStream = new MemoryStream();
            excel.SaveAs(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);//没这句话就格式错

            result.Content = new StreamContent(memoryStream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"内容列表{DateTime.Now.ToString("yyyyMMdd")}.xlsx"
            };

            return result;
        }

        private void GetContentList(out List<ExportContent> list, out string siteId)
        {
            siteId = HttpContext.Current.Request["siteId"] ?? string.Empty;
            string startTime = HttpContext.Current.Request["startTime"] ?? string.Empty;
            string endTime = HttpContext.Current.Request["endTime"] ?? string.Empty;

            var query = db.Database.SqlQuery<ExportContent>($"select * from [siteserver_Content_{siteId}]").AsQueryable();

            query = query.Where(o => o.ChannelId > 0);//siteserver用负数标识删除。

            if (DateTime.TryParse(startTime, out var start))
            {
                query = query.Where(o => o.AddDate >= start);
            }

            if (DateTime.TryParse(endTime, out var end))
            {
                query = query.Where(o => o.AddDate <= end);
            }

            list = query.Where(o => o.ChannelId > 0 && o.IsChecked.ToLower() == "true").ToList();
        }

        public class ExportContent : Content
        {
            public string Title { get; set; }
            public int ChannelId { get; set; }
            public string Picturer { get; set; }
        }

        #endregion

        #region 和令牌及跨域数据相关的一些接口

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("Login")]
        public IHttpActionResult Login(string returnUrl)
        {
            var redirectUrl = WebConfigUtils.SSOService.AuthorizeEndPoint(returnUrl);

            return Redirect(redirectUrl);
        }

        /// <summary>
        /// 用户已登录的回调
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("idlogon")]
        public IHttpActionResult IdentityServerLogon()
        {
            var request = new AuthenticatedRequest();

            var accessToken = request.HttpRequest.Form["access_token"];
            if (string.IsNullOrEmpty(accessToken))
            {
                var redirect = request.AdminRedirectCheck(true, true, true);
                if (redirect != null) return Redirect(redirect.RedirectUrl);
            }

            var parsed2 = AuthenticatedRequest.ParseAccessToken(accessToken, false);
            var adminInfo = AdminManager.GetAdminInfoByUserSub(parsed2.sub);

            // 新用户自动登录
            if (adminInfo == null)
            {
                adminInfo = AdminManager.UpdateNewUserFromIdentityServer(accessToken);
            }

            // 记录最后登录时间、失败次数清零
            DataProvider.AdministratorDao.UpdateLastActivityDateAndCountOfLogin(adminInfo);
            request.AdminLogin(adminInfo.UserName, false);

            // add idserver cookie
            CookieUtils.SetCookie(Constants.AuthKeyIdentityServer, accessToken);
            var idToken = request.HttpRequest.Form["id_token"];
            CookieUtils.SetCookie(Constants.AuthKeyIdentityServerIdToken, idToken);

            // 获取转向地址
            var state = request.HttpRequest.Form["state"].Replace('.', '=');
            var returnUrl = Encoding.UTF8.GetString(Convert.FromBase64String(state));
            if (!returnUrl.StartsWith("http"))
            {
                var requestUrl = request.HttpRequest.Url;
                returnUrl = $"{requestUrl.Scheme}://{requestUrl.Authority}{returnUrl}";
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 用户登录信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("userinfo")]
        public IHttpActionResult CurrentUserInfo()
        {
            var request = new AuthenticatedRequest();
            var accessToken = request.GetCookie(Constants.AuthKeyIdentityServer);

            var at = AuthenticatedRequest.ParseAccessToken(accessToken, false);
            if (string.IsNullOrEmpty(at.sub))
                return Ok();

            var user = AdminManager.GetAdminInfoByUserSub(at.sub);
            if (user == null)
            {
                // 新增用户
                user = AdminManager.UpdateNewUserFromIdentityServer(accessToken);
            }

            return Ok(new
            {
                user.UserName,
                user.DisplayName,
                token = accessToken
            });
        }

        /// <summary>
        /// 用户角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("CurrentUserRoles")]
        public IHttpActionResult CurrentUserRoles()
        {
            var request = new AuthenticatedRequest();
            var accessToken = request.GetCookie(Constants.AuthKeyIdentityServer);

            var at = AuthenticatedRequest.ParseAccessToken(accessToken, false);
            if (string.IsNullOrEmpty(at.sub))
                return Ok();

            var user = AdminManager.GetAdminInfoByUserSub(at.sub);

            var roles = db.siteserver_AdministratorsInRoles.Where(o => o.UserName == user.UserName).ToList();

            return Ok(new
            {
                roles
            });
        }

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("logout")]
        public IHttpActionResult Logout()
        {
            var request = new AuthenticatedRequest();
            var idToken = request.GetCookie(Constants.AuthKeyIdentityServerIdToken);

            var requestUrl = request.HttpRequest.Url;
            var postLogoutUrl = HttpUtility.UrlEncode($"{requestUrl.Scheme}://{requestUrl.Authority}/");

            // SiteServer退出
            request.AdminLogout();

            // IdentityServer退出
            return Redirect(WebConfigUtils.SSOService.LogoutEndPoint(idToken, postLogoutUrl));
        }

        [HttpGet, Route("tasks")]
        public IHttpActionResult Tasks()
        {
            var request = new AuthenticatedRequest();
            var accessToken = request.GetCookie(Constants.AuthKeyIdentityServer);
            if (accessToken == "")
            {
                return Ok(new { code = 401 });
            }

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Authorization", $"Bearer {accessToken}");

                var TasksApiUrl = ConfigurationManager.AppSettings["TasksApiUrl"];
                try
                {
                    var re = client.DownloadString(TasksApiUrl);

                    var json = JsonConvert.DeserializeObject<dynamic>(re);

                    return Ok(json);
                }
                catch (Exception)
                {
                    return Ok(new { code = 401 });
                }

            }

        }

        //screenData: ' http://screen.aibol.com.cn/home/data' //大屏幕数据接口Url
        [HttpGet, Route("screenData")]
        public IHttpActionResult ScreenData()
        {

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;

                var ScreenData = ConfigurationManager.AppSettings["ScreenDataApiUrl"];

                var re = client.DownloadString(ScreenData);

                var json = JsonConvert.DeserializeObject<dynamic>(re);


                var GetStopDaysApiUrl = ConfigurationManager.AppSettings["GetStopDaysApiUrl"];

                var GetStopDaysApiUrlRe = client.DownloadString(GetStopDaysApiUrl);

                json.Stopdays = JsonConvert.DeserializeObject<dynamic>(GetStopDaysApiUrlRe);


                return Ok(json);
            }

        }

        #endregion

        public class ResponseData<T>
        {
            public string Code { get; set; }
            public T Data { get; set; }
            public string Messages { get; set; }

        }

        /// <summary>
        /// 新增待办事项
        /// </summary>
        /// <param name="Key">任务所关联的主键记录唯一标识</param>
        /// <param name="RedirectUrl">点击消息后的转向地址，需要绝对路径</param>
        /// <param name="Name">任务标题</param>
        /// <param name="Receivers">接收人ID，多个接收人的ID以半角逗号分隔</param>
        /// <returns>Code: 200，表示保存成功 Data: 新增的待办事项ID Messages: 如果失败，此字段用于存储出错内容</returns>
        public static ResponseData<string> CreateTasks(string Key, string RedirectUrl, string Name, string Receivers)
        {
            var CreateTasksApiUrl = ConfigurationManager.AppSettings["CreateTasksApiUrl"];
            var data = $"Key={Key}&RedirectUrl={RedirectUrl}&Name={Name}&Receivers={Receivers}";

            var re = post(CreateTasksApiUrl, data);
            return (ResponseData<string>)re;
        }

        /// <summary>
        /// 完成增待办事项
        /// </summary>
        /// <param name="Key">任务所关联的主键记录唯一标识</param>
        /// <returns>Code: 200，表示保存成功 Data: 新增的待办事项ID Messages: 如果失败，此字段用于存储出错内容</returns>
        public static ResponseData<string> CompleteTasks(string Key)
        {
            var CompleteTaskApiUrl = ConfigurationManager.AppSettings["CompleteTaskApiUrl"];
            var data = $"Key={Key}";

            var re = post(CompleteTaskApiUrl, data);
            return (ResponseData<string>)re;
        }



        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据 格式 value=xxx</param>
        /// <returns>dynamic json</returns>
        public static dynamic post(string url, string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);

            var request = new AuthenticatedRequest();
            var accessToken = request.GetCookie(Constants.AuthKeyIdentityServer);
            if (accessToken == "")
            {
                throw new AuthenticationException("accessToken 为空");
            }

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add("Authorization", $"Bearer {accessToken}");

                var responseData = client.UploadData(url, "post", bytes);

                Encoding enc = Encoding.GetEncoding("UTF-8");
                string re = enc.GetString(responseData);

                var json = JsonConvert.DeserializeObject<dynamic>(re);

                return json;
            }
        }


    }
}
