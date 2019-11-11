using System;
using System.Diagnostics;
using System.Linq;
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
            Console.WriteLine("Demo TrimExcess");

            currentCapacity = myList.Capacity;

            myList.TrimExcess();

            Console.WriteLine($"Trimmed from {currentCapacity} to {myList.Capacity}");

            // Demo Clear
            Console.WriteLine("Demo Clear");

            var currentCount = myList.Count;

            myList.Clear();

            Console.WriteLine($"Cleared from count {currentCount} to {myList.Count}");

            // Demo Add
            Console.WriteLine("Demo Add");

            myList = new MyList<int>(3) { 1, 2, 3 };

            myList.Add(5000);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo AddRange
            Console.WriteLine("Demo AddRange");

            myList = new MyList<int>(3) { 1, 2, 3 };

            myList.AddRange(new [] { 10, 10, 10, 10, 10, 10, 10 });

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo Insert
            Console.WriteLine("Demo Insert");

            myList = new MyList<int>(3) { 1, 2, 3 };

            myList.Insert(2, 2500);
            myList.Insert(4, 2500);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo InsertRange
            Console.WriteLine("Demo InsertRange");

            // Performance Tests (see region tags in InsertRange())
            // Comparison between Copy then Shuffle experiment
            // Comparison between giving up CPU vs Memory experiment
            // myList = new MyList<int>();

            // int[] aBunchOfItems = Enumerable.Range(0, 100000000).ToArray();

            // myList.AddRange(aBunchOfItems);

            // Stopwatch timer = new Stopwatch();

            // timer.Start();

            // myList.InsertRange(5, aBunchOfItems);

            // timer.Stop();

            // Console.WriteLine(timer.ElapsedTicks / (float)Stopwatch.Frequency);
            // Console.WriteLine($"New Count: {myList.Count}");

            myList = new MyList<int>() { 1, 2, 3 };

            myList.InsertRange(1, new [] { 10, 10, 10, 10, 10, 10, 10 });

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo GetRange
            Console.WriteLine("Demo GetRange");

            myList = new MyList<int>() { 0, 1, 2, 3, 4, 5, 6 };

            var index = 2;
            var returnedList = myList.GetRange(index, myList.Count - index);

            foreach (int item in returnedList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {returnedList.Count}");

            // Demo RemoveAt
            Console.WriteLine("Demo RemoveAt");

            myList = new MyList<int>() { 0, 1, 2, 3, 4, 5, 6 };

            myList.RemoveAt(3);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo Remove
            Console.WriteLine("Demo Remove");

            myList = new MyList<int>() { 0, 1, 0, 1, 0 };

            myList.Remove(1);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo RemoveAll
            Console.WriteLine("Demo RemoveAll");

            myList = new MyList<int>() { 2, 0, 1, 0, 5 };

            myList.RemoveAll(x => x > 0);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo RemoveRange
            Console.WriteLine("Demo RemoveRange");

            myList = new MyList<int>() { 2, 0, 0, 0, 5 };

            myList.RemoveRange(1, 3);

            foreach (int item in myList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($"New Count: {myList.Count}");

            // Demo TrueForAll
            Console.WriteLine("Demo TrueForAll");

            myList = new MyList<int>() { 1, 2, 3 };

            Console.WriteLine(myList.TrueForAll(x => x < 4));
            Console.WriteLine(myList.TrueForAll(x => x > 1));

            // Demo Exists
            Console.WriteLine("Demo Exists");

            myList = new MyList<int>() { 1, 2, 3 };

            Console.WriteLine(myList.Exists(x => x > 1));
            Console.WriteLine(myList.Exists(x => x > 3));

            // Demo ConvertAll
            Console.WriteLine("Demo ConvertAll");

            myList = new MyList<int>() { 1, 2, 3};

            MyList<string> myListString = myList.ConvertAll(x => x.ToString());

            myListString.ForEach(x =>
            {
                Console.WriteLine(x is string);
                Console.WriteLine(x);
            });

            // Demo Find and FindLast
            Console.WriteLine("Demo Find and FindLast and FindAll");

            myList = new MyList<int>() { 1, 4, 2, 3, 1, 2, 3, 4, 3, 4, 3 };

            Console.WriteLine(myList.Find(x => x == 3));
            Console.WriteLine(myList.FindLast(x => x == 4));

            var allItems = myList.FindAll(x => x == 3);

            allItems.ForEach(x => Console.WriteLine(x));
        }
    }
}
