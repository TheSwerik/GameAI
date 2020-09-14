using System;
using System.Collections.Generic;

namespace WorkingAIRework
{
	public class GeneticAlgorithm<T>
	{
		private Random random;
		public int Elitism;
		public float MutationRate;
		private float fitnessSum;
		private List<WorkingAIRework.DNA<T>> newPopulation;
		public List<WorkingAIRework.DNA<T>> Population { get; private set; }
		public int Generation { get; private set; }
		public float BestFitness { get; private set; }
		public T[] BestGenes { get; private set; }

	
		private int dnaSize;
		private Func<T> getRandomGene;
		private Func<int, float> fitnessFunction;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,
								int elitism, float mutationRate = 0.01f)
		{
			Generation = 1;
			Elitism = elitism;
			MutationRate = mutationRate;
			Population = new List<WorkingAIRework.DNA<T>>(populationSize);
			newPopulation = new List<WorkingAIRework.DNA<T>>(populationSize);
			this.random = random;
			this.dnaSize = dnaSize;
			this.getRandomGene = getRandomGene;
			this.fitnessFunction = fitnessFunction;

			BestGenes = new T[dnaSize];

			for (int i = 0; i < populationSize; i++)
			{
				Population.Add(new WorkingAIRework.DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
		}

		public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false)
		{
			int finalCount = Population.Count + numNewDNA;

			if (finalCount <= 0) {
				return;
			}

			if (Population.Count > 0) {
				CalculateFitness();
				Population.Sort(CompareDNA);
			}
			newPopulation.Clear();

			for (int i = 0; i < Population.Count; i++)
			{
				if (i < Elitism && i < Population.Count)
				{
					newPopulation.Add(Population[i]);
				}
				else if (i < Population.Count || crossoverNewDNA)
				{
					WorkingAIRework.DNA<T> parent1 = ChooseParent();
					WorkingAIRework.DNA<T> parent2 = ChooseParent();

					WorkingAIRework.DNA<T> child = parent1.Crossover(parent2);

					child.Mutate(MutationRate);

					newPopulation.Add(child);
				}
				else
				{
					newPopulation.Add(new WorkingAIRework.DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
				}
			}

			List<WorkingAIRework.DNA<T>> tmpList = Population;
			Population = newPopulation;
			newPopulation = tmpList;

			Generation++;
		}
	
		private int CompareDNA(WorkingAIRework.DNA<T> a, WorkingAIRework.DNA<T> b)
		{
			if (a.Fitness > b.Fitness) {
				return -1;
			} else if (a.Fitness < b.Fitness) {
				return 1;
			} else {
				return 0;
			}
		}

		private void CalculateFitness()
		{
			fitnessSum = 0;
			WorkingAIRework.DNA<T> best = Population[0];

			for (int i = 0; i < Population.Count; i++)
			{
				fitnessSum += Population[i].CalculateFitness(i);

				if (Population[i].Fitness > best.Fitness)
				{
					best = Population[i];
				}
			}

			BestFitness = best.Fitness;
			best.Genes.CopyTo(BestGenes, 0);
		}

		private WorkingAIRework.DNA<T> ChooseParent()
		{
			double randomNumber = random.NextDouble() * fitnessSum;

			for (int i = 0; i < Population.Count; i++)
			{
				if (randomNumber < Population[i].Fitness)
				{
					return Population[i];
				}

				randomNumber -= Population[i].Fitness;
			}

			return null;
		}
	}
}
