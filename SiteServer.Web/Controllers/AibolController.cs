using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OfficeOpenXml;
using SiteServer.API.Models;

namespace SiteServer.API.Controllers
{

    [RoutePrefix("aibol")]
    public class AibolController : ApiController
    {
        private Db db = new Db();

        [HttpPost,Route("Upload")]
        public IHttpActionResult Upload()
        {
            string type = HttpContext.Current.Request["type"];
            HttpFileCollection files = HttpContext.Current.Request.Files;
            if (type == "author")
            {
                if (!files[0].FileName.StartsWith("信息员录入模板"))
                {
                    return Json(new {code = -1,msg = "请上传正确模板" });
                }

                using (ExcelPackage excelPackage = new ExcelPackage(files[0].InputStream))
                {
                    var ws = excelPackage.Workbook.Worksheets.First();
                    for (int i = 2; i <= ws.Dimension.Rows; i++)
                    {
                        var code = ws.Cells[i, 1].Value.ToString();
                        var name = ws.Cells[i, 2].Value.ToString();

                        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || db.Authors.Any(o=> o.Code == code))
                        {
                            continue;
                        }
                        else
                        {
                            db.Authors.Add(new Author(){Code = code,Name = name});
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

                        if (string.IsNullOrEmpty(name) || db.Departments.Any(o => o.Name == name ))
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
            return Json(db.Authors);
        }

        [HttpGet, Route("GetDepartments")]
        public IHttpActionResult GetDepartments()
        {
            return Json(db.Departments);
        }
    }
}
