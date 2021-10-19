using System;
using System.Diagnostics;
using System.Threading;
using Kitchen.CookingApparatus;

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

        private bool _isWaitingForOven;
        private bool _isWaitingForStove;

        private Oven _currentOven;
        private Stove _currentStove;
        
        public CooksThread(Cook masterCook)
        {
            _cook = masterCook;
            _currentPreparingTime = -1;
        }

        public void Start()
        {
            _isWaitingForOven = false;
            _isWaitingForStove = false;
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
            switch (_cook.ItemsBuilder.GetItemDataByItemId(_currentOrder.item_id).cooking_apparatus)
            {
                case "oven":
                    _isWaitingForOven = true;
                    break;
                case "stove":
                    _isWaitingForStove = true;
                    break;
            }
            
            if (!_isWaitingForOven && !_isWaitingForStove)
                _currentPreparingTime = 0;
            _finishPreparingTime = finishDeltaTime * Configuration.TimeUnit;
        }

        private void ContinuePreparingOrder(long deltaTime)
        {
            if (_currentPreparingTime == -1)
                if (!TryUseCookingApparatus())
                    return;
            
            _currentPreparingTime += deltaTime;

            if (_currentPreparingTime >= _finishPreparingTime)
            {
                SendOrder();
            }
        }

        private bool TryUseCookingApparatus()
        {
            if (_isWaitingForOven)
            {
                foreach (var oven in KitchenManager.Instance().KitchenSetup.Ovens)
                {
                    if (oven.UseOven())
                    {
                        _currentPreparingTime = 0;
                        _currentOven = oven;
                        return true;
                    }
                }
            }
            else if (_isWaitingForStove)
            {
                foreach (var stove in KitchenManager.Instance().KitchenSetup.Stoves)
                {
                    if (stove.UseStove())
                    {
                        _currentPreparingTime = 0;
                        _currentStove = stove;
                        return true;
                    }
                }
            }

            return false;
        }

        private void SendOrder()
        {
            Console.WriteLine("item is done");
            KitchenManager.Instance().FinishItemFromOrder(_currentOrder);

            _isWaitingForOven = false;
            _isWaitingForStove = false;

            if (_currentOven != null)
            {
                _currentOven.StopUsingOven();
                _currentOven = null;
            }

            if (_currentStove != null)
            {
                _currentStove.StopUsingStove();
                _currentStove = null;
            }
            
            Status = CookThreadStatus.Free;
            _currentPreparingTime = -1;
        }
        
    }
}