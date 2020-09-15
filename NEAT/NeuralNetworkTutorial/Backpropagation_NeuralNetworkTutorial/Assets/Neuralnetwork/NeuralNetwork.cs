using System;
using System.Collections.Generic;

namespace Neuralnetwork
{
    /// <summary>
    ///     Simple MLP Neural Network
    /// </summary>
    public class NeuralNetwork
    {
        private readonly int[] _layer; //layer information
        private readonly Layer[] _layers; //layers in the network

        /// <summary>
        ///     Constructor setting up layers
        /// </summary>
        /// <param name="layer">Layers of this network</param>
        public NeuralNetwork(IList<int> layer)
        {
            //deep copy layers
            _layer = new int[layer.Count];
            for (var i = 0; i < layer.Count; i++)
                _layer[i] = layer[i];

            //creates neural layers
            _layers = new Layer[layer.Count - 1];

            for (var i = 0; i < _layers.Length; i++) _layers[i] = new Layer(layer[i], layer[i + 1]);
        }

        /// <summary>
        ///     High level feedforward for this network
        /// </summary>
        /// <param name="inputs">Inputs to be feed forwared</param>
        /// <returns></returns>
        public float[] FeedForward(float[] inputs)
        {
            //feed forward
            _layers[0].FeedForward(inputs);
            for (var i = 1; i < _layers.Length; i++) _layers[i].FeedForward(_layers[i - 1].Outputs);

            return _layers[_layers.Length - 1].Outputs; //return output of last layer
        }

        /// <summary>
        ///     High level back porpagation
        ///     Note: It is expexted the one feed forward was done before this back prop.
        /// </summary>
        /// <param name="expected">The expected output form the last feedforward</param>
        public void BackProp(float[] expected)
        {
            // run over all layers backwards
            for (var i = _layers.Length - 1; i >= 0; i--)
                if (i == _layers.Length - 1) _layers[i].BackPropOutput(expected); //back prop output
                else _layers[i].BackPropHidden(_layers[i + 1].Gamma, _layers[i + 1].Weights); //back prop hidden


            //Update weights
            foreach (var t in _layers) t.UpdateWeights();
        }

        /// <summary>
        ///     Each individual layer in the ML{
        /// </summary>
        public class Layer
        {
            public static readonly Random Random = new Random(); //Static random class variable
            private readonly int _numberOfInputs; //# of neurons in the previous layer
            private readonly int _numberOfOuputs; //# of neurons in the current layer
            public readonly float[] Error; //error of the output layer
            public readonly float[] Gamma; //gamma of this layer

            public readonly float[] Outputs; //outputs of this layer
            public readonly float[,] Weights; //weights of this layer
            public readonly float[,] WeightsDelta; //deltas of this layer
            public float[] Inputs; //inputs in into this layer

            /// <summary>
            ///     Constructor initilizes vaiour data structures
            /// </summary>
            /// <param name="numberOfInputs">Number of neurons in the previous layer</param>
            /// <param name="numberOfOuputs">Number of neurons in the current layer</param>
            public Layer(int numberOfInputs, int numberOfOuputs)
            {
                _numberOfInputs = numberOfInputs;
                _numberOfOuputs = numberOfOuputs;

                //initilize datastructures
                Outputs = new float[numberOfOuputs];
                Inputs = new float[numberOfInputs];
                Weights = new float[numberOfOuputs, numberOfInputs];
                WeightsDelta = new float[numberOfOuputs, numberOfInputs];
                Gamma = new float[numberOfOuputs];
                Error = new float[numberOfOuputs];

                InitilizeWeights(); //initilize weights
            }

            /// <summary>
            ///     Initilize weights between -0.5 and 0.5
            /// </summary>
            public void InitilizeWeights()
            {
                for (var i = 0; i < _numberOfOuputs; i++)
                for (var j = 0; j < _numberOfInputs; j++)
                    Weights[i, j] = (float) Random.NextDouble() - 0.5f;
            }

            /// <summary>
            ///     Feedforward this layer with a given input
            /// </summary>
            /// <param name="inputs">The output values of the previous layer</param>
            /// <returns></returns>
            public float[] FeedForward(float[] inputs)
            {
                Inputs = inputs; // keep shallow copy which can be used for back propagation

                //feed forwards
                for (var i = 0; i < _numberOfOuputs; i++)
                {
                    Outputs[i] = 0;
                    for (var j = 0; j < _numberOfInputs; j++) Outputs[i] += inputs[j] * Weights[i, j];
                    Outputs[i] = (float) Math.Tanh(Outputs[i]);
                }

                return Outputs;
            }

            /// <summary>
            ///     TanH derivate
            /// </summary>
            /// <param name="value">An already computed TanH value</param>
            /// <returns></returns>
            public float TanHDer(float value) { return 1 - value * value; }

            /// <summary>
            ///     Back propagation for the output layer
            /// </summary>
            /// <param name="expected">The expected output</param>
            public void BackPropOutput(float[] expected)
            {
                //Error dervative of the cost function
                for (var i = 0; i < _numberOfOuputs; i++) Error[i] = Outputs[i] - expected[i];

                //Gamma calculation
                for (var i = 0; i < _numberOfOuputs; i++) Gamma[i] = Error[i] * TanHDer(Outputs[i]);

                //Caluclating detla weights
                for (var i = 0; i < _numberOfOuputs; i++)
                for (var j = 0; j < _numberOfInputs; j++)
                    WeightsDelta[i, j] = Gamma[i] * Inputs[j];
            }

            /// <summary>
            ///     Back propagation for the hidden layers
            /// </summary>
            /// <param name="gammaForward">the gamma value of the forward layer</param>
            /// <param name="weightsFoward">the weights of the forward layer</param>
            public void BackPropHidden(float[] gammaForward, float[,] weightsFoward)
            {
                //Caluclate new gamma using gamma sums of the forward layer
                for (var i = 0; i < _numberOfOuputs; i++)
                {
                    Gamma[i] = 0;
                    for (var j = 0; j < gammaForward.Length; j++) Gamma[i] += gammaForward[j] * weightsFoward[j, i];
                    Gamma[i] *= TanHDer(Outputs[i]);
                }

                //Caluclating detla weights
                for (var i = 0; i < _numberOfOuputs; i++)
                for (var j = 0; j < _numberOfInputs; j++)
                    WeightsDelta[i, j] = Gamma[i] * Inputs[j];
            }

            /// <summary>
            ///     Updating weights
            /// </summary>
            public void UpdateWeights()
            {
                for (var i = 0; i < _numberOfOuputs; i++)
                for (var j = 0; j < _numberOfInputs; j++)
                    Weights[i, j] -= WeightsDelta[i, j] * 0.033f;
            }
        }
    }
}