using System;
using System.Collections.Generic;

namespace AI
{
    public class GeneticAlgorithm<T>
    {
        public List<DNA<T>> Population { get; private set; }
        public int Generation { get; private set; }
        public float MutationRate;
        private Random _random;

        public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene,
                                Func<float, int> calculateFitness, float mutationRate = 0.01f)
        {
            Generation = 1;
            MutationRate = mutationRate;
            _random = random;
            Population = new List<DNA<T>>(populationSize);
            for (var i = 0; i < populationSize; i++)
                Population.Add(new DNA<T>(dnaSize, random, getRandomGene, calculateFitness));
        }

        public void NewGeneration()
        {
            CalculateFitness();

            var newPopulation = new List<DNA<T>>(Population.Count);

            for (var i = 0; i < Population.Count; i++)
            {
                var parent1 = Population[0];
                var parent2 = Population[0];

                var child = parent1.Crossover(parent2);
                child.Mutate(MutationRate);
                newPopulation.Add(child);
            }

            Population = newPopulation;
            Generation++;
        }

        private void CalculateFitness() { throw new NotImplementedException(); }
    }
}