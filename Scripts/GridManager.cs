using System;
using DefaultNamespace;
using Events;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using EventBus = EventBusScripts.EventBus;

public class GridManager : MonoBehaviour
{
    private const int DEFAULT_Y_POSITION = 7;
    private const int DEFAULT_Z_POSITION = 0;
    private const int STARTING_DEFAULT_COLUMN = 2;
    private const float CUBE_SIZE = 1.8f;
    
    [SerializeField] private GameObject[] _columns;
    [SerializeField] private GameObject _ground;
    private StateMachine.StateMachine _stateMachine;
    private int moveSpeed = (int)FallSpeed.BasicSpeed;
    [Inject] private ICubeFactory _cubeFactory;

    private BaseCube[,] _board;
    private ActiveCube _activeCube;
    private int[] _cubesInColumn;
    private int _currentColumnIndex;
    private float _groundHeight;
    private bool isDropping = false;
    private bool isDragging;

    private void Awake()
    {
        EventsSubscribe();
    }

    private void EventsSubscribe()
    {
        EventBus.Get<CubeCollideEvent>().Subscribe(OnCollide);
        EventBus.Get<KeyboardMoveEvent>().Subscribe(OnKeyboardMove);
        EventBus.Get<KeyboardDropEvent>().Subscribe(OnKeyboardDrop);
        EventBus.Get<MouseStartDragEvent>().Subscribe(OnMouseStartMove);
        EventBus.Get<MouseDraggingEvent>().Subscribe(OnMouseMove);
        EventBus.Get<MouseStopDragEvent>().Subscribe(OnKeyboardDrop);

    }

    private void OnMouseStartMove(Vector2 position)
    {
        if (isDropping || ActiveCubeIsNull()) return;
        isDragging = true;
        UpdateColumnBasedOnMousePosition(position);
    }

    private void OnMouseMove(Vector2 position)
    {
        if (ActiveCubeIsNull() || !isDragging) return;
        Debug.Log("ONMOUSEMOVE: " + position);
        UpdateColumnBasedOnMousePosition(position);

    }

