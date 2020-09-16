using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class ConnectionGene : Gene
    {
        public bool Enabled = true;
        public NodeGene From;
        public NodeGene To;
        public double Weight;

        public ConnectionGene(NodeGene from, NodeGene to)
        {
            From = from;
            To = to;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionGene)) return false;
            var c = (ConnectionGene) obj;
            return From.Equals(c.From) && To.Equals(c.To);
        }

        public override int GetHashCode()
        {
            return From.getInnovation_number() * Neat.MaxNodes + To.getInnovation_number();
        }
    }
}