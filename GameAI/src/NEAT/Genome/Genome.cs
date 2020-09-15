using System;
using GameAI.NEAT.data_structures;
using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class Genome
    {
        private static Random _random;

        private readonly RandomHashSet<ConnectionGene> _connections;
        private readonly Neat _neat;
        private readonly RandomHashSet<NodeGene> _nodes;

        public Genome(Neat neat)
        {
            _connections = new RandomHashSet<ConnectionGene>();
            _nodes = new RandomHashSet<NodeGene>();
            _neat = neat;
            _random = new Random();
        }

        public static Genome CrossOver(Genome g1, Genome g2)
        {
            var g = g1.GetNeat().empty_genome();

            var indexG1 = 0;
            var indexG2 = 0;

            while (indexG1 < g1.GetConnections().Size() && indexG2 < g2.GetConnections().Size())
            {
                var con1 = g1.GetConnections().Get(indexG1);
                var con2 = g2.GetConnections().Get(indexG2);

                var id1 = con1.getInnovation_number();
                var id2 = con2.getInnovation_number();

                if (id1 < id2)
                {
                    g.GetConnections().Add(Neat.GetConnection(con1));
                    indexG1++;
                }
                else if (id1 > id2)
                {
                    g.GetConnections().Add(Neat.GetConnection(con2));
                    indexG2++;
                }
                else
                {
                    g.GetConnections().Add(Neat.GetConnection(_random.NextDouble() > 0.5 ? con1 : con2));
                    indexG1++;
                    indexG2++;
                }
            }

            while (indexG1 < g1.GetConnections().Size())
            {
                g.GetConnections().Add(g1.GetConnections().Get(indexG1));
                indexG1++;
            }

            foreach (var c in g.GetConnections().GetData())
            {
                g.GetNodes().Add(c.GetFrom());
                g.GetNodes().Add(c.GetTo());
            }

            return g;
        }

        public double Distance(Genome g2)
        {
            var g1 = this;

            var lastInnovationG1 = g1.GetConnections().Size() == 0
                                       ? 0
                                       : g1.GetConnections().Get(g1.GetConnections().Size() - 1)
                                           .getInnovation_number();

            var lastInnovationG2 = g2.GetConnections().Size() == 0
                                       ? 0
                                       : g2.GetConnections().Get(g2.GetConnections().Size() - 1)
                                           .getInnovation_number();

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

            while (indexG1 < g1.GetConnections().Size() && indexG2 < g2.GetConnections().Size())
            {
                var con1 = g1.GetConnections().Get(indexG1);
                var con2 = g2.GetConnections().Get(indexG2);

                var id1 = con1.getInnovation_number();
                var id2 = con2.getInnovation_number();

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
                    weightDiff += (int) Math.Abs(con1.GetWeight() - con2.GetWeight());
                    indexG1++;
                    indexG2++;
                }
            }

            if (indexG1 == g1.GetConnections().Size()) excess = g2.GetConnections().Size() - indexG2;
            else excess = g1.GetConnections().Size() - indexG1;

            double n = Math.Max(g1.GetConnections().Size(), g2.GetConnections().Size());
            n = n < 20 ? 1 : n;

            return Neat.GetC1() * excess / n + Neat.GetC2() * disjoint / n + Neat.GetC3() * weightDiff;
        }

        public void Mutate() { }
        public RandomHashSet<ConnectionGene> GetConnections() { return _connections; }
        public RandomHashSet<NodeGene> GetNodes() { return _nodes; }
        public Neat GetNeat() { return _neat; }
    }
}