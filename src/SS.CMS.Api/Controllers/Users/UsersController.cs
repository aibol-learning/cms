﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.CMS.Models;
using SS.CMS.Repositories;
using SS.CMS.Services;
using SS.CMS.Utils;

namespace SS.CMS.Api.Controllers.Users
{
    [ApiController]
    [AllowAnonymous]
    [Route("users")]
    public partial class UsersController : ControllerBase
    {
        public const string RouteLogin = "login";
        public const string RouteLoginCaptcha = "login/captcha";
        public const string RouteInfo = "info";
        public const string RouteMenus = "menus";
        public const string RouteLogout = "logout";

        private readonly ISettingsManager _settingsManager;
        private readonly IUserManager _userManager;
        private readonly IUserRepository _userRepository;

        public UsersController(ISettingsManager settingsManager, IUserManager userManager, IUserRepository userRepository)
        {
            _settingsManager = settingsManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpGet(RouteInfo)]
        public async Task<ActionResult<InfoResponse>> GetInfo()
        {
            var userInfo = await _userManager.GetUserAsync();
            if (userInfo == null || userInfo.IsLockedOut) return NotFound();
            var menus = await GetTopMenusAsync();

            return new InfoResponse
            {
                Id = userInfo.Id,
                DisplayName = userInfo.DisplayName,
                UserName = userInfo.UserName,
                AvatarUrl = userInfo.AvatarUrl,
                Bio = userInfo.Bio,
                Roles = new string[] { "admin" },
                Menus = menus
            };
        }

        [Authorize]
        [HttpGet(RouteMenus)]
        public async Task<IList<Menu>> GetMenus(string topMenu, int siteId)
        {
            if (StringUtils.EqualsIgnoreCase(topMenu, AuthTypes.Menus.Sites))
            {
                return await GetSiteMenusAsync(siteId);
            }
            return await GetAppMenusAsync(topMenu);
        }

        [HttpGet(RouteLoginCaptcha)]
        public ActionResult GetLoginCaptcha()
        {
            var code = CreateValidateCode();

            Response.Cookies.Delete(_cookieName);
            Response.Cookies.Append(_cookieName, code, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(10)
            });

            byte[] buffer;

            using (var image = new Bitmap(130, 53, PixelFormat.Format32bppRgb))
            {
                var r = new Random();
                var colors = _colors[r.Next(0, 5)];

                using (var g = Graphics.FromImage(image))
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(240, 243, 248)), 0, 0, 200, 200); //矩形框
                    g.DrawString(code, new Font(FontFamily.GenericSerif, 28, FontStyle.Bold | FontStyle.Italic), new SolidBrush(colors), new PointF(14, 3));//字体/颜色

                    var random = new Random();

                    for (var i = 0; i < 25; i++)
                    {
                        var x1 = random.Next(image.Width);
                        var x2 = random.Next(image.Width);
                        var y1 = random.Next(image.Height);
                        var y2 = random.Next(image.Height);

                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }

                    for (var i = 0; i < 100; i++)
                    {
                        var x = random.Next(image.Width);
                        var y = random.Next(image.Height);

                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }

                    g.Save();
                }

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Png);
                    buffer = ms.ToArray();
                }
            }

            if (!ContentTypeUtils.TryGetContentType(".png", out var contentType))
            {
                contentType = ContentTypeUtils.ContentTypeAttachment;
            }

            return File(buffer, contentType);
        }

        [HttpPost(RouteLogin)]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            User userInfo;

            var (isSuccess, userName, errorMessage) = await _userRepository.ValidateAsync(request.UserName, request.Password, true);

            if (!isSuccess)
            {
                userInfo = await _userRepository.GetByUserNameAsync(userName);
                if (userInfo != null)
                {
                    await _userRepository.UpdateLastActivityDateAndCountOfFailedLoginAsync(userInfo); // 记录最后登录时间、失败次数+1
                }
                return BadRequest(new
                {
                    Code = 400,
                    Message = errorMessage
                });
            }

            userInfo = await _userRepository.GetByUserNameAsync(userName);
            await _userRepository.UpdateLastActivityDateAndCountOfLoginAsync(userInfo); // 记录最后登录时间、失败次数清零
            //var accessToken = AdminLogin(userInfo.UserName, context.IsAutoLogin);
            //var expiresAt = DateTime.Now.AddDays(Constants.AccessTokenExpireDays);

            var token = _userManager.GetToken(userInfo);

            return Ok(new
            {
                Token = token,
                //AccessToken = accessToken,
                //ExpiresAt = expiresAt
            });
        }

        [HttpPost(RouteLogout)]
        public ActionResult Logout()
        {
            //await _userManager.SignOutAsync();

            return Ok(new
            {
                Value = true
            });
        }
    }
}