namespace Kitchen
{
    public class OrderData
    {
        public int order_id { get; set; }
        public int table_id { get; set; }
        public int waiter_id { get; set; }
        public int[] items { get; set; }
        public int priority { get; set; }
        public int max_wait { get; set; }
        public long pick_up_time { get; set; }
        
        public override string ToString()
        {
            string itemsToString = "";

            foreach (var item in items)
            {
                itemsToString += item + " ";
            }
            
            return base.ToString() + "\n" +
                   "order_id: " + order_id + "\n" +
                   "table_id: " + table_id + "\n" +
                   "waiter_id: " + waiter_id + "\n" +
                   "items: " + itemsToString + "\n" +
                   "priority: " + priority + "\n" +
                   "max_wait: " + max_wait + "\n" +
                   "pick_up_time: " + pick_up_time;
        }
    }
}