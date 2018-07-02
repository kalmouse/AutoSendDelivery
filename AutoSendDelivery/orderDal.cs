using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using LeadingClass;

namespace AutoSendDelivery
{
    class orderDal
    {
        DBOperate db = new DBOperate();
        /// <summary>
        /// 根据projectId获取已确认的订单
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public DataSet getOrder(int projectId)
        {
            DateTime timeNow = DateTime.Now;
            int nowHour = timeNow.Hour;
            switch (nowHour)
            {
                case 8: timeNow = timeNow.AddHours(-14); break;
                default: timeNow = timeNow.AddHours(-2); break;
            }
            string sql = string.Format(@"select o.Id from [order] o join tpi_order tpio on o.tpi_orderId=tpio.Id where projectId={0} and IsDelete=0 and RelationOrderId < 1
                                and (tpio.OutboundStatus!='推送成功！' or tpio.OutboundStatus is null) and o.OrderTime>'{1}' and o.OrderTime <'{2}'"
                ,projectId, DateTime.Now.AddDays(-4).ToString("yyyy-MM-dd HH:mm:ss"), timeNow.ToString("yyyy-MM-dd HH:00:00"));
            return db.GetDataSet(sql);
        }
    }
}
