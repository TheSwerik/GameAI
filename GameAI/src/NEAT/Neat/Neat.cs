using System;
using System.Collections.Generic;
using GameAI.NEAT.DataStructures;
using GameAI.NEAT.genome;

namespace GameAI.NEAT.neat
{
    public class Neat
    {
        #region Fields

        public static readonly int MaxNodes = (int) Math.Pow(2, 20);
        private readonly Dictionary<ConnectionGene, ConnectionGene> _allConnections;
        private readonly RandomHashSet<NodeGene> _allNodes;
        private int _inputSize;
        private int _maxClients;
        private int _outputSize;

        public double C1 { get; } = 1;
        public double C2 { get; } = 1;
        public double C3 { get; } = 1;

        public Neat(int inputSize, int outputSize, int clients)
        {
            _allConnections = new Dictionary<ConnectionGene, ConnectionGene>();
            _allNodes = new RandomHashSet<NodeGene>();
            Reset(inputSize, outputSize, clients);
        }

        #endregion

        #region Methods

        public static ConnectionGene GetConnection(ConnectionGene con)
        {
            return new ConnectionGene(con.From, con.To) {Weight = con.Weight, Enabled = con.Enabled};
        }

        public Genome empty_genome()
        {
            var g = new Genome(this);
            for (var i = 0; i < _inputSize + _outputSize; i++) g.Nodes.Add(GetNode(i + 1));
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

        public ConnectionGene GetConnection(NodeGene node1, NodeGene node2)
        {
            var connectionGene = new ConnectionGene(node1, node2);

            if (_allConnections.ContainsKey(connectionGene))
            {
                connectionGene.InnovationNumber = _allConnections[connectionGene].InnovationNumber;
            }
            else
            {
                _allConnections.Add(connectionGene, connectionGene);
                connectionGene.InnovationNumber = _allConnections.Count;
            }

            return connectionGene;
        }

        public NodeGene GetNode()
        {
            var n = new NodeGene(_allNodes.Count + 1);
            _allNodes.Add(n);
            return n;
        }

        public NodeGene GetNode(int id) { return id <= _allNodes.Count ? _allNodes[id - 1] : GetNode(); }

        #endregion
    }
}