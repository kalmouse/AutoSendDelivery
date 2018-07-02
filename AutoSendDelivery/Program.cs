using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadingClass;
using CommenClass;
using System.Data;
using System.Web;
using System.Net;
using System.IO;

namespace AutoSendDelivery
{
    class Program
    {
        static void Main(string[] args)
        {
            string opreationLog = "";
            string orderIds = "";
            orderDal orderDal = new orderDal();
            string projectIds = System.Configuration.ConfigurationManager.AppSettings["ProjectIds"].ToString();
            List<int> projectIdList = dataTypeChange.strArrayToInt(projectIds);
            foreach (var projectId in projectIdList)
            {
                try
                {
                    DataTable dt = orderDal.getOrder(projectId).Tables[0];
                    foreach (DataRow item in dt.Rows)
                    {
                        orderIds += "," + item["Id"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(orderIds))
                    {
                        orderIds = orderIds.Substring(1);
                        string url = staticValue.deliveryUrl;
                        string param = "ProjectId=" + projectId + "&OrderId=" + orderIds;
                        string returnMeg = SendRequest(url, param);
                        opreationLog = "调用接口成功！";
                    }
                    else
                    {
                        opreationLog = "没有待发货订单！";
                    }
                }
                catch (Exception ex)
                {
                    opreationLog = ex.Message;
                }
                TPI_OperationLog tpi_operationlog = new TPI_OperationLog();
                tpi_operationlog.ProjectId = projectId;
                tpi_operationlog.Url = "自动发货程序";
                tpi_operationlog.UpdateTime = DateTime.Now;
                tpi_operationlog.Operation = "自动发货";
                tpi_operationlog.PushState = "";
                tpi_operationlog.ReturnMsg = opreationLog;
                tpi_operationlog.UserId = 0;
                tpi_operationlog.Save();
            }
        }

        /// <summary>
        /// 发送请求 调用AllInterface里的接口进行上传
        /// </summary>
        /// <param name="goodsId">商品编号</param>
        /// /// <param name="project">项目编号</param>
        private static string SendRequest(string url, string param)
        {
            string resultMsg = "";
            byte[] bs = Encoding.ASCII.GetBytes(param);
            ////建立一个http请求，请求ALLinterface。           
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bs.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            using (WebResponse wr = request.GetResponse())
            {
                Stream responseStream = wr.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                resultMsg = streamReader.ReadToEnd();
            }
            return resultMsg;
        }
    }
}
