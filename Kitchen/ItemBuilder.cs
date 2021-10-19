using System.Collections.Generic;

namespace Kitchen
{
    public class ItemsBuilder
    {
        private List<ItemData> _items;

        public ItemData[] GetItems()
        {
            _items = new List<ItemData>
            {
                BuildPizza(),
                BuildSalad(),
                BuildZeama(),
                BuildScallopSashimiWithMeyerLemonConfit(),
                BuildIslandDuckWithMulberryMustard(),
                BuildWaffles(),
                BuildAubergine(),
                BuildLasagna(),
                BuildBurger(),
                BuildGyros()
            };

            return _items.ToArray();
        }

        public ItemData GetItemDataByItemId(int id)
        {
            foreach (var item in _items)
            {
                if (item.id == id)
                    return item;
            }

            return null;
        }

        private ItemData BuildPizza()
        {
            var item = new ItemData()
            {
                id = 1,
                name = "pizza",
                preparation_time = 20,
                complexity = 2,
                cooking_apparatus = "oven"
            };
            return item;
        }
        
        private ItemData BuildSalad()
        {
            var item = new ItemData()
            {
                id = 2,
                name = "salad",
                preparation_time = 10,
                complexity = 1,
                cooking_apparatus = null
            };
            return item;
        }
        
        private ItemData BuildZeama()
        {
            var item = new ItemData()
            {
                id = 3,
                name = "zeama",
                preparation_time = 7,
                complexity = 1,
                cooking_apparatus = "stove"
            };
            return item;
        }
        
        private ItemData BuildScallopSashimiWithMeyerLemonConfit()
        {
            var item = new ItemData()
            {
                id = 4,
                name = "Scallop Sashimi with Meyer Lemon Confit",
                preparation_time = 32,
                complexity = 3,
                cooking_apparatus = null
            };
            return item;
        }
        
        private ItemData BuildIslandDuckWithMulberryMustard()
        {
            var item = new ItemData()
            {
                id = 5,
                name = "Island Duck with Mulberry Mustard",
                preparation_time = 35,
                complexity = 3,
                cooking_apparatus = "oven"
            };
            return item;
        }
        
        private ItemData BuildWaffles()
        {
            var item = new ItemData()
            {
                id = 6,
                name = "Waffles",
                preparation_time = 10,
                complexity = 1,
                cooking_apparatus = "stove"
            };
            return item;
        }

        private ItemData BuildAubergine()
        {
            var item = new ItemData()
            {
                id = 7,
                name = "Aubergine",
                preparation_time = 20,
                complexity = 2,
                cooking_apparatus = null
            };
            return item;
        }
        
        private ItemData BuildLasagna()
        {
            var item = new ItemData()
            {
                id = 8,
                name = "Lasagna",
                preparation_time = 30,
                complexity = 2,
                cooking_apparatus = "oven"
            };
            return item;
        }
        
        private ItemData BuildBurger()
        {
            var item = new ItemData()
            {
                id = 9,
                name = "pizza",
                preparation_time = 15,
                complexity = 1,
                cooking_apparatus = "oven"
            };
            return item;
        }
        
        private ItemData BuildGyros()
        {
            var item = new ItemData()
            {
                id = 10,
                name = "Gyros",
                preparation_time = 15,
                complexity = 1,
                cooking_apparatus = null
            };
            return item;
        }
    }
}