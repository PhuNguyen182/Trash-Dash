using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterInput : MonoBehaviour
    {
        [SerializeField] private float minSwipeLength = 0.5f;

        private const float MinCalculateDirection = 0.5f;
        private const float MaxCalculateDirection = 1.0f;

        private PlayerInput _playerInput;

        private Vector2 _swipeDelta;
        private Vector2 _swipeDirection;

        public bool IsPressed => IsPointerDown();
        public bool IsReleased => IsPointerUp();

        public bool HasSwiped { get; set; }
        public bool IsActivate { get; set; }
        public Vector2Int SwipeDirection { get; private set; }

        private void Awake()
        {
            _playerInput = new();

            _playerInput.Player.Swipe.started += SwipeHandle;
            _playerInput.Player.Swipe.performed += SwipeHandle;
            _playerInput.Player.Swipe.canceled += SwipeHandle;
        }

        private bool IsPointerDown()
        {
            return _playerInput.Player.Interact.WasPressedThisFrame();
        }

        private bool IsPointerUp()
        {
            return _playerInput.Player.Interact.WasReleasedThisFrame();
        }

        private void SwipeHandle(InputAction.CallbackContext context)
        {
            if (IsActivate)
            {
                _swipeDelta = context.ReadValue<Vector2>();
                _swipeDirection.x = _swipeDelta.x;
                _swipeDirection.y = _swipeDelta.y;
            }

            else _swipeDirection = Vector2.zero;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            EnhancedTouchSupport.Enable();
        }

        public bool CanSwipe()
        {
            return _swipeDirection.sqrMagnitude >= minSwipeLength * minSwipeLength;
        }

        public void CalculateSwipe()
        {
            SwipeDirection = CalculateDirection(_swipeDirection);
        }

        private Vector2Int CalculateDirection(Vector2 direction)
        {
            if (IsCloseToDirection(Vector2.up, direction))
                return Vector2Int.up;

            else if (IsCloseToDirection(Vector2.left, direction))
                return Vector2Int.left;

            else if (IsCloseToDirection(Vector2.down, direction))
                return Vector2Int.down;

            else if (IsCloseToDirection(Vector2.right, direction))
                return Vector2Int.right;

            return Vector2Int.zero;
        }

        private bool IsCloseToDirection(Vector2 checkDirection, Vector2 closeDirection)
        {
            float dotProduct = Vector2.Dot(checkDirection, closeDirection.normalized);
            return dotProduct > MinCalculateDirection && dotProduct <= MaxCalculateDirection;
        }

        private void OnDisable()
        {
            _playerInput.Disable();
            EnhancedTouchSupport.Disable();
        }

        private void OnDestroy()
        {
            _playerInput.Dispose();
        }
    }
}
