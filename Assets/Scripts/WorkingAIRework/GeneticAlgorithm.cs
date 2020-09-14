using System;
using System.Collections.Generic;

namespace WorkingAIRework
{
	public class GeneticAlgorithm<T>
	{
		private readonly Random _random;
		private readonly int _elitism;
		private readonly float _mutationRate;
		private float _fitnessSum;
		private List<DNA<T>> _newPopulation;
		public List<DNA<T>> Population { get; private set; }
		public int Generation { get; private set; }
		public float BestFitness { get; private set; }
		public T[] BestGenes { get; }

	
		private readonly int _dnaSize;
		private readonly Func<T> _getRandomGene;
		private readonly Func<int, float> _fitnessFunction;

		public GeneticAlgorithm(int populationSize, int dnaSize, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,
								int elitism, float mutationRate = 0.01f)
		{
			Generation = 1;
			_elitism = elitism;
			_mutationRate = mutationRate;
			Population = new List<DNA<T>>(populationSize);
			_newPopulation = new List<DNA<T>>(populationSize);
			this._random = random;
			this._dnaSize = dnaSize;
			this._getRandomGene = getRandomGene;
			this._fitnessFunction = fitnessFunction;

			BestGenes = new T[dnaSize];

			for (int i = 0; i < populationSize; i++)
			{
				Population.Add(new DNA<T>(dnaSize, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
		}

		public void NewGeneration(int numNewDna = 0, bool crossoverNewDna = false)
		{
			int finalCount = Population.Count + numNewDna;

			if (finalCount <= 0) {
				return;
			}

			if (Population.Count > 0) {
				CalculateFitness();
				Population.Sort(CompareDNA);
			}
			_newPopulation.Clear();

			for (int i = 0; i < Population.Count; i++)
			{
				if (i < _elitism && i < Population.Count)
				{
					_newPopulation.Add(Population[i]);
				}
				else if (i < Population.Count || crossoverNewDna)
				{
					var parent1 = ChooseParent();
					var parent2 = ChooseParent();

					var child = parent1.Crossover(parent2);

					child.Mutate(_mutationRate);

					_newPopulation.Add(child);
				}
				else
				{
					_newPopulation.Add(new DNA<T>(_dnaSize, _random, _getRandomGene, _fitnessFunction, shouldInitGenes: true));
				}
			}

			List<DNA<T>> tmpList = Population;
			Population = _newPopulation;
			_newPopulation = tmpList;

			Generation++;
		}
	
		private int CompareDNA(DNA<T> a, DNA<T> b)
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
			_fitnessSum = 0;
			var best = Population[0];

			for (int i = 0; i < Population.Count; i++)
			{
				_fitnessSum += Population[i].CalculateFitness(i);

				if (Population[i].Fitness > best.Fitness)
				{
					best = Population[i];
				}
			}

			BestFitness = best.Fitness;
			best.Genes.CopyTo(BestGenes, 0);
		}

		private DNA<T> ChooseParent()
		{
			double randomNumber = _random.NextDouble() * _fitnessSum;

			foreach (var t in Population)
			{
				if (randomNumber < t.Fitness)
				{
					return t;
				}

				randomNumber -= t.Fitness;
			}

			return null;
		}
	}
}
