using System;
using CollectionsDemo.Collections;

namespace CollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList<int> myList = new MyList<int>() { 1, 2, 3 };

            // Demo Enumerator
            Console.WriteLine("Demo Enumerator");

            foreach(int item in myList)
            {
                Console.WriteLine(item);
            }

            // Demo Resize and Capacity
            Console.WriteLine("Demo Resize and Capacity");

            var currentCapacity = myList.Capacity;

            for (int i = 0; i < 500; i++)
            {
                myList.Add(i);

                if (currentCapacity != myList.Capacity)
                {
                    Console.WriteLine($"Resized too {myList.Capacity}");

                    currentCapacity = myList.Capacity;
                }
            }

            // Demo Resize and Initial Capacity
            Console.WriteLine("Demo Resize and Initial Capacity");

            myList = new MyList<int>(500){ 1 };

            currentCapacity = myList.Capacity;

            for (int i = 0; i < 500; i++)
            {
                myList.Add(i);

                if (currentCapacity != myList.Capacity)
                {
                    Console.WriteLine($"Resized too {myList.Capacity}");

                    currentCapacity = myList.Capacity;
                }
            }

            // Demo TrimExcess
            Console.WriteLine("Demo Trim Excess");

            currentCapacity = myList.Capacity;

            myList.TrimExcess();

            Console.WriteLine($"Trimmed from {currentCapacity} to {myList.Capacity}");

            // Demo Clear
            Console.WriteLine("Demo Clear");

            var currentCount = myList.Count;

            myList.Clear();

            Console.WriteLine($"Cleared from count {currentCount} to {myList.Count}");
        }
    }
}
