using System;
using Events;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using EventBus = EventBusScripts.EventBus;

namespace DefaultNamespace
{
    public class GameInput:MonoBehaviour
    {
        private PlayerInputAction _playerInputAction;
        private void Awake()
        {
            _playerInputAction = new PlayerInputAction();
            _playerInputAction.Controls.Move.performed += KeboardMoveHandler;
            _playerInputAction.Controls.Drop.performed += KeboardDropCubeHandler;
            _playerInputAction.Controls.Click.started += MouseStartDragHandler;
            _playerInputAction.Controls.Click.canceled += MouseStopDragHandler;
            _playerInputAction.Controls.Point.performed += MouseDraggingHandler;

        }

        private void MouseDraggingHandler(InputAction.CallbackContext obj)
        {
            Vector2 position = Mouse.current.position.ReadValue();
            EventBus.Get<MouseDraggingEvent>().Invoke(position);
        }

        private void MouseStartDragHandler(InputAction.CallbackContext obj)
        {
            Vector2 position = Mouse.current.position.ReadValue();
            EventBus.Get<MouseStartDragEvent>().Invoke(position);
        } 
        private void MouseStopDragHandler(InputAction.CallbackContext obj)
        {
            EventBus.Get<MouseStopDragEvent>().Invoke();
        }

        private void KeboardDropCubeHandler(InputAction.CallbackContext obj)
        {
            EventBus.Get<KeyboardDropEvent>().Invoke();
        }

        private void KeboardMoveHandler(InputAction.CallbackContext obj)
        {
            Vector2 direction = obj.ReadValue<Vector2>();
            EventBus.Get<KeyboardMoveEvent>().Invoke(direction);
        }

        private void OnEnable()
        {
            _playerInputAction.Controls.Enable();
        }
        private void OnDisable()
        {
            _playerInputAction.Controls.Disable();
        }
    }
}