using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class ConnectionGene : Gene
    {
        #region Fields

        public bool Enabled { get; set; } = true;
        public NodeGene From { get; set; }
        public NodeGene To { get; set; }
        public double Weight { get; set; }

        public ConnectionGene(NodeGene from, NodeGene to)
        {
            From = from;
            To = to;
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return obj is ConnectionGene cg && From.Equals(cg.From) && To.Equals(cg.To);
        }

        public override int GetHashCode() { return From.InnovationNumber * Neat.MaxNodes + To.InnovationNumber; }

        #endregion
    }
}