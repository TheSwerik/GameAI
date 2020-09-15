using System;
using System.Collections.Generic;

namespace GameAI.NEAT.data_structures
{
    public class RandomSelector<T>
    {
        private readonly List<T> _objects = new List<T>();
        private readonly Random _random = new Random();
        private readonly List<double> _scores = new List<double>();
        private double _totalScore;

        public void Add(T element, double score)
        {
            _objects.Add(element);
            _scores.Add(score);
            _totalScore += score;
        }

        public T Random()
        {
            var v = _random.NextDouble() * _totalScore;

            var c = .0;
            for (var i = 0; i < _objects.Count; i++)
            {
                c += _scores[i];
                if (c > v) return _objects[i];
            }

            return default;
        }

        public void Reset()
        {
            _objects.Clear();
            _scores.Clear();
            _totalScore = 0;
        }
    }
}