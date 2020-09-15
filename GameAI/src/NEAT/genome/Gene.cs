namespace GameAI.NEAT.genome
{
    public class Gene
    {
        private int _innovationNumber;

        public Gene(int innovationNumber) { _innovationNumber = innovationNumber; }

        protected Gene() { }

        public int getInnovation_number() { return _innovationNumber; }

        public void setInnovation_number(int innovationNumber) { _innovationNumber = innovationNumber; }
    }
}