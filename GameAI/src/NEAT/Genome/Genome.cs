using System;
using GameAI.NEAT.DataStructures;
using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class Genome
    {
        #region Fields

        private static Random _random;
        public RandomHashSet<ConnectionGene> Connections { get; }
        public RandomHashSet<NodeGene> Nodes { get; }
        public Neat Neat { get; }

        public Genome(Neat neat)
        {
            Connections = new RandomHashSet<ConnectionGene>();
            Nodes = new RandomHashSet<NodeGene>();
            Neat = neat;
            _random = new Random();
        }

        #endregion

        #region Methods

        public Genome CrossOver(Genome g2)
        {
            var g = Neat.empty_genome();

            var indexG1 = 0;
            var indexG2 = 0;

            while (indexG1 < Connections.Count && indexG2 < g2.Connections.Count)
            {
                var con1 = Connections[indexG1];
                var con2 = g2.Connections[indexG2];

                var id1 = con1.InnovationNumber;
                var id2 = con2.InnovationNumber;

                if (id1 < id2)
                {
                    g.Connections.Add(Neat.GetConnection(con1));
                    indexG1++;
                }
                else if (id1 > id2)
                {
                    g.Connections.Add(Neat.GetConnection(con2));
                    indexG2++;
                }
                else
                {
                    g.Connections.Add(Neat.GetConnection(_random.NextDouble() > 0.5 ? con1 : con2));
                    indexG1++;
                    indexG2++;
                }
            }

            while (indexG1 < Connections.Count)
            {
                g.Connections.Add(Connections[indexG1]);
                indexG1++;
            }

            foreach (var c in g.Connections.Data)
            {
                g.Nodes.Add(c.From);
                g.Nodes.Add(c.To);
            }

            return g;
        }

        public double Distance(Genome g2)
        {
            var g1 = this;

            var lastInnovationG1 = g1.Connections.Count == 0
                                       ? 0
                                       : g1.Connections[g1.Connections.Count - 1].InnovationNumber;

            var lastInnovationG2 = g2.Connections.Count == 0
                                       ? 0
                                       : g2.Connections[g2.Connections.Count - 1].InnovationNumber;

            if (lastInnovationG1 < lastInnovationG2)
            {
                var g = g1;
                g1 = g2;
                g2 = g;
            }

            var indexG1 = 0;
            var indexG2 = 0;

            int excess;
            var disjoint = 0;
            var weightDiff = .0;

            while (indexG1 < g1.Connections.Count && indexG2 < g2.Connections.Count)
            {
                var con1 = g1.Connections[indexG1];
                var con2 = g2.Connections[indexG2];

                var id1 = con1.InnovationNumber;
                var id2 = con2.InnovationNumber;

                if (id1 < id2)
                {
                    indexG1++;
                    disjoint++;
                }
                else if (id1 > id2)
                {
                    indexG2++;
                    disjoint++;
                }
                else
                {
                    weightDiff += Math.Abs(con1.Weight - con2.Weight);
                    indexG1++;
                    indexG2++;
                }
            }

            if (indexG1 == g1.Connections.Count) excess = g2.Connections.Count - indexG2;
            else excess = g1.Connections.Count - indexG1;

            double n = Math.Max(g1.Connections.Count, g2.Connections.Count);
            if (n < 20) n = 1;

            return Neat.C1 * excess / n + Neat.C2 * disjoint / n + Neat.C3 * weightDiff;
        }

        public void Mutate() { }

        #endregion
    }
}