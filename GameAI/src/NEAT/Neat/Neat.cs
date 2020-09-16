using System;
using System.Collections.Generic;
using GameAI.NEAT.DataStructures;
using GameAI.NEAT.genome;

namespace GameAI.NEAT.neat
{
    public class Neat
    {
        private const double C1 = 1;
        private const double C2 = 1;
        private const double C3 = 1;
        public static readonly int MaxNodes = (int) Math.Pow(2, 20);
        private readonly Dictionary<ConnectionGene, ConnectionGene> _allConnections;
        private readonly RandomHashSet<NodeGene> _allNodes;
        private int _inputSize;
        private int _maxClients;
        private int _outputSize;

        public Neat(int inputSize, int outputSize, int clients)
        {
            _allConnections = new Dictionary<ConnectionGene, ConnectionGene>();
            _allNodes = new RandomHashSet<NodeGene>();
            Reset(inputSize, outputSize, clients);
        }

        public static ConnectionGene GetConnection(ConnectionGene con)
        {
            var c = new ConnectionGene(con.GetFrom(), con.GetTo());
            c.SetWeight(con.GetWeight());
            c.SetEnabled(con.IsEnabled());
            return c;
        }

        public Genome empty_genome()
        {
            var g = new Genome(this);
            for (var i = 0; i < _inputSize + _outputSize; i++) g.GetNodes().Add(GetNode(i + 1));
            return g;
        }

        public void Reset(int inputSize, int outputSize, int clients)
        {
            _inputSize = inputSize;
            _outputSize = outputSize;
            _maxClients = clients;

            _allConnections.Clear();
            _allNodes.Clear();

            for (var i = 0; i < inputSize; i++)
            {
                var n = GetNode();
                n.X = 0.1;
                n.Y = (i + 1) / (double) (inputSize + 1);
            }

            for (var i = 0; i < outputSize; i++)
            {
                var n = GetNode();
                n.X = 0.9;
                n.Y = (i + 1) / (double) (outputSize + 1);
            }
        }

        public ConnectionGene getConnection(NodeGene node1, NodeGene node2)
        {
            var connectionGene = new ConnectionGene(node1, node2);

            if (_allConnections.ContainsKey(connectionGene))
            {
                connectionGene.setInnovation_number(_allConnections[connectionGene].getInnovation_number());
            }
            else
            {
                connectionGene.setInnovation_number(_allConnections.Count + 1);
                _allConnections.Add(connectionGene, connectionGene);
            }

            return connectionGene;
        }

        public NodeGene GetNode()
        {
            var n = new NodeGene(_allNodes.Size() + 1);
            _allNodes.Add(n);
            return n;
        }

        public NodeGene GetNode(int id) { return id <= _allNodes.Size() ? _allNodes.Get(id - 1) : GetNode(); }

        public static double GetC1() { return C1; }

        public static double GetC2() { return C2; }

        public static double GetC3() { return C3; }
    }
}