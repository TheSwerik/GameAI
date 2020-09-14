using System;

namespace WorkingAIRework
{
    // ReSharper disable once InconsistentNaming
    public class DNA<T>
    {
        private readonly Func<int, float> _fitnessFunction;
        private readonly Func<T> _getRandomGene;
        private readonly Random _random;

        public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,
                   bool shouldInitGenes = true)
        {
            Genes = new T[size];
            _random = random;
            _getRandomGene = getRandomGene;
            _fitnessFunction = fitnessFunction;

            if (!shouldInitGenes) return;
            for (var i = 0; i < Genes.Length; i++) Genes[i] = getRandomGene();
        }

        public T[] Genes { get; }
        public float Fitness { get; private set; }

        public float CalculateFitness(int index) { return Fitness = _fitnessFunction(index); }

        public DNA<T> Crossover(DNA<T> otherParent)
        {
            var child = new DNA<T>(Genes.Length, _random, _getRandomGene, _fitnessFunction, false);
            for (var i = 0; i < Genes.Length; i++)
                child.Genes[i] = _random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (var i = 0; i < Genes.Length; i++)
                if (_random.NextDouble() < mutationRate)
                    Genes[i] = _getRandomGene();
        }
    }
}