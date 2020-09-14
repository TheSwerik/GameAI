using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using WorkingAI;
using Random = System.Random;

public class TestShakespeare : MonoBehaviour
{
    [Header("Genetic Algorithm")] [SerializeField]
    private string targetString = "To be, or not to be, that is the question.";

    [SerializeField]
    private string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ,.|!#$%&/()=? ";

    [SerializeField] private int populationSize = 200;
    [SerializeField] private float mutationRate = 0.01f;
    [SerializeField] private int elitism = 5;

    [Header("Other")] [SerializeField] private int numCharsPerText = 15000;
    [SerializeField] private Text targetText;
    [SerializeField] private Text bestText;
    [SerializeField] private Text bestFitnessText;
    [SerializeField] private Text numGenerationsText;
    [SerializeField] private Transform populationTextParent;
    [SerializeField] private Text textPrefab;
    private readonly List<Text> textList = new List<Text>();

    private GeneticAlgorithm<char> ga;

    private int numCharsPerTextObj;
    private Random random;

    private void Awake()
    {
        numCharsPerTextObj = numCharsPerText / validCharacters.Length;
        if (numCharsPerTextObj > populationSize) numCharsPerTextObj = populationSize;

        var numTextObjects = Mathf.CeilToInt((float) populationSize / numCharsPerTextObj);

        for (var i = 0; i < numTextObjects; i++) textList.Add(Instantiate(textPrefab, populationTextParent));
    }

    private void Start()
    {
        targetText.text = targetString;

        if (string.IsNullOrEmpty(targetString))
        {
            Debug.LogError("Target string is null or empty");
            enabled = false;
        }

        random = new Random();
        ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, random, GetRandomCharacter,
                                        FitnessFunction, elitism, mutationRate);
    }

    private void Update()
    {
        ga.NewGeneration();

        UpdateText(ga.BestGenes, ga.BestFitness, ga.Generation, ga.Population.Count, j => ga.Population[j].Genes);

        if (ga.BestFitness == 1) enabled = false;
    }

    private char GetRandomCharacter()
    {
        var i = random.Next(validCharacters.Length);
        return validCharacters[i];
    }

    private float FitnessFunction(int index)
    {
        float score = 0;
        var dna = ga.Population[index];

        for (var i = 0; i < dna.Genes.Length; i++)
            if (dna.Genes[i] == targetString[i])
                score += 1;

        score /= targetString.Length;

        score = (Mathf.Pow(2, score) - 1) / (2 - 1);

        return score;
    }

    private void UpdateText(char[] bestGenes, float bestFitness, int generation, int populationSize,
                            Func<int, char[]> getGenes)
    {
        bestText.text = CharArrayToString(bestGenes);
        bestFitnessText.text = bestFitness.ToString();

        numGenerationsText.text = generation.ToString();

        for (var i = 0; i < textList.Count; i++)
        {
            var sb = new StringBuilder();
            var endIndex = i == textList.Count - 1 ? populationSize : (i + 1) * numCharsPerTextObj;
            for (var j = i * numCharsPerTextObj; j < endIndex; j++)
            {
                foreach (var c in getGenes(j)) sb.Append(c);

                if (j < endIndex - 1) sb.AppendLine();
            }

            textList[i].text = sb.ToString();
        }
    }

    private string CharArrayToString(char[] charArray)
    {
        var sb = new StringBuilder();
        foreach (var c in charArray) sb.Append(c);

        return sb.ToString();
    }
}