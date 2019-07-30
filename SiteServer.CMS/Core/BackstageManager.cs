using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="remark">用于快速完成的标识</param>
        public static void SendMessage(MessageType type, List<string> receiverIds, string content, string remark = "")
        {
            LogUtils.AddAdminLog("aibol系统日志",$"发送消息任务{Enum.GetName(typeof(MessageType),type)},{string.Join(",",receiverIds)},{content},{remark}");
        }

        
    }
    public enum MessageType
    {
        消息,
        任务
    }
}
