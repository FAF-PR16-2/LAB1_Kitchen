using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Kitchen
{
    public class Cook
    {
        private int _rank;
        private int _proficiency;
        public readonly string Name;
        public readonly string CatchPhrase;


        private Thread _thread;
        
        private CooksThread[] _cooksThreads;

        private Stopwatch _stopwatch;
        
        public Cook(string name, string catchPhrase, int rank, int proficiency)
        {
            _rank = rank;
            _proficiency = proficiency;
            Name = name;
            CatchPhrase = catchPhrase;

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
                Thread.Sleep(1); //todo add from config
                _stopwatch.Stop();
                Console.WriteLine(_stopwatch.ElapsedMilliseconds);
                
                foreach (var cooksThread in _cooksThreads)
                {
                    cooksThread.Update(_stopwatch.ElapsedMilliseconds);
                }
                _stopwatch.Restart();
                
            }
        }

        public (DistributionData, long) GetOrder()
        {
            
            return (new DistributionData(), -1);
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