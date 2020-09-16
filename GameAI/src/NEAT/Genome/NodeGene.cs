namespace GameAI.NEAT.genome
{
    public class NodeGene : Gene
    {
        public double X;
        public double Y;

        public NodeGene(int innovationNumber) { InnovationNumber = innovationNumber; }

        public override bool Equals(object obj)
        {
            return obj is NodeGene gene && InnovationNumber == gene.InnovationNumber;
        }

        public override int GetHashCode() { return InnovationNumber; }
    }
}