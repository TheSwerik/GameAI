using System;
using System.Collections.Generic;
using GameAI.NEAT.genome;

namespace GameAI.NEAT.DataStructures
{
    public class RandomHashSet<T>
    {
        #region Fields

        private readonly Random _random;
        private readonly HashSet<T> _set;
        public List<T> Data { get; }
        public int Count => Data.Count;

        public RandomHashSet()
        {
            _random = new Random();
            _set = new HashSet<T>();
            Data = new List<T>();
        }

        #endregion

        #region Methods

        private bool Contains(T value) { return _set.Contains(value); }

        public T random_element() { return Data[_random.Next(0, Count)]; }

        public void Add(T value)
        {
            if (!_set.Add(value)) return;
            Data.Add(value);
        }

        public void AddSorted(T value)
        {
            var valueAsGene = value as Gene;
            for (var i = 0; i < Count; i++)
            {
                if (valueAsGene!.InnovationNumber >= (Data[i] as Gene)!.InnovationNumber) continue;
                Data[i] = value;
                _set.Add(value);
                return;
            }

            if (!_set.Add(value)) return;
            Data.Add(value);
        }

        public void Clear()
        {
            _set.Clear();
            Data.Clear();
        }

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

        public T this[int i] => Data[i];

        #endregion
    }
}