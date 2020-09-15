using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GameAI.Genetics
{
    public static class Serializer<T>
    {
        private const string SplitLine = "\n_________________________\n";

        private static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public static void Save(GeneticAlgorithm<T> algorithm)
        {
            var x = JsonConvert.SerializeObject(algorithm.Population, Formatting.Indented);
            x += SplitLine + JsonConvert.SerializeObject(algorithm.Generation, Formatting.Indented);
            using var file = File.CreateText($@"{Path}\test.gai");
            file.Write(x);
        }

        public static void Load(ref GeneticAlgorithm<T> algorithm)
        {
            var file = File.ReadAllText($@"{Path}\test.gai").Split(SplitLine.ToCharArray());
            algorithm.Load(JsonConvert.DeserializeObject<List<DNA<T>>>(file[0]),
                           JsonConvert.DeserializeObject<int>(file[1]));
        }
    }
}