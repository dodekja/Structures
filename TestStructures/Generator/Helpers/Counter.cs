using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestStructures.Generator.Helpers
{
    public class Counter
    {
        public int MaxCount { get; set; }
        public int CurrentCount { get; set; }

        public Counter(int maxCount)
        {
            MaxCount = maxCount;
            CurrentCount = 0;
        }

        public void Increment()
        {
            if (CurrentCount >= MaxCount)
            {
                throw new InvalidOperationException("Max Count Reached");
            }
            CurrentCount++;
        }

        public bool IsCountReached()
        {
            return CurrentCount == MaxCount;
        }
    }
}
