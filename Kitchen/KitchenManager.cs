using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kitchen.Singleton;

namespace Kitchen
{
    public class KitchenManager : Singleton<KitchenManager>
    {
        public KitchenSetup KitchenSetup;

        private ItemsBuilder _itemsBuilder;

        private List<DistributionData> _distributionDatas;
        private List<ItemFromOrderData> _rawListItemsToPrepare;
        private List<ItemFromOrderData> _sortedListItemsToPrepare;

        private Mutex _mutexForRemoving;
        private Mutex _mutexForFinishing;

        public KitchenManager()
        {
            _distributionDatas = new List<DistributionData>();
            _rawListItemsToPrepare = new List<ItemFromOrderData>();
            _sortedListItemsToPrepare = new List<ItemFromOrderData>();

            _itemsBuilder = new ItemsBuilder();
            _itemsBuilder.GetItems(); // ye i know

            _mutexForRemoving = new Mutex();
            _mutexForFinishing = new Mutex();

            //KitchenSetup should be setuped before start in main function
        }

        public void ReceiveOrder(OrderData orderData)
        {
            _distributionDatas.Add(new DistributionData()
            {
                order_id = orderData.order_id,
                table_id = orderData.table_id,
                waiter_id = orderData.waiter_id,
                items = orderData.items,
                priority = orderData.priority,
                max_wait = orderData.max_wait,
                pick_up_time = orderData.pick_up_time,
                cooking_details = new Dictionary<string, int>[orderData.items.Length]
            });

            foreach (var item in orderData.items)
            {
                _rawListItemsToPrepare.Add(new ItemFromOrderData
                {
                    order_id = orderData.order_id,
                    item_id = item,
                    priority = orderData.priority,
                    pick_up_time = orderData.pick_up_time,
                    
                    complexity = _itemsBuilder.GetItemDataByItemId(item).complexity
                });
            }

            SortRawList();
        }

        public List<ItemFromOrderData> GetListOfItemsFromOrderData()
        {
            return new List<ItemFromOrderData>(_sortedListItemsToPrepare);
        }

        public bool RemoveItemFromListOfItemsFromOrderData(ItemFromOrderData itemFromOrderData)
        {
            _mutexForRemoving.WaitOne();
            if (_sortedListItemsToPrepare.Contains(itemFromOrderData))
                _sortedListItemsToPrepare.Remove(itemFromOrderData);
            else
            {
                _mutexForRemoving.ReleaseMutex();
                return false;
            }
            
            _mutexForRemoving.ReleaseMutex();
            return true;
        }

        public void FinishItemFromOrder(ItemFromOrderData itemFromOrderData)
        {
            _mutexForFinishing.WaitOne();
            
            foreach (var distributionData in _distributionDatas)
            {
                if (distributionData.order_id == itemFromOrderData.order_id)
                {
                    for (int i = 0; i < distributionData.cooking_details.Length; i++)
                    {
                        if (distributionData.cooking_details[i] == null)
                        {
                            distributionData.cooking_details[i] = new Dictionary<string, int>()
                            {
                                ["food_id"] = itemFromOrderData.item_id,
                                ["cook_id"] = itemFromOrderData.cook_id
                            };
                            break;
                        }
                    }

                    if (distributionData.cooking_details.Any(t => t == null))
                    {
                        _mutexForFinishing.ReleaseMutex();
                        return;
                    }
                    
                    _distributionDatas.Remove(distributionData);
                    SendRequestWithFinishedOrder(distributionData);
                    _mutexForFinishing.ReleaseMutex();
                    return;
                }
            }
            
            Console.WriteLine("got here somehow");
            _mutexForFinishing.ReleaseMutex();
        }

        private void SendRequestWithFinishedOrder(DistributionData distributionData)
        {
            RequestsSender.SendOrderRequest(distributionData);
            Console.WriteLine("order with id = " + distributionData.order_id + " was sent");
            // todo
        }

        private void SortRawList()
        {
            foreach (var item in _rawListItemsToPrepare)
            {
                _sortedListItemsToPrepare.Add(item);
            }
            
            _sortedListItemsToPrepare = 
                _sortedListItemsToPrepare.OrderByDescending(item => item.priority)
                    .ThenBy(item => item.pick_up_time)
                    .ThenByDescending(item => item.complexity).ToList();

            _rawListItemsToPrepare.Clear();
        }
        
        
    }
}