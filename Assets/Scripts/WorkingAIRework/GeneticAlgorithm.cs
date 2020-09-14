using System;
using System.Collections.Generic;

namespace WorkingAIRework
{
    public class GeneticAlgorithm<T>
    {
        private readonly int _dnaSize;
        private readonly int _elitism;
        private readonly Func<int, float> _fitnessFunction;
        private readonly Func<T> _getRandomGene;
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
            _dnaSize = dnaSize;
            _getRandomGene = getRandomGene;
            _fitnessFunction = fitnessFunction;

            BestGenes = new T[dnaSize];

            for (var i = 0; i < populationSize; i++)
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction));
        }

        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public float BestFitness { get; private set; }
        public T[] BestGenes { get; }

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


            var tmpList = Population;
            Population = _newPopulation;
            _newPopulation = tmpList;

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
            var best = Population[0];

            for (var i = 0; i < Population.Count; i++)
            {
                _fitnessSum += Population[i].CalculateFitness(i);
                if (Population[i].Fitness > best.Fitness) best = Population[i];
            }

            BestFitness = best.Fitness;
            best.Genes.CopyTo(BestGenes, 0);
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