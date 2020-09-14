using System;

namespace WorkingAIRework
{
	// ReSharper disable once InconsistentNaming
	public class DNA<T>
	{
		public T[] Genes { get; private set; }
		public float Fitness { get; private set; }
		private readonly Random _random;
		private readonly Func<T> _getRandomGene;
		private readonly Func<int, float> _fitnessFunction;

		public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
		{
			Genes = new T[size];
			this._random = random;
			this._getRandomGene = getRandomGene;
			this._fitnessFunction = fitnessFunction;

			if (shouldInitGenes)
			{
				for (int i = 0; i < Genes.Length; i++)
				{
					Genes[i] = getRandomGene();
				}
			}
		}

		public float CalculateFitness(int index)
		{
			Fitness = _fitnessFunction(index);
			return Fitness;
		}

		public DNA<T> Crossover(DNA<T> otherParent)
		{
			DNA<T> child = new DNA<T>(Genes.Length, _random, _getRandomGene, _fitnessFunction, shouldInitGenes: false);

			for (int i = 0; i < Genes.Length; i++)
			{
				child.Genes[i] = _random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
			}

			return child;
		}

		public void Mutate(float mutationRate)
		{
			for (int i = 0; i < Genes.Length; i++)
			{
				if (_random.NextDouble() < mutationRate)
				{
					Genes[i] = _getRandomGene();
				}
			}
		}
	}
}