using UnityEngine;

namespace Enhanced
{
    public class SmallList<T>
    {
        public T[] Data { get; set; }
        public int Count { get; set; }

        public T this[int i]
        {
            get
            {
                if (Data is null)
                {
                    return default(T);
                }
                else
                {
                    return Data[i];
                }
            }

            set
            {
                Data[i] = value;
            }
        }

        private void ResizeArray()
        {
            T[] newData;
            
            if (Data != null)
            {
                newData = new T[Mathf.Max(Data.Length << 1, 64)];
            }
            else
            {
                newData = new T[64];
            }

            if (Data != null && Count > 0)
            {
                Data.CopyTo(newData, 0);
            }

            Data = newData;
        }

        public void Clear()
        {
            Count = 0;
        }

        public T First()
        {
            if (Data == null || Count == 0)
            {
                return default(T);
            }

            return Data[0];
        }

        public T Last()
        {
            if (Data == null || Count == 0)
            {
                return default(T);
            }

            return Data[Count - 1];
        }

        public void Add(T item)
        {
            if (Data == null || Count == Data.Length)
            {
                ResizeArray();
            }

            Data[Count] = item;
            Count++;
        }

        public void AddStart(T item)
        {
            Insert(item, 0);
        }

        public void Insert(T item, int index)
        {
            if (Data == null || Count == Data.Length)
            {
                ResizeArray();
            }

            for (int i = Count; i > index; i--)
            {
                Data[i] = Data[i - 1];
            }

            Data[index] = item;
            Count++;
        }

        public T RemoveStart()
        {
            return RemoveAt(0);
        }

        public T RemoveAt(int index)
        {
            if (Data != null && Count != 0)
            {
                T val = Data[index];

                for (int i = index; i < Count - 1; i++)
                {
                    Data[i] = Data[i + 1];
                }

                Count--;
                Data[Count] = default(T);
                return val;
            }
            else
            {
                return default(T);
            }
        }

        public T Remove(T item)
        {
            if (Data != null && Count != 0)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (Data[i].Equals(item))
                    {
                        return RemoveAt(i);
                    }
                }
            }

            return default(T);
        }

        public T RemoveEnd()
        {
            if (Data != null && Count != 0)
            {
                Count--;
                T val = Data[Count];
                Data[Count] = default(T);

                return val;
            }
            else
            {
                return default(T);
            }
        }

        public bool Contains(T item)
        {
            if (Data == null)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (Data[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
