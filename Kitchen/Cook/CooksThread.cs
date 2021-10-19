using System;
using System.Diagnostics;
using System.Threading;

namespace Kitchen
{
    public class CooksThread
    {
        public enum CookThreadStatus
        {
            Free,
            Busy
        }

        public CookThreadStatus Status;

        private Cook _cook;
        private DistributionData _currentOrder;

        private long _currentPreparingTime;
        private long _finishPreparingTime;
        
        public CooksThread(Cook masterCook)
        {
            _cook = masterCook;
            _currentPreparingTime = -1;
        }

        public void Start()
        {
            Status = CookThreadStatus.Free;
        }

        public void Update(long deltaTime)
        {
            if (Status == CookThreadStatus.Free)
            {
                var (distributionData, finishDeltaTime) = _cook.GetOrder();
                
                GetOrder(distributionData, finishDeltaTime);
            }
            else
            {
                ContinuePreparingOrder(deltaTime);
            }
        }

        private void GetOrder(DistributionData distributionData, long finishDeltaTime)
        {
            if (finishDeltaTime == -1)
                return;

            Status = CookThreadStatus.Busy;

            _currentOrder = distributionData;
            StartPreparingOrder(finishDeltaTime);

            //todo maybe i forgot something here
        }

        private void StartPreparingOrder(long finishDeltaTime)
        {
            _currentPreparingTime = 0;
            _finishPreparingTime = finishDeltaTime;
        }

        private void ContinuePreparingOrder(long deltaTime)
        {
            if (_currentPreparingTime == -1)
                return;
            
            _currentPreparingTime += deltaTime;

            if (_currentPreparingTime >= _finishPreparingTime)
            {
                SendOrder();
            }
        }

        private void SendOrder()
        {
            //todo

            Console.WriteLine("order is done");

            Status = CookThreadStatus.Free;
            _currentPreparingTime = -1;
        }
        
    }
}