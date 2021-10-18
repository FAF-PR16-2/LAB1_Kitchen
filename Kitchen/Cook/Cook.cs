using System;
using System.Collections.Generic;
using System.Linq;

namespace Kitchen
{
    public class Cook
    {
        private int _rank;
        private int _proficiency;
        public readonly string Name;
        public readonly string CatchPhrase;

        private CooksThread[] _cooksThreads;
        
        public Cook(string name, string catchPhrase, int rank, int proficiency)
        {
            _rank = rank;
            _proficiency = proficiency;
            Name = name;
            CatchPhrase = catchPhrase;

            SetupCookThreads();

            foreach (var cooksThread in _cooksThreads)
            {
                cooksThread.Run();
            }
        }

        public void Run()
        {
            
        }


        private void SetupCookThreads()
        {
            List<CooksThread> cooksThreadsTemp = new List<CooksThread>();
            foreach (var _ in Enumerable.Range(0, _proficiency))
            {
                cooksThreadsTemp.Add(new CooksThread());
            }

            _cooksThreads = cooksThreadsTemp.ToArray();
        }
    }
}