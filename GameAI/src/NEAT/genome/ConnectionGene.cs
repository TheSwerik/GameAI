using GameAI.NEAT.neat;

namespace GameAI.NEAT.genome
{
    public class ConnectionGene : Gene
    {
        private bool _enabled = true;
        private NodeGene _from;
        private NodeGene _to;
        private double _weight;

        public ConnectionGene(NodeGene from, NodeGene to)
        {
            _from = from;
            _to = to;
        }

        public NodeGene GetFrom() { return _from; }
        public void SetFrom(NodeGene from) { _from = from; }
        public NodeGene GetTo() { return _to; }
        public void SetTo(NodeGene to) { _to = to; }
        public double GetWeight() { return _weight; }
        public void SetWeight(double weight) { _weight = weight; }
        public bool IsEnabled() { return _enabled; }
        public void SetEnabled(bool enabled) { _enabled = enabled; }

        public override bool Equals(object obj)
        {
            if (!(obj is ConnectionGene)) return false;
            var c = (ConnectionGene) obj;
            return _from.Equals(c._from) && _to.Equals(c._to);
        }

        public override int GetHashCode()
        {
            return _from.getInnovation_number() * Neat.MaxNodes + _to.getInnovation_number();
        }
    }
}