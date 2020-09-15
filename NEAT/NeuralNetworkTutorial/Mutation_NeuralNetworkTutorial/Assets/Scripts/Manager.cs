using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject boomerPrefab;
    public GameObject hex;
    private readonly int[] _layers = {1, 10, 10, 1}; //1 input and 1 output
    private List<Boomerang> _boomerangList;
    private int _generationNumber;

    private bool _isTraining;
    private bool _leftMouseDown;
    private List<NeuralNetwork> _nets;
    private int _populationSize = 50;

    private void Update()
    {
        if (_isTraining == false)
        {
            if (_generationNumber == 0)
            {
                InitBoomerangNeuralNetworks();
            }
            else
            {
                _nets.Sort();
                for (var i = 0; i < _populationSize / 2; i++)
                {
                    _nets[i] = new NeuralNetwork(_nets[i + _populationSize / 2]);
                    _nets[i].Mutate();
                    _nets[i + _populationSize / 2] = new NeuralNetwork(_nets[i + _populationSize / 2]);
                }

                for (var i = 0; i < _populationSize; i++) _nets[i].SetFitness(0f);
            }

            _generationNumber++;
            _isTraining = true;
            Invoke("Timer", 15f);
            CreateBoomerangBodies();
        }


        if (Input.GetMouseButtonDown(0)) _leftMouseDown = true;
        else if (Input.GetMouseButtonUp(0)) _leftMouseDown = false;
        if (!_leftMouseDown) return;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hex.transform.position = mousePosition;
    }

    private void Timer() { _isTraining = false; }

    private void CreateBoomerangBodies()
    {
        if (_boomerangList != null)
            foreach (var t in _boomerangList)
                Destroy(t.gameObject);
        _boomerangList = new List<Boomerang>();
        for (var i = 0; i < _populationSize; i++)
        {
            var boomer =
                Instantiate(boomerPrefab, new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0),
                            boomerPrefab.transform.rotation).GetComponent<Boomerang>();
            boomer.Init(_nets[i], hex.transform);
            _boomerangList.Add(boomer);
        }
    }

    private void InitBoomerangNeuralNetworks()
    {
        if (_populationSize % 2 != 0) _populationSize = 20;
        _nets = new List<NeuralNetwork>();
        for (var i = 0; i < _populationSize; i++)
        {
            var net = new NeuralNetwork(_layers);
            net.Mutate();
            _nets.Add(net);
        }
    }
}