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

        public bool UseStove()
        {
            _mutex.WaitOne();
            if (Status == StoveStatus.Busy)
            {
                _mutex.ReleaseMutex();
                return false;
            }
            Status = StoveStatus.Busy;
            _mutex.ReleaseMutex();
            return true;
        }

        public void StopUsingStove()
        {
            Status = StoveStatus.Free;
        }

        private void ChangeStatusToFree()
        {
            Status = StoveStatus.Free;
        }
    }
}