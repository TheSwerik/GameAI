using System;
using System.Collections.Generic;

namespace GameAI.NEAT.data_structures
{
    public class RandomHashSet<T>
    {
        private readonly List<T> _data;
        private readonly Random _random;
        private readonly HashSet<T> _set;

        public RandomHashSet()
        {
            _data = new List<T>();
            _set = new HashSet<T>();
            _random = new Random();
        }

        private bool Contains(T value) { return _set.Contains(value); }

        public T random_element() { return _set.Count > 0 ? _data[_random.Next(0, Size())] : default; }

        public int Size() { return _data.Count; }

        public void Add(T value)
        {
            if (_set.Contains(value)) return;
            _set.Add(value);
            _data.Add(value);
        }

        public void Clear()
        {
            _set.Clear();
            _data.Clear();
        }

        public T Get(int index) { return index >= 0 && index < Size() ? _data[index] : default; }

        public void Remove(int index)
        {
            if (index < 0 || index >= Size()) return;
            _set.Remove(_data[index]);
            _data.RemoveAt(index);
        }

        public void Remove(T value)
        {
            _set.Remove(value);
            _data.Remove(value);
        }

        public List<T> GetData() { return _data; }
    }
}