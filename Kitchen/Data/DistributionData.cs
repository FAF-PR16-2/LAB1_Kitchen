using System;
using System.Collections.Generic;

namespace Kitchen
{
    public class DistributionData
    {
        public int order_id { get; set; }
        public int table_id { get; set; }
        public int waiter_id { get; set; }
        public int[] items { get; set; }
        public int priority { get; set; }
        public int max_wait { get; set; }
        public int pick_up_time { get; set; }
        public int cooking_time { get; set; }
        public Dictionary<string, int>[] cooking_details { get; set; }

        public override string ToString()
        {
            string itemsToString = "";
            string cooking_detailsToString = "";

            foreach (var item in items)
            {
                itemsToString += item + " ";
            }

            foreach (var cookingDetail in cooking_details)
            {
                cooking_detailsToString += "\n{\n" + string.Join(Environment.NewLine, cookingDetail) + "\n}";
            }
            
            
            
            return base.ToString() + "\n" +
                   "order_id: " + order_id + "\n" +
                   "table_id: " + table_id + "\n" +
                   "waiter_id: " + waiter_id + "\n" +
                   "items: " + itemsToString + "\n" +
                   "priority: " + priority + "\n" +
                   "max_wait: " + max_wait + "\n" +
                   "pick_up_time: " + pick_up_time + "\n" +
                   "cooking_time: " + cooking_time + "\n" +
                   "cooking_details: " + cooking_detailsToString;
        }
        
        
    }
}