namespace GameAI.NEAT.genome
{
    public class NodeGene : Gene
    {
        private double _x, _y;
        public NodeGene(int innovationNumber) : base(innovationNumber) { }
        public double GetX() { return _x; }
        public void SetX(double x) { _x = x; }
        public double GetY() { return _y; }
        public void SetY(double y) { _y = y; }

        public override bool Equals(object obj)
        {
            return obj is NodeGene gene && getInnovation_number() == gene.getInnovation_number();
        }

        public override int GetHashCode() { return getInnovation_number(); }
    }
}