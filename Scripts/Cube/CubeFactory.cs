using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

public interface ICubeFactory
{
    ActiveCube CreateCube(Vector3 position); 
}
public class CubeFactory:ICubeFactory
{
    private const string CUBE_PREFAB_PATH = "Prefabs/Cube";
    private readonly ActiveCube _cubePrefab;
    private int[] _cubeValues;

    public CubeFactory()
    {
        _cubePrefab=Resources.Load<ActiveCube>(CUBE_PREFAB_PATH);
        _cubeValues = new[] { 2, 4, 8, 16,32};
    }

    public ActiveCube CreateCube(Vector3 position)
    { 
        ActiveCube newCube = GameObject.Instantiate(_cubePrefab, position, Quaternion.identity);
        int _randomIndex = GetRandomIndex();
        newCube.Initialize(_cubeValues[_randomIndex]);
        return newCube;
    }
    private int GetRandomIndex() 
    {
        return Random.Range(0, _cubeValues.Length);
    }



}
