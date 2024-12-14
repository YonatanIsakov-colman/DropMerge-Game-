using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Debug.Log("GameInstaller Started");
        Container.Bind<ICubeFactory>().To<CubeFactory>().AsSingle();
    }
}