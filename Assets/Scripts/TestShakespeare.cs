using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using AI;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class TestShakespeare : MonoBehaviour
{
    #region Attributes

    #region UI

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

    #endregion

    private readonly List<Text> _textList = new List<Text>();
    private GeneticAlgorithm<char> _ga;
    private int _numCharsPerTextObj;
    private Random _random;

    #endregion

    #region Methods

    #region Unity

    private void Awake()
    {
        _numCharsPerTextObj = numCharsPerText / validCharacters.Length;
        if (_numCharsPerTextObj > populationSize) _numCharsPerTextObj = populationSize;
        var numTextObjects = Mathf.CeilToInt((float) populationSize / _numCharsPerTextObj);
        for (var i = 0; i < numTextObjects; i++) _textList.Add(Instantiate(textPrefab, populationTextParent));
    }

    private void Start()
    {
        targetText.text = targetString;

        if (string.IsNullOrEmpty(targetString))
        {
            Debug.LogError("Target string is null or empty");
            enabled = false;
        }

        _random = new Random();
        _ga = new GeneticAlgorithm<char>(populationSize, targetString.Length, _random, GetRandomCharacter,
                                         FitnessFunction, elitism, mutationRate);
    }

    private void Update()
    {
        _ga.NewGeneration();
        UpdateText(_ga.BestDNA, _ga.Generation, _ga.Population.Count, j => _ga.Population[j].Genes);
        if (_ga.BestDNA.Fitness >= 1) enabled = false;
    }

    #endregion

    private char GetRandomCharacter() { return validCharacters[_random.Next(validCharacters.Length)]; }

    private float FitnessFunction(int index)
    {
        var score = _ga.Population[index].Genes
                       .Where((c, i) => c == targetString[i])
                       .Aggregate<char, float>(0, (current, t) => current + 1);

        return Mathf.Pow(2, score / targetString.Length) - 1;
    }

    private void UpdateText(DNA<char> bestDna, int generation, int popSize, Func<int, char[]> getGenes)
    {
        bestText.text = new string(bestDna.Genes);
        bestFitnessText.text = bestDna.Fitness.ToString(CultureInfo.InvariantCulture);
        numGenerationsText.text = generation.ToString();

        for (var i = 0; i < _textList.Count; i++)
        {
            var sb = new StringBuilder();
            var endIndex = i == _textList.Count - 1 ? popSize : _textList.Count * _numCharsPerTextObj;
            for (var j = i * _numCharsPerTextObj; j < endIndex; j++)
            {
                foreach (var c in getGenes(j)) sb.Append(c);
                if (j < endIndex - 1) sb.AppendLine();
            }

            _textList[i].text = sb.ToString();
        }
    }

    #endregion
}