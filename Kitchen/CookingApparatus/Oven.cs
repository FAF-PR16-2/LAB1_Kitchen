namespace Kitchen.CookingApparatus
{
    public class Oven
    {
        public enum OvenStatus
        {
            Free,
            Busy
        }

        public OvenStatus Status;

        public Oven()
        {
            ChangeStatusToFree();
        }

        public void UseOven()
        {
            
        }

        private void ChangeStatusToFree()
        {
            Status = OvenStatus.Free;
        }
        
        
        
    }
}