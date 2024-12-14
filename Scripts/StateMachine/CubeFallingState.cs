using UnityEngine;

namespace StateMachine
{
    public class CubeFallingState:IState
    {
        private GridManager _gridManager;

        public CubeFallingState(GridManager gridManager)
        {
            _gridManager = gridManager;
        }
        public void Enter()
        {
            
        }

        public void Update()
        {
            if (!_gridManager.ActiveCubeIsNull())
            {
                _gridManager.MoveActiveCube();
            }
        }

        public void Exit()
        {
            
        }
    }
}