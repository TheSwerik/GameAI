using System;
using System.Collections.Generic;

namespace GameAI.NEAT.DataStructures
{
    public class RandomHashSet<T>
    {
        private readonly Random _random;
        private readonly HashSet<T> _set;

        public RandomHashSet()
        {
            Data = new List<T>();
            _set = new HashSet<T>();
            _random = new Random();
        }

        public List<T> Data { get; }

        private bool Contains(T value) { return _set.Contains(value); }

        public T random_element() { return _set.Count > 0 ? Data[_random.Next(0, Size())] : default; }

        public int Size() { return Data.Count; }

        public void Add(T value)
        {
            if (_set.Contains(value)) return;
            _set.Add(value);
            Data.Add(value);
        }

        public void Clear()
        {
            _set.Clear();
            Data.Clear();
        }

        public T Get(int index) { return index >= 0 && index < Size() ? Data[index] : default; }

        public void Remove(int index)
        {
            if (index < 0 || index >= Size()) return;
            _set.Remove(Data[index]);
            Data.RemoveAt(index);
        }

        public void Remove(T value)
        {
            _set.Remove(value);
            Data.Remove(value);
        }
    }
}