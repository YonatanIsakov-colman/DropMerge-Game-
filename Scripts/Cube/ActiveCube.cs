using System;
using Events;
using UnityEngine;
using EventBus = EventBusScripts.EventBus;


public class ActiveCube : BaseCube
{

    public Vector3 Target;
    
    public override void Initialize(int value)
    {
        base.Initialize(value);
        
    }

    private void Update()
    {
        if (transform.position == Target)
            EventBus.Get<CubeCollideEvent>().Invoke();
    }
}
