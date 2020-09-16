using System;
using System.Collections.Generic;

namespace GameAI.NEAT.DataStructures
{
    public class RandomSelector<T>
    {
        private readonly List<T> _objects;
        private readonly Random _random;
        private readonly List<double> _scores;
        private double _totalScore;

        public RandomSelector()
        {
            _objects = new List<T>();
            _random = new Random();
            _scores = new List<double>();
        }

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
                if ((c += _scores[i]) > v)
                    return _objects[i];
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