using System;
using GameAI.NEAT.DataStructures;
using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class Genome
    {
        private static Random _random;

        public Genome(Neat neat)
        {
            Connections = new RandomHashSet<ConnectionGene>();
            Nodes = new RandomHashSet<NodeGene>();
            Neat = neat;
            _random = new Random();
        }

        public RandomHashSet<ConnectionGene> Connections { get; }
        public Neat Neat { get; }
        public RandomHashSet<NodeGene> Nodes { get; }

        public static Genome CrossOver(Genome g1, Genome g2)
        {
            var g = g1.Neat.empty_genome();

            var indexG1 = 0;
            var indexG2 = 0;

            while (indexG1 < g1.Connections.Size() && indexG2 < g2.Connections.Size())
            {
                var con1 = g1.Connections.Get(indexG1);
                var con2 = g2.Connections.Get(indexG2);

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

            while (indexG1 < g1.Connections.Size())
            {
                g.Connections.Add(g1.Connections.Get(indexG1));
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

            var lastInnovationG1 = g1.Connections.Size() == 0
                                       ? 0
                                       : g1.Connections.Get(g1.Connections.Size() - 1).InnovationNumber;

            var lastInnovationG2 = g2.Connections.Size() == 0
                                       ? 0
                                       : g2.Connections.Get(g2.Connections.Size() - 1).InnovationNumber;

            if (lastInnovationG1 < lastInnovationG2)
            {
                var g = g1;
                g1 = g2;
                g2 = g;
            }

            var indexG1 = 0;
            var indexG2 = 0;

            var excess = 0;
            var disjoint = 0;
            var weightDiff = 0;

            while (indexG1 < g1.Connections.Size() && indexG2 < g2.Connections.Size())
            {
                var con1 = g1.Connections.Get(indexG1);
                var con2 = g2.Connections.Get(indexG2);

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
                    weightDiff += (int) Math.Abs(con1.Weight - con2.Weight);
                    indexG1++;
                    indexG2++;
                }
            }

            if (indexG1 == g1.Connections.Size()) excess = g2.Connections.Size() - indexG2;
            else excess = g1.Connections.Size() - indexG1;

            double n = Math.Max(g1.Connections.Size(), g2.Connections.Size());
            n = n < 20 ? 1 : n;

            return Neat.C1 * excess / n + Neat.C2 * disjoint / n + Neat.C3 * weightDiff;
        }

        public void Mutate() { }
    }
}