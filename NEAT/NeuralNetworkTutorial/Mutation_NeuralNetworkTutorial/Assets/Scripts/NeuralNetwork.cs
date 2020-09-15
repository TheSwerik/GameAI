using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

/// <summary>
///     Neural Network C# (Unsupervised)
/// </summary>
public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private readonly int[] _layers; //layers
    private float _fitness; //fitness of the network
    private float[][] _neurons; //neuron matix
    private float[][][] _weights; //weight matrix

    /// <summary>
    ///     Initilizes and neural network with random weights
    /// </summary>
    /// <param name="layers">layers to the neural network</param>
    public NeuralNetwork(IList<int> layers)
    {
        //deep copy of layers of this network 
        _layers = new int[layers.Count];
        for (var i = 0; i < layers.Count; i++) _layers[i] = layers[i];


        //generate matrix
        InitNeurons();
        InitWeights();
    }

    /// <summary>
    ///     Deep copy constructor
    /// </summary>
    /// <param name="copyNetwork">Network to deep copy</param>
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        _layers = new int[copyNetwork._layers.Length];
        for (var i = 0; i < copyNetwork._layers.Length; i++) _layers[i] = copyNetwork._layers[i];

        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork._weights);
    }

    /// <summary>
    ///     Compare two neural networks and sort based on fitness
    /// </summary>
    /// <param name="other">Network to be compared to</param>
    /// <returns></returns>
    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;
        if (_fitness > other._fitness) return 1;
        if (_fitness < other._fitness) return -1;
        return 0;
    }

    private void CopyWeights(float[][][] copyWeights)
    {
        for (var i = 0; i < _weights.Length; i++)
        for (var j = 0; j < _weights[i].Length; j++)
        for (var k = 0; k < _weights[i][j].Length; k++)
            _weights[i][j][k] = copyWeights[i][j][k];
    }

    /// <summary>
    ///     Create neuron matrix
    /// </summary>
    private void InitNeurons()
    {
        //Neuron Initilization

        _neurons = _layers.Select(t => new float[t]).ToArray(); //convert list to array
    }

    /// <summary>
    ///     Create weights matrix.
    /// </summary>
    private void InitWeights()
    {
        var weightsList = new List<float[][]>(); //weights list which will later will converted into a weights 3D array

        //itterate over all neurons that have a weight connection
        for (var i = 1; i < _layers.Length; i++)
        {
            var layerWeightsList =
                new List<float[]>(); //layer weight list for this current layer (will be converted to 2D array)

            var neuronsInPreviousLayer = _layers[i - 1];

            //itterate over all neurons in this current layer
            for (var j = 0; j < _neurons[i].Length; j++)
            {
                var neuronWeights = new float[neuronsInPreviousLayer]; //neruons weights

                //itterate over all neurons in the previous layer and set the weights randomly between 0.5f and -0.5
                for (var k = 0; k < neuronsInPreviousLayer; k++)
                    //give random weights to neuron weights
                    neuronWeights[k] = Random.Range(-0.5f, 0.5f);

                layerWeightsList.Add(neuronWeights); //add neuron weights of this current layer to layer weights
            }

            weightsList.Add(layerWeightsList
                                .ToArray()); //add this layers weights converted into 2D array into weights list
        }

        _weights = weightsList.ToArray(); //convert to 3D array
    }

    /// <summary>
    ///     Feed forward this neural network with a given input array
    /// </summary>
    /// <param name="inputs">Inputs to network</param>
    /// <returns></returns>
    public float[] FeedForward(float[] inputs)
    {
        //Add inputs to the neuron matrix
        for (var i = 0; i < inputs.Length; i++) _neurons[0][i] = inputs[i];

        //itterate over all neurons and compute feedforward values 
        for (var i = 1; i < _layers.Length; i++)
        for (var j = 0; j < _neurons[i].Length; j++)
        {
            var value = 0f;

            for (var k = 0; k < _neurons[i - 1].Length; k++)
                value += _weights[i - 1][j][k] *
                         _neurons[i - 1]
                             [k]; //sum off all weights connections of this neuron weight their values in previous layer

            _neurons[i][j] = (float) Math.Tanh(value); //Hyperbolic tangent activation
        }

        return _neurons[_neurons.Length - 1]; //return output layer
    }

    /// <summary>
    ///     Mutate neural network weights
    /// </summary>
    public void Mutate()
    {
        for (var i = 0; i < _weights.Length; i++)
        for (var j = 0; j < _weights[i].Length; j++)
        for (var k = 0; k < _weights[i][j].Length; k++)
        {
            var weight = _weights[i][j][k];

            //mutate weight value 
            var randomNumber = Random.Range(0f, 100f);

            if (randomNumber <= 2f)
            { //if 1
                //flip sign of weight
                weight *= -1f;
            }
            else if (randomNumber <= 4f)
            { //if 2
                //pick random weight between -1 and 1
                weight = Random.Range(-0.5f, 0.5f);
            }
            else if (randomNumber <= 6f)
            { //if 3
                //randomly increase by 0% to 100%
                var factor = Random.Range(0f, 1f) + 1f;
                weight *= factor;
            }
            else if (randomNumber <= 8f)
            { //if 4
                //randomly decrease by 0% to 100%
                var factor = Random.Range(0f, 1f);
                weight *= factor;
            }

            _weights[i][j][k] = weight;
        }
    }

    public void AddFitness(float fit) { _fitness += fit; }

    public void SetFitness(float fit) { _fitness = fit; }

    public float GetFitness() { return _fitness; }
}