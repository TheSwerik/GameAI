using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GameAI.Genetics
{
    public static class Serializer<T>
    {
        private static readonly string splitLine = "\n_________________________\n";

        public static void Save(GeneticAlgorithm<T> algorithm)
        {
            var x = JsonConvert.SerializeObject(algorithm.Population);
            x += splitLine + JsonConvert.SerializeObject(algorithm.Generation);
            using var file = File.CreateText(@"D:\Dokumente");
            file.Write(x);
        }

        public static void Load(ref GeneticAlgorithm<T> algorithm)
        {
            var file = File.ReadAllText(@"D:\Dokumente").Split(splitLine.ToCharArray());
            algorithm.Load(JsonConvert.DeserializeObject<List<DNA<T>>>(file[0]),
                           JsonConvert.DeserializeObject<int>(file[1]));
        }
    }
}