using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using SiteServer.CMS.Api.V1;
using SiteServer.CMS.Core;
using SiteServer.CMS.DataCache;
using SiteServer.CMS.Model;
using SiteServer.CMS.Plugin.Impl;
using SiteServer.Utils;

namespace SiteServer.API.Controllers.V1
{
    [RoutePrefix("v1/administrators")]
    public class V1AdministratorsController : ApiController
    {
        private const string Route = "";
        private const string RouteActionsLogin = "actions/login";
        private const string RouteActionsLogout = "actions/logout";
        private const string RouteActionsResetPassword = "actions/resetPassword";
        private const string RouteAdministrator = "{id:int}";

        [HttpPost, Route(Route)]
        public IHttpActionResult Create([FromBody] AdministratorInfoCreateUpdate adminInfo)
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                var retval = DataProvider.AdministratorDao.ApiInsert(adminInfo, out var errorMessage);
                if (retval == null)
                {
                    return BadRequest(errorMessage);
                }

                return Ok(new
                {
                    Value = retval
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route(RouteAdministrator)]
        public IHttpActionResult Update(int id, [FromBody] AdministratorInfoCreateUpdate adminInfo)
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                if (adminInfo == null) return BadRequest("Could not read administrator from body");

                if (!DataProvider.AdministratorDao.ApiIsExists(id)) return NotFound();

                var retval = DataProvider.AdministratorDao.ApiUpdate(id, adminInfo, out var errorMessage);
                if (retval == null)
                {
                    return BadRequest(errorMessage);
                }

                return Ok(new
                {
                    Value = retval
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route(RouteAdministrator)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                if (!DataProvider.AdministratorDao.ApiIsExists(id)) return NotFound();

                var adminInfo = DataProvider.AdministratorDao.ApiDelete(id);

                return Ok(new
                {
                    Value = adminInfo
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route(RouteAdministrator)]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                if (!DataProvider.AdministratorDao.ApiIsExists(id)) return NotFound();

                var adminInfo = DataProvider.AdministratorDao.ApiGetAdministrator(id);

                return Ok(new
                {
                    Value = adminInfo
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route(Route)]
        public IHttpActionResult List()
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                var top = request.GetQueryInt("top", 20);
                var skip = request.GetQueryInt("skip");

                var administrators = DataProvider.AdministratorDao.ApiGetAdministrators(skip, top);
                var count = DataProvider.AdministratorDao.ApiGetCount();

                return Ok(new PageResponse(administrators, top, skip, request.HttpRequest.Url.AbsoluteUri) { Count = count });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        [Obsolete("使用IdentityServer3")]
        [HttpPost, Route(RouteActionsLogin)]
        public IHttpActionResult Login()
        {
            try
            {
                var request = new AuthenticatedRequest();

                var account = request.GetPostString("account");
                var password = request.GetPostString("password");
                var isAutoLogin = request.GetPostBool("isAutoLogin");

                AdministratorInfo adminInfo;

                if (!DataProvider.AdministratorDao.Validate(account, password, true, out var userName, out var errorMessage))
                {
                    adminInfo = AdminManager.GetAdminInfoByUserName(userName);
                    if (adminInfo != null)
                    {
                        DataProvider.AdministratorDao.UpdateLastActivityDateAndCountOfFailedLogin(adminInfo); // 记录最后登录时间、失败次数+1
                    }
                    return BadRequest(errorMessage);
                }

                adminInfo = AdminManager.GetAdminInfoByUserName(userName);
                DataProvider.AdministratorDao.UpdateLastActivityDateAndCountOfLogin(adminInfo); // 记录最后登录时间、失败次数清零
                var accessToken = request.AdminLogin(adminInfo.UserName, isAutoLogin);
                var expiresAt = DateTime.Now.AddDays(Constants.AccessTokenExpireDays);

                return Ok(new
                {
                    Value = adminInfo,
                    AccessToken = accessToken,
                    ExpiresAt = expiresAt
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// 管理员退出
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("actions/logout")]
        public IHttpActionResult Logout()
        {
            var request = new AuthenticatedRequest();
            var idToken = request.GetCookie(Constants.AuthKeyIdentityServerIdToken);

            var requestUrl = request.HttpRequest.Url;
            var postLogoutUrl = HttpUtility.UrlEncode($"{requestUrl.Scheme}://{requestUrl.Authority}/");

            // SiteServer退出
            request.AdminLogout();

            return Redirect(WebConfigUtils.SSOService.LogoutEndPoint(idToken, postLogoutUrl));
        }

        [HttpPost, Route(RouteActionsResetPassword)]
        public IHttpActionResult ResetPassword()
        {
            try
            {
                var request = new AuthenticatedRequest();
                var isApiAuthorized = request.IsApiAuthenticated && AccessTokenManager.IsScope(request.ApiToken, AccessTokenManager.ScopeAdministrators);
                if (!isApiAuthorized) return Unauthorized();

                var account = request.GetPostString("account");
                var password = request.GetPostString("password");
                var newPassword = request.GetPostString("newPassword");

                if (!DataProvider.AdministratorDao.Validate(account, password, true, out var userName, out var errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                var adminInfo = AdminManager.GetAdminInfoByUserName(userName);

                if (!DataProvider.AdministratorDao.ChangePassword(adminInfo, newPassword, out errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                return Ok(new
                {
                    Value = adminInfo
                });
            }
            catch (Exception ex)
            {
                LogUtils.AddErrorLog(ex);
                return InternalServerError(ex);
            }
        }
    }
}
