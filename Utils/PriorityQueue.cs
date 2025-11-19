using System;
using System.Collections.Generic;

namespace JYW.RandomSurvival.Utils
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> heap;

        public PriorityQueue(int capacity = 0)
        {
            heap = new List<T>(capacity);
        }

        public void Push(T data)
        {
            heap.Add(data);
            int now = heap.Count - 1;

            while (now > 0)
            {
                int parent = (now - 1) / 2;
                if (heap[now].CompareTo(heap[parent]) <= 0)
                    break;

                (heap[now], heap[parent]) = (heap[parent], heap[now]);
                now = parent;
            }
        }

        public T Pop()
        {
            T ret = heap[0];
            int lastIndex = heap.Count - 1;
            heap[0] = heap[lastIndex];
            heap.RemoveAt(lastIndex);

            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;
                int largest = now;

                if (left < heap.Count && heap[largest].CompareTo(heap[left]) < 0)
                    largest = left;
                if (right < heap.Count && heap[largest].CompareTo(heap[right]) < 0)
                    largest = right;

                if (largest == now) break;

                (heap[now], heap[largest]) = (heap[largest], heap[now]);
                now = largest;
            }

            return ret;
        }

        public int Count => heap.Count;
    }
}