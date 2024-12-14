using System;
using System.Collections.Generic;
using EventBusScripts;
using Events;
using Zenject;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int ROWS = 7;
    private const int COLS = 5;

    [SerializeField] private GridManager _gridManager;

    private void Start()
    {
        _gridManager.Initialize(ROWS,COLS);
    }
    
}