    private void UpdateColumnBasedOnMousePosition(Vector2 screenPosition)
    {
        // Convert screen position to world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            screenPosition.x,
            screenPosition.y,
            Camera.main.nearClipPlane));
        Debug.Log("UPDATE COLUMN Worl Position: "+ worldPosition);
        
        // Decline to move if not pressing on one of the columns 
        if (worldPosition.x < _columns[0].transform.position.x-CUBE_SIZE/2 ||
            worldPosition.x > _columns[^1].transform.position.x+CUBE_SIZE/2) return;
        
        float closestDistance = float.MaxValue;
        int closestColumnIndex = _currentColumnIndex;
        // Find the closest column based on the mouse's x-coordinate
        for (int i = 0; i < _columns.Length; i++)
        {
            float distance = Mathf.Abs(worldPosition.x - _columns[i].transform.position.x);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestColumnIndex = i;
            }
        }
        
        // Update the cube's column if it has changed
        if (closestColumnIndex != _currentColumnIndex)
        {
            _currentColumnIndex = CheckForPossibleColumn(closestColumnIndex);
            _activeCube.transform.position = new Vector3(
                _columns[_currentColumnIndex].transform.position.x,
                _activeCube.transform.position.y,
                _activeCube.transform.position.z
            );
            Debug.Log($"Cube moved to column {_currentColumnIndex}");
            UpdateTargetPosition(); 
        }
    }

    private int CheckForPossibleColumn(int columnTarget)
    {
        int step = columnTarget > _currentColumnIndex ? +1 : -1;
        int tempColumn = _currentColumnIndex;
        for (int i = _currentColumnIndex+step; i-step != columnTarget; i+= step)
        {
            if (_cubesInColumn[i] > 0 &&
                _board[_cubesInColumn[i] - 1,
                    i].OnTop.transform.position.y >=
                _activeCube.OnBottom.transform.position.y)
                return tempColumn;
            tempColumn = i;
        }

        return tempColumn;
    }


    private void OnKeyboardDrop()
    {
        if (ActiveCubeIsNull() ) return;
            SetMoveSpeedToOnClick();
            isDropping = true;
            isDragging = false;
    }
    private void OnKeyboardMove(Vector2 direction)
    {
        if (ActiveCubeIsNull()|| isDropping) return;
        
        switch (direction.x)
        {
            case < 0 when _currentColumnIndex > 0 :
                MoveLeft();
                break;
            case > 0 when _currentColumnIndex < _columns.Length - 1 :
                MoveRight();
                break;
        }
    }

    private void MoveRight()
    {
        if (_cubesInColumn[_currentColumnIndex + 1] > 0 &&
            _board[_cubesInColumn[_currentColumnIndex + 1] - 1,
                _currentColumnIndex + 1].OnTop.transform.position.y >=
            _activeCube.OnBottom.transform.position.y)
            return;

        _currentColumnIndex++;
        _activeCube.transform.position = new Vector3(_columns[_currentColumnIndex].transform.position.x,
            _activeCube.transform.position.y, _activeCube.transform.position.z);
        UpdateTargetPosition(); 
    }

    private void MoveLeft()
    {
        if (_cubesInColumn[_currentColumnIndex - 1]>0 && _board[_cubesInColumn[_currentColumnIndex-1]-1,_currentColumnIndex-1].OnTop.transform.position.y>=_activeCube.OnBottom.transform.position.y ) return;

        _currentColumnIndex--;
        _activeCube.transform.position = new Vector3(_columns[_currentColumnIndex].transform.position.x,
            _activeCube.transform.position.y, _activeCube.transform.position.z);
        UpdateTargetPosition();
    }

    private void OnCollide()
    {
        isDropping = false;
        isDragging = false;
        SetMoveSpeedToBasic();
        PlaceCubeInBoard(GetStaticCube());
        if(_cubesInColumn[_currentColumnIndex]<_board.GetLength(0))
            CreateNewActiveCube();
    }

    private void PlaceCubeInBoard(StaticCube staticCube)
    {
        _board[_cubesInColumn[_currentColumnIndex], _currentColumnIndex] = staticCube;
        _cubesInColumn[_currentColumnIndex]++;
            
    }

    public bool ActiveCubeIsNull()
    {
        return _activeCube == null;
    }
    private StaticCube GetStaticCube()
    {
        var staticCube = _activeCube.AddComponent<StaticCube>();
        staticCube.Initialize(_activeCube);
        Destroy(_activeCube);
        _activeCube = null;
        return staticCube;
    }

    public void Initialize(int rows, int columns)
    {
        _board = new BaseCube[rows, columns];
        _cubesInColumn = new int[rows];
        _groundHeight = _ground.transform.position.y;
        _currentColumnIndex = STARTING_DEFAULT_COLUMN;
        CreateNewActiveCube();
        _stateMachine = new StateMachine.StateMachine(this);
        _stateMachine.Initialize(_stateMachine.CubeFallingState);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    private void CreateNewActiveCube()
    {
        Vector3 spawnPosition = new Vector3(_columns[_currentColumnIndex].transform.position.x,
            DEFAULT_Y_POSITION,
            DEFAULT_Z_POSITION);
        _activeCube = _cubeFactory.CreateCube(spawnPosition);
        UpdateTargetPosition();
    }

    private void UpdateTargetPosition()
    {
        Vector3 newTarget = _activeCube.transform.position;
        newTarget.y = _groundHeight + _cubesInColumn[_currentColumnIndex] * CUBE_SIZE;
        _activeCube.Target = newTarget;
    }

    public void MoveActiveCube()
    {
        _activeCube.transform.position = Vector3.MoveTowards(
            _activeCube.transform.position,
            _activeCube.Target,
            moveSpeed * Time.deltaTime);
    }

    private void SetMoveSpeedToBasic()
    {
        moveSpeed = (int)FallSpeed.BasicSpeed;
    }
    private void SetMoveSpeedToOnClick()
    {
        moveSpeed = (int)FallSpeed.OnClickSpeed;
    }
}
