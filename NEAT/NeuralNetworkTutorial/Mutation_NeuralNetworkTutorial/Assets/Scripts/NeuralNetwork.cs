using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private readonly int[] _layers;
    private float _fitness;
    private float[][] _neurons;
    private float[][][] _weights;

    public NeuralNetwork(IList<int> layers)
    {
        _layers = new int[layers.Count];
        for (var i = 0; i < layers.Count; i++) _layers[i] = layers[i];
        InitNeurons();
        InitWeights();
    }

    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        _layers = new int[copyNetwork._layers.Length];
        for (var i = 0; i < copyNetwork._layers.Length; i++) _layers[i] = copyNetwork._layers[i];
        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork._weights);
    }

    public int CompareTo(NeuralNetwork other)
    {
        if (other == null || _fitness > other._fitness) return 1;
        if (_fitness < other._fitness) return -1;
        return 0;
    }

    private void CopyWeights(IList<float[][]> copyWeights)
    {
        for (var i = 0; i < _weights.Length; i++)
        for (var j = 0; j < _weights[i].Length; j++)
        for (var k = 0; k < _weights[i][j].Length; k++)
            _weights[i][j][k] = copyWeights[i][j][k];
    }

    private void InitNeurons() { _neurons = _layers.Select(t => new float[t]).ToArray(); }

    private void InitWeights()
    {
        var weightsList = new List<float[][]>();

        for (var i = 1; i < _layers.Length; i++)
        {
            var layerWeightsList = new List<float[]>();
            var neuronsInPreviousLayer = _layers[i - 1];
            for (var j = 0; j < _neurons[i].Length; j++)
            {
                var neuronWeights = new float[neuronsInPreviousLayer];
                for (var k = 0; k < neuronsInPreviousLayer; k++) neuronWeights[k] = Random.Range(-0.5f, 0.5f);
                layerWeightsList.Add(neuronWeights);
            }

            weightsList.Add(layerWeightsList.ToArray());
        }

        _weights = weightsList.ToArray();
    }

    public float[] FeedForward(float[] inputs)
    {
        for (var i = 0; i < inputs.Length; i++) _neurons[0][i] = inputs[i];
        for (var i = 1; i < _layers.Length; i++)
        for (var j = 0; j < _neurons[i].Length; j++)
        {
            var value = 0f;
            for (var k = 0; k < _neurons[i - 1].Length; k++) value += _weights[i - 1][j][k] * _neurons[i - 1][k];
            _neurons[i][j] = (float) Math.Tanh(value);
        }

        return _neurons[_neurons.Length - 1];
    }

    public void Mutate()
    {
        foreach (var t in _weights)
        foreach (var t1 in t)
            for (var k = 0; k < t1.Length; k++)
            {
                var weight = t1[k];
                var randomNumber = Random.Range(0f, 100f);
                if (randomNumber <= 2f)
                {
                    weight *= -1f;
                }
                else if (randomNumber <= 4f)
                {
                    weight = Random.Range(-0.5f, 0.5f);
                }
                else if (randomNumber <= 6f)
                {
                    var factor = Random.Range(0f, 1f) + 1f;
                    weight *= factor;
                }
                else if (randomNumber <= 8f)
                {
                    var factor = Random.Range(0f, 1f);
                    weight *= factor;
                }

                t1[k] = weight;
            }
    }

    public void AddFitness(float fit) { _fitness += fit; }

    public void SetFitness(float fit) { _fitness = fit; }

    public float GetFitness() { return _fitness; }
}