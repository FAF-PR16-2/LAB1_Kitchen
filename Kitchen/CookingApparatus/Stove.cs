using System.Threading;

namespace Kitchen.CookingApparatus
{
    public class Stove
    {
        public enum StoveStatus
        {
            Free,
            Busy
        }

        public StoveStatus Status;
        
        private Mutex _mutex; 
        
        public Stove()
        {
            ChangeStatusToFree();
            _mutex = new Mutex();
        }

        public bool UseOven()
        {
            _mutex.WaitOne();
            if (Status == StoveStatus.Busy)
                return false;
            Status = StoveStatus.Busy;
            _mutex.ReleaseMutex();
            return true;
        }

        public void StopUsingOven()
        {
            Status = StoveStatus.Free;
        }

        private void ChangeStatusToFree()
        {
            Status = StoveStatus.Free;
        }
    }
}