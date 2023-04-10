using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK_EAM_Lib
{
    [System.Serializable]
    public class AccountOrders
    {
        public List<OrderData> orderData = new List<OrderData>();
    }

     [System.Serializable]
    public class OrderData
    {
        public int index;
        public string email;
    }
}
