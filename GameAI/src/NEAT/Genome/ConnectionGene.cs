using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class ConnectionGene : Gene
    {
        public ConnectionGene(NodeGene from, NodeGene to)
        {
            From = from;
            To = to;
        }

        public bool Enabled { get; set; } = true;
        public NodeGene From { get; set; }
        public NodeGene To { get; set; }
        public double Weight { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionGene)) return false;
            var c = (ConnectionGene) obj;
            return From.Equals(c.From) && To.Equals(c.To);
        }

        public override int GetHashCode() { return From.InnovationNumber * Neat.MaxNodes + To.InnovationNumber; }
    }
}