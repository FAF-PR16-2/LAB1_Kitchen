﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Kitchen.CookingApparatus;

namespace Kitchen
{
    public class Cook
    {
        public int  Id => _id;

        private int _id;
        
        private int _rank;
        private int _proficiency;
        private ItemsBuilder _itemsBuilder;
        private ItemData[] _possibleItemsData;

        public readonly string Name;
        public readonly string CatchPhrase;
        
        
        private Thread _thread;

        private CooksThread[] _cooksThreads;

        private Stopwatch _stopwatch;
        
        public Cook(int id, string name, string catchPhrase, int rank, int proficiency)
        {
            _id = id;
            _rank = rank;
            _proficiency = proficiency;
            Name = name;
            CatchPhrase = catchPhrase;
            _itemsBuilder = new ItemsBuilder();
            _possibleItemsData = _itemsBuilder.GetItems();

            SetupCookThreads();
            
            _stopwatch = new Stopwatch();

            _thread = new Thread(Update);
        }

        public void Start()
        {
            foreach (var cooksThread in _cooksThreads)
            {
                cooksThread.Start();
            }
            
            _stopwatch.Start();
            _thread.Start();

        }

        public void Update()
        {
            while (true)
            {
                Thread.Sleep(Configuration.TimeUnit);
                _stopwatch.Stop();

                foreach (var cooksThread in _cooksThreads)
                {
                    cooksThread.Update(_stopwatch.ElapsedMilliseconds);
                }
                _stopwatch.Restart();
                
            }
        }

        public (ItemFromOrderData, long) GetOrder()
        {
            var itemFromOrderDatas = KitchenManager.Instance().GetListOfItemsFromOrderData();
            
            if (itemFromOrderDatas.Count == 0)
                return (new ItemFromOrderData(), -1);

            ItemFromOrderData itemToReturn = null;
            
            bool skippedOven = false;
            bool skippedStove = false;

            foreach (var item in itemFromOrderDatas)
            {
                if (item.complexity <= _rank)
                {
                    var itemData = _itemsBuilder.GetItemDataByItemId(item.item_id).cooking_apparatus; 
                    if (itemData != null)
                    {
                        switch (itemData)
                        {
                            case "oven":
                                if (KitchenManager.Instance().KitchenSetup.Ovens
                                    .Any(o => o.Status == Oven.OvenStatus.Free))
                                    itemToReturn = item;
                                else
                                {
                                    skippedOven = true;
                                }
                                break;
                            case "stove":
                                if (KitchenManager.Instance().KitchenSetup.Stoves
                                    .Any(o => o.Status == Stove.StoveStatus.Free))
                                    itemToReturn = item;
                                else
                                {
                                    skippedStove = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        itemToReturn = item;
                    }

                    if (itemToReturn != null)
                        break;
                }
            }

            if (itemToReturn == null)
            {

                if (skippedOven)
                {
                    foreach (var item in itemFromOrderDatas)
                    {
                        if (item.complexity <= _rank)
                        {
                            var itemData = _itemsBuilder.GetItemDataByItemId(item.item_id).cooking_apparatus;
                            if (itemData == "oven")
                            {
                                itemToReturn = item;
                            }
                        }
                    }
                }

                if (skippedStove)
                {
                    foreach (var item in itemFromOrderDatas)
                    {
                        if (item.complexity <= _rank)
                        {
                            var itemData = _itemsBuilder.GetItemDataByItemId(item.item_id).cooking_apparatus;
                            if (itemData == "stove")
                                itemToReturn = item;
                        }
                    }
                }
            }

            if (itemToReturn == null)
                return (new ItemFromOrderData(), -1);

            bool check =  KitchenManager.Instance().RemoveItemFromListOfItemsFromOrderData(itemToReturn);

            if (!check)
                return GetOrder();
            
            itemToReturn.cook_id = Id;
            return (itemToReturn, _itemsBuilder.GetItemDataByItemId(itemToReturn.item_id).preparation_time);
        } 


        private void SetupCookThreads()
        {
            List<CooksThread> cooksThreadsTemp = new List<CooksThread>();
            foreach (var _ in Enumerable.Range(0, _proficiency))
            {
                cooksThreadsTemp.Add(new CooksThread(this));
            }

            _cooksThreads = cooksThreadsTemp.ToArray();
        }
    }
}