namespace Kitchen
{
    public class ItemData
    {
        public int id;
        public string name;
        public int preparation_time;
        public int complexity;
        public string cooking_apparatus;

        public override string ToString()
        {
            return base.ToString() + "\n" +
                   "id: " + id + "\n" +
                   "name: " + name + "\n" +
                   "preparation_time: " + preparation_time + "\n" +
                   "complexity: " + complexity + "\n" +
                   "cooking_apparatus: " + cooking_apparatus;
        }
    }
}