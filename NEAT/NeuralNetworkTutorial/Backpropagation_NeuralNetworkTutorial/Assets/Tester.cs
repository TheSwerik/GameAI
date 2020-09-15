using Neuralnetwork;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        // 0 0 0    => 0
        // 0 0 1    => 1
        // 0 1 0    => 1
        // 0 1 1    => 0
        // 1 0 0    => 1
        // 1 0 1    => 0
        // 1 1 0    => 0
        // 1 1 1    => 1

        var net = new NeuralNetwork(new[] {3, 25, 25, 1}); //intiilize network

        //Itterate 5000 times and train each possible output
        //5000*8 = 40000 traning operations
        for (var i = 0; i < 5000; i++)
        {
            net.FeedForward(new float[] {0, 0, 0});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[] {0, 0, 1});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[] {0, 1, 0});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[] {0, 1, 1});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[] {1, 0, 0});
            net.BackProp(new float[] {1});

            net.FeedForward(new float[] {1, 0, 1});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[] {1, 1, 0});
            net.BackProp(new float[] {0});

            net.FeedForward(new float[] {1, 1, 1});
            net.BackProp(new float[] {1});
        }


        //output to see if the network has learnt
        //WHICH IT HAS!!!!!
        Debug.Log(net.FeedForward(new float[] {0, 0, 0})[0]);
        Debug.Log(net.FeedForward(new float[] {0, 0, 1})[0]);
        Debug.Log(net.FeedForward(new float[] {0, 1, 0})[0]);
        Debug.Log(net.FeedForward(new float[] {0, 1, 1})[0]);
        Debug.Log(net.FeedForward(new float[] {1, 0, 0})[0]);
        Debug.Log(net.FeedForward(new float[] {1, 0, 1})[0]);
        Debug.Log(net.FeedForward(new float[] {1, 1, 0})[0]);
        Debug.Log(net.FeedForward(new float[] {1, 1, 1})[0]);
    }

    // Update is called once per frame
    private void Update() { }
}