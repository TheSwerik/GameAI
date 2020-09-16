namespace GameAI.NEAT.genome
{
    public class NodeGene : Gene
    {
        public double X;
        public double Y;
        public NodeGene(int innovationNumber) : base(innovationNumber) { }

        public override bool Equals(object obj)
        {
            return obj is NodeGene gene && getInnovation_number() == gene.getInnovation_number();
        }

        public override int GetHashCode() { return getInnovation_number(); }
    }
}