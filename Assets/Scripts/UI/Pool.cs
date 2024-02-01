using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _container;

    private Queue<GameObject> _queue;

    private void Start()
    {
        _queue = new Queue<GameObject>();

        InitializeQueue();
    }

    private void InitializeQueue()
    {
        for (int i = 0; i < _count; i++)
        {
            _queue.Enqueue(CreateGameObject());
        }
    }

    public GameObject CreateGameObject()
    {
        return Instantiate(_prefab, _container);
    }

    public void Enqueue(GameObject obj)
    {
        _queue.Enqueue(obj);
    }

    public bool TryDequeue(out GameObject obj)
    {
        return _queue.TryDequeue(out obj);
    }
 }
