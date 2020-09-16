using System;
using System.Collections.Generic;

namespace GameAI.NEAT.DataStructures
{
    public class RandomHashSet<T>
    {
        #region Fields

        private readonly Random _random;
        private readonly HashSet<T> _set;
        public List<T> Data { get; }
        public int Size => Data.Count;

        public RandomHashSet()
        {
            _random = new Random();
            _set = new HashSet<T>();
            Data = new List<T>();
        }

        #endregion

        #region Methods

        private bool Contains(T value) { return _set.Contains(value); }

        public T random_element() { return _set.Count > 0 ? Data[_random.Next(0, Size)] : default; }

        public void Add(T value)
        {
            if (!_set.Add(value)) return;
            Data.Add(value);
        }

        public void Clear()
        {
            _set.Clear();
            Data.Clear();
        }

        public T Get(int index) { return index >= 0 && index < Size ? Data[index] : default; }

        public void Remove(int index)
        {
            _set.Remove(Data[index]);
            Data.RemoveAt(index);
        }

        public void Remove(T value)
        {
            _set.Remove(value);
            Data.Remove(value);
        }

        #endregion
    }
}