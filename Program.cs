using System;
using CollectionsDemo.Collections;

namespace CollectionsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList<int> myList = new MyList<int>();

            myList.Add(12);
            myList.Add(10);
            myList.Add(5);
            myList.Add(8);
            myList.Add(100);
            myList.Add(12);

            foreach(int item in myList)
            {
                Console.WriteLine(item);
            }

            foreach(int item in myList)
            {
                Console.WriteLine(item);
            }
        }
    }
}
