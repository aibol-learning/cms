using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SiteServer.Utils;

namespace SiteServer.CMS.Core
{
    public class BackstageManager
    {
        /// <summary>
        /// 发送消息任务
        /// </summary>
        /// <param name="type">类型:消息/任务</param>
        /// <param name="receiverIds">接收人ssoid guid列表</param>
        /// <param name="content">内容</param>
        /// <param name="redirectUrl">跳转地址</param>
        /// <param name="key">用于快速完成的标识</param>
        public static ResponseData<string> SendMessage(MessageType type, List<string> receiverIds, string content,string redirectUrl, string key = "")
        {
            LogUtils.AddAdminLog("aibol系统日志",$"发送消息任务{Enum.GetName(typeof(MessageType),type)},receiverIds:{string.Join(",",receiverIds)},content:{content},Key:{key}");

            try
            {
                if (type == MessageType.任务)
                {
                    return CreateTasks(key, redirectUrl, content, string.Join(",", receiverIds));
                }
                else
                {
                    return CreateMessages(key, redirectUrl, content, string.Join(",", receiverIds));
                }
            }
            catch (Exception e)
            {
                LogUtils.AddAdminLog("aibol系统日志-错误-发送消息任务",e.Message+e.StackTrace);
                return new ResponseData<string>() { Code = "-1",Messages = "发送失败" };
            }
        }

        public static ResponseData<string> CloseMessage(MessageType type, string key = "")
        {
            LogUtils.AddAdminLog("aibol系统日志", $"关闭消息任务{Enum.GetName(typeof(MessageType), type)},Key:{key}");

            try
            {
                if (type == MessageType.任务)
                {
                    return CompleteTasks(key);
                }
                else
                {
                    //消息不需要关闭
                    throw new NotImplementedException();
                }
            }
            catch (Exception e)
            {
                LogUtils.AddAdminLog("aibol系统日志-错误-关闭消息任务"+ key, e.Message + e.StackTrace);
                return new ResponseData<string>() { Code = "-1", Messages = "关闭失败" };
            }
        }



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
            var CreateTaskApiUrl = ConfigurationManager.AppSettings["CreateTaskApiUrl"];
            var data = $"Key={Key}&RedirectUrl={RedirectUrl}&Name={Name}&Receivers={Receivers}";

            var re = post<ResponseData<string>>(CreateTaskApiUrl, data);
            return re;
        }

        public static ResponseData<string> CreateMessages(string Key, string RedirectUrl, string Name, string Receivers)
        {
            var CreateTaskApiUrl = ConfigurationManager.AppSettings["CreateMessageApiUrl"];
            var data = $"Key={Key}&RedirectUrl={RedirectUrl}&Name={Name}&Receivers={Receivers}";

            var re = post<ResponseData<string>>(CreateTaskApiUrl, data);
            return re;
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

            var re = post<ResponseData<string>>(CompleteTaskApiUrl, data);
            return re;
        }



        /// <summary>
        /// 发送post请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据 格式 value=xxx</param>
        /// <returns>json</returns>
        public static T post<T>(string url, string data)
        {

            var request = new AuthenticatedRequest();
            var accessToken = request.GetCookie(Constants.AuthKeyIdentityServer);
            if (accessToken == "")
            {
                throw new AuthenticationException("accessToken 为空");
            }

            var re = HttpHelper.HttpPost(url, data, new List<string>() {$"Authorization:Bearer {accessToken}"});

            var json = JsonConvert.DeserializeObject<T>(re);

            return json;

        }

    }


    public enum MessageType
    {
        消息,
        任务
    }
}
