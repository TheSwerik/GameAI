using System;

namespace AI
{
    // ReSharper disable once InconsistentNaming
    public class DNA<T> : IComparable
    {
        #region Attributes

        public float Fitness { get; private set; }

        public readonly T[] Genes;
        private readonly Random _random;
        private readonly Func<T> _getRandomGene;
        private readonly Func<int, float> _calculateFitness;

        public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> calculateFitness,
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

        public float CalculateFitness(int index) { return Fitness = _calculateFitness(index); }

        public DNA<T> Crossover(DNA<T> other)
        {
            var child = new DNA<T>(Genes.Length, _random, _getRandomGene, CalculateFitness, false);

            for (var i = 0; i < Genes.Length; i++)
                child.Genes[i] = _random.NextDouble() < .5 ? Genes[i] : other.Genes[i];

            return child;
        }

        public void Mutate(float mutationRate)
        {
            for (var i = 0; i < Genes.Length; i++)
                if (_random.NextDouble() < mutationRate)
                    Genes[i] = _getRandomGene();
        }

        #region Overrides

        public int CompareTo(object obj)
        {
            if (obj is DNA<T> other) return Fitness.CompareTo(other.Fitness);
            return 0;
        }

        public override string ToString() { return $"{string.Join(", ", Genes)}\t|\tFitness: {Fitness}"; }

        #endregion

        #endregion
    }
}