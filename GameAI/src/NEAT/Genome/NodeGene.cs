namespace GameAI.NEAT.genome
{
    public class NodeGene : Gene
    {
        #region Fields

        public double X { get; set; }
        public double Y { get; set; }
        public NodeGene(int innovationNumber) { InnovationNumber = innovationNumber; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            return obj is NodeGene gene && InnovationNumber == gene.InnovationNumber;
        }

        public override int GetHashCode() { return InnovationNumber; }

        #endregion
    }
}