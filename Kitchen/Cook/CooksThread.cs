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
        private ItemFromOrderData _currentOrder;

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
                var (itemFromOrderData, finishDeltaTime) = _cook.GetOrder();
                
                GetOrder(itemFromOrderData, finishDeltaTime);
            }
            else
            {
                ContinuePreparingOrder(deltaTime);
            }
        }

        private void GetOrder(ItemFromOrderData itemFromOrderData, long finishDeltaTime)
        {
            if (finishDeltaTime == -1)
                return;

            Status = CookThreadStatus.Busy;

            _currentOrder = itemFromOrderData;
            StartPreparingOrder(finishDeltaTime);

            //todo use oven or whatever here
        }

        private void StartPreparingOrder(long finishDeltaTime)
        {
            _currentPreparingTime = 0;
            _finishPreparingTime = finishDeltaTime * Configuration.TimeUnit;
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
            KitchenManager.Instance().FinishItemFromOrder(_currentOrder);

            Console.WriteLine("item is done");

            Status = CookThreadStatus.Free;
            _currentPreparingTime = -1;
        }
        
    }
}