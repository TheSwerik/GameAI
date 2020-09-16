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
            var scoreThreshold = _random.NextDouble() * _totalScore;
            for (var i = 0; i < _objects.Count; i++)
                if (_scores[i] > scoreThreshold) return _objects[i];
                else scoreThreshold -= _scores[i];
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