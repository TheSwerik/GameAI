using System;
using System.Globalization;
using GameAI.NEAT.neat;

namespace TestProject
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var neat = new Neat(2, 1, 100);

            var in1 = neat.GetNode(1);
            var in2 = neat.GetNode(2);
            var out1 = neat.GetNode(3);

            var con11 = neat.getConnection(in1, out1);
            var con12 = neat.getConnection(in2, out1);

            Console.WriteLine(con11.InnovationNumber);
            Console.WriteLine(con12.InnovationNumber);


            var con112 = neat.getConnection(in1, out1);
            con112.Weight = 3;

            Console.WriteLine(con112.Weight);

            //Genome g = neat.empty_genome();
            //System.out.println(g.getNodes().size());
        }
    }
}