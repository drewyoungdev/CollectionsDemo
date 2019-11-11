using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsDemo.Collections
{
    public class MyList<T> : IEnumerable<T>
    {
        /// <summary>
        /// Allow MyList<T>'s items to be accessed through index
        /// If an index that exceeds the number of items within the underlying Items [], throw exception
        /// <param name="index">The index return and set items in the underlying Items[] at</param>
        /// </summary>
        public T this[int index]
        {
            get
            {
                CheckIndexBoundary(index);

                return Items[index];
            }
            set
            {
                CheckIndexBoundary(index);

                Items[index] = value;
            }
        }

        /// <summary>
        /// The length of the underlying Items []
        /// </summary>
        public int Capacity => Items.Length;

        /// <summary>
        /// The number of items in the underlying Items [].
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// The underlying Items []
        /// </summary>
        T[] Items;

        /// <summary>
        /// Initialize MyList at a given capacity
        /// <param name="capacity">The size to initialize the underlying Items [] with</param>
        /// </summary>
        public MyList(int capacity = 5)
        {
            Items = new T[capacity];
        }

        /// <summary>
        /// Resizes underlying Items [] to array size of the current Count
        /// </summary>
        public void TrimExcess()
        {
            if (Count < Capacity)
            {
                Array.Resize(ref Items, Count);
            }
        }

        /// <summary>
        /// Reset Count to zero and nullify reference types and abandon value types
        /// Note: this does NOT resize the underlying Items [] in case user re-adds items into MyList
        /// </summary>
        public void Clear()
        {
            Count = 0;

            // Pre-mature Optimization to avoid for-loop:
            // if (typeof(T).BaseType.Equals(typeof(ValueType)))
            // {
            //     return;
            // }

            // Nullify Reference Types to allow GC or Abandon Value Types
            for (int i = 0; i < Count; i++)
            {
                Items[i] = default(T);
            }
        }

        /// <summary>
        /// Add new item to end of Items []
        /// <param name="item">The item to be added at the end of the underlying Items []</param>
        /// </summary>
        public void Add(T item)
        {
            EnsureCapacity();

            Items[Count++] = item;
        }

        /// <summary>
        /// Add new collection to end of Items []
        /// <param name="collection">The collection to be added at the end of the underlying Items []</param>
        /// </summary>
        public void AddRange(IEnumerable<T> collection)
        {
            int collectionCount = collection.Count();

            EnsureCapacity(collectionCount);

            // Copy items from incoming collection into underlying Items []
            foreach (T item in collection)
            {
                this.Items[Count++] = item;
            }
        }

        /// <summary>
        /// Add new item at the specified index in the underlying Items []
        /// <param name="index">The index to add the specified item</param>
        /// <param name="item">The item to add to the specified index</param>
        /// </summary>
        public void Insert(int index, T item)
        {
            CheckIndexBoundary(index);

            EnsureCapacity();

            Array.Copy(
                sourceArray: this.Items,
                sourceIndex: index,
                destinationArray: this.Items,
                destinationIndex: index + 1,
                length: Count - index);

            this.Items[index] = item;

            Count++;
        }

        /// <summary>
        /// Inserts collection into underlying Items [] at the specified index
        /// <param name="index">The index to add the specified item</param>
        /// <param name="collection">The collection to be inserted</param>
        /// </summary>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            CheckIndexBoundary(index);

            #region Give up Memory for CPU
            // So that we do not need to enumerate the collection twice
            // Once to get the count/size of the incoming collection
            // Once to insert the incoming collection into the copied array
            // We can create another Array in memory to
            T[] newItemsAsArray = collection.ToArray();

            var collectionCount = newItemsAsArray.Length;
            #endregion

            #region Give up CPU for Memory
            // // This will cause an enumeration to occur on items passed in
            // var collectionCount = collection.Count();
            #endregion

            #region Avoid Shuffle
            // Here we don't use EnsureCapacity
            // Which will do a full Copy the original array into the new array if a new target size is required
            // Then after that full Copy we move items accordingly to their new positions
            // Instead we create an empty Array and do two separate partial Copies
            // This only results in minor performance gains
            var targetSize = Count + collectionCount;

            if (targetSize > Capacity)
            {
                T[] newUnderlyingArray = new T[targetSize];

                Array.Copy(
                    sourceArray: this.Items,
                    destinationArray: newUnderlyingArray,
                    length: index);

                Array.Copy(
                    sourceArray: this.Items,
                    sourceIndex: index,
                    destinationArray: newUnderlyingArray,
                    destinationIndex: index + collectionCount,
                    length: Count - index);

                this.Items = newUnderlyingArray;
            }
            // If the current size of the Items [] is sufficient
            else
            {
                Array.Copy(
                    sourceArray: this.Items,
                    sourceIndex: index,
                    destinationArray: this.Items,
                    destinationIndex: index + collectionCount,
                    length: Count - index);
            }
            #endregion

            #region Copy then Shuffle
            // EnsureCapacity(collectionCount);

            // Array.Copy(
            //     sourceArray: this.Items,
            //     sourceIndex: index,
            //     destinationArray: this.Items,
            //     destinationIndex: index + collectionCount,
            //     length: Count - index);
            #endregion

            #region Give up Memory for CPU
            // Copy items from incoming collection into underlying Items []
            // In this example we take the existing newItemsArray created earlier in memory
            // And copy all of the values over into the Items []
            Array.Copy(
                sourceArray: newItemsAsArray,
                sourceIndex: 0,
                destinationArray: this.Items,
                destinationIndex: index,
                length: collectionCount);
            #endregion

            #region Give up CPU for Memory
            // // Copy items from incoming collection into underlying Items []
            // // In this example we need to re-iterate through the Enumerable to copy each item in their specified index
            // foreach (T item in collection)
            // {
            //     this.Items[index++] = item;
            // }
            #endregion

            Count += collectionCount;
        }

        /// <summary>
        /// Returns a new instance of MyList<T> containing items from the underlying Items [] starting from the specified index for a given amount
        /// </summary>
        /// <param name="index">The index to retrieve the specified item</param>
        /// <param name="amount">The amount of items to retrieve from the specified index position</param>
        public MyList<T> GetRange(int index, int amount)
        {
            MyList<T> returnedList = new MyList<T>(amount);

            Array.Copy(
                sourceArray: this.Items,
                sourceIndex: index,
                destinationArray: returnedList.Items,
                destinationIndex: 0,
                length: amount);

            returnedList.Count = amount;

            return returnedList;
        }

        /// <summary>
        /// Searches for index of item in underlying Items [] based on the EqualityComparer for the given type
        /// Removes item at the given index if the item is found
        /// </summary>
        /// <param name="item">The item to be removed within the underlying Items []</param>
        /// <return>Returns a boolean value indicating whether the removal was successful</return>
        public bool Remove(T item)
        {
            int index = Array.IndexOf(this.Items, item);

            if (index >= 0)
            {
                RemoveAt(index);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove the item in the underlying Items [] at the specified index
        /// </summary>
        public void RemoveAll(Predicate<T> predicate)
        {
            for (int i = 0; i < Count; i++)
            {
                // Runs predicate and shuffles Array each time predicate returns true
                if (predicate(this.Items[i]))
                {
                    RemoveAt(i);

                    // if true,
                    // reset counter at same position as original i when body is executed
                    i--;
                }
            }
        }

        /// <summary>
        /// Remove the item in the underlying Items [] at the specified index
        /// </summary>
        /// <param name="index">The index to remove the item</param>
        public void RemoveAt(int index)
        {
            CheckIndexBoundary(index);

            // Move all elements one position after the specified index into the specified index
            // Length will be the current Count - (offset plus one position after)
            Array.Copy(
                sourceArray: this.Items,
                sourceIndex: index + 1,
                destinationArray: this.Items,
                destinationIndex: index,
                length: Count - (index + 1));

            Count--;
        }

        /// <summary>
        /// Removes collection from the underlying Items []
        /// <param name="index">The index to remove the specified item</param>
        /// <param name="amount">The amount of items to remove from the specified index position</param>
        public void RemoveRange(int index, int amount)
        {
            CheckIndexBoundary(index);

            // If amount desired to be removed exceeds the current Count, throw exception
            if (index + amount > Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            Array.Copy(
                sourceArray: this.Items,
                sourceIndex: index + amount,
                destinationArray: this.Items,
                destinationIndex: index,
                length: Count - (index + amount));

            Count -= amount;
        }

        /// <summary>
        /// Check if specified index exists in current Items []
        /// <param name="index">The index to check if exists</param>
        /// </summary>
        void CheckIndexBoundary(int index)
        {
            if (index > this.Count || index < 0)
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// The underlying Items [] should always be >= the number of items
        /// To achieve this, we will copy and resize the array into a new one and move the ref
        /// Check if the current number of items in the underlying Items [] is at max capacity
        /// If action calling this method requires more space, the underlying Items [] will resize
        /// <param name="numberOfNewItems">The number of new items being added</param>
        /// </summary>
        void EnsureCapacity(int numberOfNewItems = 1)
        {
            var requiredCapacity = Count + numberOfNewItems;

            var targetSize = Capacity * 2;

            if (requiredCapacity > Capacity)
            {
                if (targetSize < requiredCapacity)
                {
                    targetSize = requiredCapacity;
                }

                Array.Resize(ref Items, targetSize);
            }
        }

        /// <summary>
        /// Alternatively we could just do a for-loop with yield return.
        /// This would allow the compiler to generate an Enumerator for us.
        /// However, we will implement it ourselves in this demo.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnumerator(this);

            // Alternative:
            // for (int i = 0; i < Count; i++)
            // {
            //     yield return Items[i];
            // };
        }

        #region IEnumerator<T> State-Machine Implementation

        class MyListEnumerator : IEnumerator<T>
        {
            /// <summary>
            /// Provide the state of enumerator
            /// </summary>
            int State;

            /// <summary>
            /// Provide the current index of the underlying Items [] when enumerating
            /// </summary>
            int CurrentIndex;

            /// <summary>
            /// Instance of MyList being enumerated
            /// </summary>
            MyList<T> MyList;

            public MyListEnumerator(MyList<T> myList)
            {
                this.MyList = myList;
            }

            /// <summary>
            /// Provide calling code enumerating over MyList the value at the current index
            /// </summary>
            public T Current => this.MyList.Items[this.CurrentIndex];

            /// <summary>
            /// Provide calling code enumerating over MyList the value at the current index
            /// </summary>
            public bool MoveNext()
            {
                switch (this.State)
                {
                    // State 0: Equivalent to Initialization of for-loop
                    case 0:
                        this.CurrentIndex = 0;
                        goto case 1;
                    // State 1: Equivalent to Conditional check in for-loop if we should execute body
                    case 1:
                        this.State = 1;

                        // If CurrentIndex is at end of MyList, return false
                        // Greater than for safe-measure although should be unnecessary
                        if (this.CurrentIndex >= this.MyList.Count)
                        {
                            return false;
                        }

                        this.State = 2;

                        return true;
                    // State 2: Equivalent to Increment in for-loop
                    // On the next MoveNext(), increment index then check if we are still in bounds of MyList
                    // This allows us to peek into the next item spot prior to .Curent being called
                    case 2:
                        this.CurrentIndex++;
                        goto case 1;
                }

                return false;
            }

            #region Unused/Deprecated method/properties
            object IEnumerator.Current => this.Current;
            public void Dispose() { }
            public void Reset() { }
            #endregion
        }
        #endregion

        #region Unused/Deprecated method/properties
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
