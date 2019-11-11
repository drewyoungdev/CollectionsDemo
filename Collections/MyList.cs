using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionsDemo.Collections
{
    public class MyList<T> : IEnumerable<T>
    {
        /// <summary>
        /// Allow MyList's items to be accessed through index
        /// If an index that exceeds the number of items within the underlying Items [], throw exception
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

        void CheckIndexBoundary(int index)
        {
            if (index >= this.ListSize || index < 0)
            {
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// The underlying Items []
        /// </summary>
        T[] Items;

        /// <summary>
        /// ListSize is the number of items in the underlying Items [].
        /// Items [] should always be >= the number of items
        /// </summary>
        int ListSize;

        /// <summary>
        /// List initializes memory allocated for 5 items
        /// </summary>
        public MyList()
        {
            Items = new T[5];
        }

        /// <summary>
        /// We must always ensure the underlying Items[] has space for the new item
        /// To achieve this, we will copy and resize the array into a new one and move the ref
        /// Each time an item is added, we increase the space "taken" up in the array with the item added
        /// </summary>
        public void Add(T item)
        {
            if (ListSize == Items.Length)
            {
                Array.Resize(ref Items, Items.Length * 2);
            }

            Items[ListSize++] = item;
        }

        /// <summary>
        /// Alternatively we could just do a for-loop with yield return.
        /// This would allow the compiler to generate an Enumerator for us.
        /// However, we will implement it ourselves in this demo.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new MyListEnumerator(this);

            // for (int i = 0; i < ListSize; i++)
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
                        if (this.CurrentIndex >= this.MyList.ListSize)
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
