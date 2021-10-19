namespace Kitchen
{
    public class ItemFromOrderData
    {
        public int order_id { get; set; }
        public int item_id { get; set; }
        public int priority { get; set; }
        public long pick_up_time { get; set; }

        public int complexity { get; set; }
        public int cook_id { get; set; }
        
    }
}