﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Create;
using SiteServer.CMS.DataCache;
using SiteServer.CMS.DataCache.Content;
using SiteServer.CMS.Model;
using SiteServer.CMS.Model.Attributes;
using SiteServer.CMS.Provider;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalContentCheck : BasePageCms
    {
        protected override bool IsSinglePage => true;
        public Literal LtlTitles;
        public HtmlInputText Checker;
        public DropDownList DdlCheckType;
        public DropDownList DdlTranslateChannelId;
        public TextBox TbCheckReasons;

        public int _channelId { get; set; }
        public int _contentId { get; set; }

        private Dictionary<int, List<int>> _idsDictionary = new Dictionary<int, List<int>>();
        private string _returnUrl;

        public static string GetOpenWindowString(int siteId, int channelId, string returnUrl)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("审核内容", PageUtils.GetCmsUrl(siteId, nameof(ModalContentCheck), new NameValueCollection
            {
                {"channelId", channelId.ToString()},
                {"ReturnUrl", StringUtils.ValueToUrl(returnUrl)}
            }), "contentIdCollection", "请选择需要审核的内容！", 560, 550);
        }

        public static string GetOpenWindowStringForMultiChannels(int siteId, string returnUrl)
        {
            return LayerUtils.GetOpenScriptWithCheckBoxValue("审核内容", PageUtils.GetCmsUrl(siteId, nameof(ModalContentCheck), new NameValueCollection
            {
                {"ReturnUrl", StringUtils.ValueToUrl(returnUrl)}
            }), "IDsCollection", "请选择需要审核的内容！", 560, 550);
        }

        public static string GetOpenWindowString(int siteId, int channelId, int contentId, string returnUrl)
        {
            return LayerUtils.GetOpenScript("审核内容", PageUtils.GetCmsUrl(siteId, nameof(ModalContentCheck), new NameValueCollection
            {
                {"channelId", channelId.ToString()},
                {"contentIdCollection", contentId.ToString()},
                {"ReturnUrl", StringUtils.ValueToUrl(returnUrl)}
            }), 560, 550);
        }

        public static string GetRedirectUrl(int siteId, int channelId, int contentId, string returnUrl)
        {
            return PageUtils.GetCmsUrl(siteId, nameof(ModalContentCheck), new NameValueCollection
            {
                {"channelId", channelId.ToString()},
                {"ReturnUrl", StringUtils.ValueToUrl(returnUrl)},
                {"contentIdCollection", contentId.ToString()}
            });
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            PageUtils.CheckRequestParameter("siteId", "ReturnUrl");
            _returnUrl = StringUtils.ValueFromUrl(AuthRequest.GetQueryString("ReturnUrl"));

            _idsDictionary = ContentUtility.GetIDsDictionary(Request.QueryString);

            if (IsPostBack) return;

            var titles = new StringBuilder();
            foreach (var channelId in _idsDictionary.Keys)
            {
                var tableName = ChannelManager.GetTableName(SiteInfo, channelId);
                var contentIdList = _idsDictionary[channelId];

                //aibol 规定只能一条审核 channelId contentId
                _channelId = channelId;
                _contentId = contentIdList.FirstOrDefault();

                foreach (var contentId in contentIdList)
                {
                    var title = DataProvider.ContentDao.GetValue(tableName, contentId, ContentAttribute.Title);
                    titles.Append(title + "<br />");
                }
            }

            if (!string.IsNullOrEmpty(LtlTitles.Text))
            {
                titles.Length -= 6;
            }
            LtlTitles.Text = titles.ToString();

            var checkedLevel = 5;
            var isChecked = true;

            foreach (var channelId in _idsDictionary.Keys)
            {
                int checkedLevelByChannelId;
                var isCheckedByChannelId = CheckManager.GetUserCheckLevel(AuthRequest.AdminPermissionsImpl, SiteInfo, channelId, out checkedLevelByChannelId);
                if (checkedLevel > checkedLevelByChannelId)
                {
                    checkedLevel = checkedLevelByChannelId;
                }
                if (!isCheckedByChannelId)
                {
                    isChecked = false;
                }
            }

            CheckManager.LoadContentLevelToCheck(DdlCheckType, SiteInfo, isChecked, checkedLevel);

            var listItem = new ListItem("<保持原栏目不变>", "0");
            DdlTranslateChannelId.Items.Add(listItem);

            ChannelManager.AddListItemsForAddContent(DdlTranslateChannelId.Items, SiteInfo, true, AuthRequest.AdminPermissionsImpl);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            var checkedLevel = TranslateUtils.ToIntWithNagetive(DdlCheckType.SelectedValue);

            var checkSub = this.Checker.Value; // "73213610577d4d9ba88b549e61c24c8e";


            var isChecked = checkedLevel >= SiteInfo.Additional.CheckContentLevel;

            var contentInfoListToCheck = new List<ContentInfo>();
            var idsDictionaryToCheck = new Dictionary<int, List<int>>();
            foreach (var channelId in _idsDictionary.Keys)
            {
                var channelInfo = ChannelManager.GetChannelInfo(SiteInfo.Id, channelId);
                var contentIdList = _idsDictionary[channelId];
                var contentIdListToCheck = new List<int>();

                int checkedLevelOfUser;
                var isCheckedOfUser = CheckManager.GetUserCheckLevel(AuthRequest.AdminPermissionsImpl, SiteInfo, channelId, out checkedLevelOfUser);

                foreach (var contentId in contentIdList)
                {
                    var contentInfo = ContentManager.GetContentInfo(SiteInfo, channelInfo, contentId);
                    if (contentInfo != null)
                    {
                        if (CheckManager.IsCheckable(contentInfo.IsChecked, contentInfo.CheckedLevel, isCheckedOfUser, checkedLevelOfUser))
                        {
                            contentInfoListToCheck.Add(contentInfo);
                            contentIdListToCheck.Add(contentId);
                        }

                        //DataProvider.ContentDao.Update(SiteInfo, channelInfo, contentInfo);

                        //CreateManager.CreateContent(SiteId, contentInfo.ChannelId, contentId);
                        //CreateManager.TriggerContentChangedEvent(SiteId, contentInfo.ChannelId);


                        switch (DdlCheckType.SelectedItem.Text)
                        {
                            case "初审通过，等待二审":
                                //关闭初审代办
                                //发送消息给其他初审员和发稿人

                                var lv2SSOIds = DataProvider.PermissionsInRolesDao.GetCheckerSSOIds(2);
                                var addUserName = contentInfo.AddUserName;
                                var author = DataProvider.AdministratorDao.GetByUserName(addUserName);
                                lv2SSOIds.Add(author.SSOId);
                                BackstageManager.SendMessage(MessageType.消息, lv2SSOIds, "初审通过，等待二审");
                                //发送代办给二审
                                break;
                            case "初审退稿":
                                //关闭初审代办
                                //发送消息给其他初审员和发稿人
                                //发送代办给发稿人
                                break;
                            case "二审通过，等待终审":
                                //关闭二审代办
                                //发送消息给其他二审员和发稿人
                                //发送代办给终审员
                                break;
                            case "二审退稿":
                                //关闭初审代办
                                //发送消息给其他初审员
                                //发送代办给发稿人
                                break;
                            case "终审通过":
                                //关闭终审代办
                                //发送消息给其他终审员和发稿人
                                break;
                            case "终审退稿":
                                //关闭初审代办
                                //发送消息给其他初审员
                                //发送代办给发稿人
                                break;
                        }
                    }
                }
                if (contentIdListToCheck.Count > 0)
                {
                    idsDictionaryToCheck[channelId] = contentIdListToCheck;
                }




            }

            if (contentInfoListToCheck.Count == 0)
            {
                LayerUtils.CloseWithoutRefresh(Page, "alert('您的审核权限不足，无法审核所选内容！');");
                return;
            }

            var translateChannelId = TranslateUtils.ToInt(DdlTranslateChannelId.SelectedValue);

            foreach (var channelId in idsDictionaryToCheck.Keys)
            {
                var tableName = ChannelManager.GetTableName(SiteInfo, channelId);
                var contentIdList = idsDictionaryToCheck[channelId];
                DataProvider.ContentDao.UpdateIsChecked(tableName, SiteId, channelId, contentIdList, translateChannelId, AuthRequest.AdminName, isChecked, checkedLevel, TbCheckReasons.Text, checkSub);
            }

            if (translateChannelId > 0)
            {
                var tableName = ChannelManager.GetTableName(SiteInfo, translateChannelId);
                ContentManager.RemoveCache(tableName, translateChannelId);
            }

            AuthRequest.AddSiteLog(SiteId, SiteId, 0, "设置内容状态为" + DdlCheckType.SelectedItem.Text, TbCheckReasons.Text);


            foreach (var channelId in idsDictionaryToCheck.Keys)
            {
                var contentIdList = _idsDictionary[channelId];
                if (contentIdList != null)
                {
                    foreach (var contentId in contentIdList)
                    {
                        CreateManager.CreateContent(SiteId, channelId, contentId);
                        CreateManager.TriggerContentChangedEvent(SiteId, channelId);
                    }
                }
            }

            LayerUtils.CloseAndRedirect(Page, _returnUrl);
        }
    }
}
