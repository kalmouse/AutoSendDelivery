using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSendDelivery
{
    class staticValue
    {
        /// <summary>
        /// 中铁
        /// </summary>
        public static int projectId_CREC = 42;

        static string api_url = System.Configuration.ConfigurationManager.AppSettings["API_URL"];
        public static string deliveryUrl = api_url + "api/DivideOrder/Post_pushDeliveryOrder";
    }
}
