using UnityEngine;

public class StaticCube : BaseCube
{
    public void Initialize(ActiveCube activeCube)
    {
        OnTop = activeCube.OnTop;
        OnBottom = activeCube.OnBottom;
    }
}
