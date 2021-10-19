using System.Threading;

namespace Kitchen.CookingApparatus
{
    public class Oven
    {
        private Mutex _mutex; 
        
        public enum OvenStatus
        {
            Free,
            Busy
        }

        public OvenStatus Status;

        public Oven()
        {
            _mutex = new Mutex();
            ChangeStatusToFree();
            
        }

        public bool UseOven()
        {
            _mutex.WaitOne();
            if (Status == OvenStatus.Busy)
            {
                _mutex.ReleaseMutex();
                return false;
            }
            Status = OvenStatus.Busy;
            _mutex.ReleaseMutex();
            return true;
        }

        public void StopUsingOven()
        {
            Status = OvenStatus.Free;
            
        }

        private void ChangeStatusToFree()
        {
            Status = OvenStatus.Free;
        }
        
        
        
    }
}