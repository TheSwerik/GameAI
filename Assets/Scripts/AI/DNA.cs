using System;

namespace AI
{
    public class DNA<T>
    {
        #region Attributes

        private readonly Func<T> _getRandomGene;
        private readonly Func<float, int> _calculateFitness;
        private Random _random;
        public T[] Genes { get; }
        public float Fitness { get; private set; }

        public DNA(int size, Random random, Func<T> getRandomGene, Func<float, int> calculateFitness,
                   bool shouldInitGenes = true)
        {
            _random = random;
            _getRandomGene = getRandomGene;
            _calculateFitness = calculateFitness;
            Genes = new T[size];

            if (!shouldInitGenes) return;
            for (var i = 0; i < Genes.Length; i++) Genes[i] = getRandomGene();
        }

        #endregion

        #region Methods

        public float CalculateFitness(int index) => _calculateFitness(index);

        public DNA<T> Crossover(DNA<T> other)
        {
            var child = new DNA<T>(Genes.Length, _random, _getRandomGene, _calculateFitness,false);
            _random = new Random();

            for (var i = 0; i < Genes.Length; i++)
                child.Genes[i] = _random.NextDouble() < .5 ? Genes[i] : other.Genes[i];

            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (var i = 0; i < Genes.Length; i++)
                if (_random.NextDouble() < .5)
                    Genes[i] = _getRandomGene();
        }

        #endregion
    }
}