using System;
using System.Collections.Generic;

namespace WorkingAIRework
{
    public class GeneticAlgorithm<T>
    {
        private readonly int _elitism;
        private readonly float _mutationRate;
        private readonly Random _random;
        private float _fitnessSum;
        private List<DNA<T>> _newPopulation;

        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene,
                                Func<int, float> fitnessFunction,
                                int elitism, float mutationRate = 0.01f)
        {
            Generation = 1;
            _elitism = elitism;
            _mutationRate = mutationRate;
            Population = new List<DNA<T>>(populationSize);
            _newPopulation = new List<DNA<T>>(populationSize);
            _random = random;

            for (var i = 0; i < populationSize; i++)
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction));
        }

        // ReSharper disable once InconsistentNaming
        public DNA<T> BestDNA { get; private set; }

        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }

        public void NewGeneration()
        {
            CalculateFitness();
            Population.Sort(CompareDNA);
            _newPopulation.Clear();

            for (var i = 0; i < Population.Count; i++)
            {
                if (i < _elitism)
                {
                    _newPopulation.Add(Population[i]);
                    continue;
                }

                var parent1 = ChooseParent();
                var parent2 = ChooseParent();

                var child = parent1.Crossover(parent2);
                child.Mutate(_mutationRate);
                _newPopulation.Add(child);
            }


            var tmp = Population;
            Population = _newPopulation;
            _newPopulation = tmp;

            Generation++;
        }

        private static int CompareDNA(DNA<T> a, DNA<T> b)
        {
            if (a.Fitness > b.Fitness) return -1;
            return a.Fitness < b.Fitness ? 1 : 0;
        }

        private void CalculateFitness()
        {
            _fitnessSum = 0;

            for (var i = 0; i < Population.Count; i++)
            {
                _fitnessSum += Population[i].CalculateFitness(i);
                if (BestDNA == null || Population[i].Fitness > BestDNA.Fitness) BestDNA = Population[i];
            }
        }

        private DNA<T> ChooseParent()
        {
            var randomNumber = _random.NextDouble() * _fitnessSum;
            foreach (var t in Population)
            {
                if (randomNumber < t.Fitness) return t;
                randomNumber -= t.Fitness;
            }

            return null;
        }
    }
}