using System;
using System.Collections;
using System.Collections.Generic;




namespace Genetic_Algorithm_1
{

    public class FixedQueue<T> : IList<T>
    {

        public FixedQueue(int MaxSize) {
            this.MaxSize = MaxSize;
        }
        public int MaxSize { get; set; }
        private Queue<T> Items = new Queue<T>();
        private bool ReadOnly { get; set; }

        public void Add(T item)
        {
            Items.Enqueue(item);
            if (Items.Count == MaxSize)
            {
                Items.Dequeue();
            }

        }

        public int Count => Items.Count;

        public bool IsReadOnly => ReadOnly;

        public T this[int index]
        {
            get => Items.ToArray()[index];
            set
            {
                T[] array = Items.ToArray();
                Items.Clear();
                array.SetValue(value, index);
                Array.ForEach(array, elem => Items.Enqueue(elem));
            }
        }


        public int IndexOf(T item)
        {
            return Array.IndexOf(Items.ToArray(), item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();

        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(T item)
        {
            return Items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
        }
    }

    
}


