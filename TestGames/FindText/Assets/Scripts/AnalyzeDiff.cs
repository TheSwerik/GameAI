using System.IO;
using UnityEngine;

public class AnalyzeDiff : MonoBehaviour
{
    private void Awake()
    {
        // DNA();
        // GA();
        TS();
    }

    private void DNA()
    {
        var dna1 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\AI\DNA.cs").ReadToEnd().Split('\n');
        var dna2 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\WorkingAIRework\DNA.cs").ReadToEnd().Split('\n');

        var isDna1Longer = dna1.Length.CompareTo(dna2.Length);
        if (isDna1Longer != 0)
            Debug.Log($"AI.DNA is {(isDna1Longer == 1 ? "longer" : "shorter")} than WorkingAIRework.DNA");

        for (var i = 0; i < (isDna1Longer <= 0 ? dna1.Length : dna2.Length); i++)
        {
            var isLine1Longer = dna1[i].Length.CompareTo(dna2[i].Length);
            if (isLine1Longer == 0) continue;
            Debug.Log($"Difference in line {i}:");
            for (var j = 0; j < (isLine1Longer < 0 ? dna1[i].Length : dna2[i].Length); j++)
                if (dna1[i][j] != dna2[i][j])
                    Debug.Log($"Difference in char {j}: DNA1: {dna1[i][j]} DNA2: {dna2[i][j]}");
        }
    }

    private void GA()
    {
        var ga1 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\AI\GeneticAlgorithm.cs").ReadToEnd().Split('\n');
        var ga2 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\WorkingAIRework\GeneticAlgorithm.cs").ReadToEnd()
                      .Split('\n');

        var isDna1Longer = ga1.Length.CompareTo(ga2.Length);
        if (isDna1Longer != 0)
            Debug.Log(
                $"AI.GeneticAlgorithm is {(isDna1Longer == 1 ? "longer" : "shorter")} than WorkingAIRework.GeneticAlgorithm");

        for (var i = 0; i < (isDna1Longer <= 0 ? ga1.Length : ga2.Length); i++)
        {
            var isLine1Longer = ga1[i].Length.CompareTo(ga2[i].Length);
            if (isLine1Longer == 0) continue;
            Debug.Log($"Difference in line {i}:");
            for (var j = 0; j < (isLine1Longer <= 0 ? ga1[i].Length : ga2[i].Length); j++)
                if (ga1[i][j] != ga2[i][j])
                    Debug.Log($"Difference in char {j}: DNA1: {ga1[i][j]} DNA2: {ga2[i][j]}");
        }
    }

    private void TS()
    {
        var ts1 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\TestShakespeare2.cs").ReadToEnd().Split('\n');
        var ts2 = File.OpenText(@"D:\Projects\AITests\Assets\Scripts\TestShakespeareRework.cs").ReadToEnd().Split('\n');

        var isDna1Longer = ts1.Length.CompareTo(ts2.Length);
        if (isDna1Longer != 0)
            Debug.Log($"TestShakespeare2 is {(isDna1Longer == 1 ? "longer" : "shorter")} than TestShakespeareRework");

        for (var i = 0; i < (isDna1Longer <= 0 ? ts1.Length : ts2.Length); i++)
        {
            var isLine1Longer = ts1[i].Length.CompareTo(ts2[i].Length);
            if (isLine1Longer == 0) continue;
            Debug.Log($"Difference in line {i}:");
            for (var j = 0; j < (isLine1Longer <= 0 ? ts1[i].Length : ts2[i].Length); j++)
                if (ts1[i][j] != ts2[i][j])
                    Debug.Log($"Difference in char {j}: DNA1: {ts1[i][j]} DNA2: {ts2[i][j]}");
        }
    }
}