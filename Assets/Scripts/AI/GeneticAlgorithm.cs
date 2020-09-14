using System;
using System.Collections.Generic;

namespace AI
{
    public class GeneticAlgorithm<T>
    {
        private readonly Random _random;
        private List<DNA<T>> _newPopulation;
        public float FitnessSum;
        public float MutationRate;

        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene,
                                Func<int, float> calculateFitness, float mutationRate = 0.01f)
        {
            Generation = 1;
            MutationRate = mutationRate;
            _random = random;
            _newPopulation = new List<DNA<T>>(populationSize);
            Population = new List<DNA<T>>(populationSize);
            for (var i = 0; i < populationSize; i++)
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, calculateFitness));
        }

        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public DNA<T> BestDNA { get; private set; }

        public void NewGeneration()
        {
            CalculateFitness();

            _newPopulation.Clear();
            for (var i = 0; i < Population.Count; i++)
            {
                var parent1 = ChooseParent();
                var parent2 = ChooseParent();

                var child = parent1.Crossover(parent2);
                child.Mutate(MutationRate);
                _newPopulation.Add(child);
            }

            var tmp = Population;
            Population = _newPopulation;
            _newPopulation = tmp;

            Generation++;
        }

        private void CalculateFitness()
        {
            FitnessSum = 0;
            for (var i = 0; i < Population.Count; i++)
            {
                FitnessSum += Population[i].CalculateFitness(i);
                if (Population[i] > BestDNA) BestDNA = Population[i];
            }
        }

        private DNA<T> ChooseParent()
        {
            var fitnessThreshold = _random.NextDouble() * FitnessSum;
            foreach (var t in Population)
                if (t.Fitness > fitnessThreshold) return t;
                else fitnessThreshold -= t.Fitness;

            return null;
        }
    }
}