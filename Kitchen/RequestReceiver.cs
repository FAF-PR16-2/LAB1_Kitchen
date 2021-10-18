using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kitchen
{
    public class RequestReceiver
    {
        public static List<OrderData> OrderDatas = new List<OrderData>();  

        public static async Task GetOrder(OrderData orderData)
        {
            if (OrderDatas.Count > 0)
                Console.WriteLine(OrderDatas[^1]);
            await new Task(() => OrderDatas.Add(orderData));
            
        }
    }
}