using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterInput : MonoBehaviour
    {
        private Touch touch;
        private NavigationDirection direction;
        private Vector2 touchInputVector;

        private bool _isActive = true;
        private bool _isUp;
        private bool _isDown;
        private bool _isLeft;
        private bool _isRight;

        public bool IsUp => _isUp;
        public bool IsDown => _isDown;
        public bool IsLeft => _isLeft;
        public bool IsRight => _isRight;

        public bool IsActivate 
        { 
            get => _isActive; 
            set => _isActive = value; 
        }

        private void Update()
        {
            if (IsActivate)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                StandaloneInput();
#elif UNITY_ANDROID
                MobileInput();
#endif
            }
        }

        private void StandaloneInput()
        {
            _isUp = Input.GetKeyDown(KeyCode.UpArrow);
            _isDown = Input.GetKeyDown(KeyCode.DownArrow);
            _isLeft = Input.GetKeyDown(KeyCode.LeftArrow);
            _isRight = Input.GetKeyDown(KeyCode.RightArrow);
        }

        private void MobileInput()
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    // Should be divide to screen size in order to prevent return too fast input value
                    touchInputVector.x = touch.deltaPosition.x / Screen.width;
                    touchInputVector.y = touch.deltaPosition.y / Screen.height;

                    touchInputVector = touchInputVector.sqrMagnitude > 1 ? touchInputVector.normalized : touchInputVector;
                    CalculateDirection(touchInputVector);
                }

                else
                {
                    touchInputVector = Vector2.zero;
                    CalculateDirection(touchInputVector);
                }
            }

            else
            {
                touchInputVector = Vector2.zero;
                CalculateDirection(touchInputVector);
            }
        }

        private void CalculateDirection(Vector2 dir)
        {
            Vector2 normalizedDir = dir.normalized;
            float x = normalizedDir.x, y = normalizedDir.y;

            if (dir.sqrMagnitude > 0)
            {
                if (x > 0)
                {
                    if (Mathf.Abs(y) <= x)
                        direction = NavigationDirection.Right;
                    else
                        direction = y > 0 ? NavigationDirection.Up : NavigationDirection.Down;
                }

                else if (x < 0)
                {
                    if (Mathf.Abs(y) <= Mathf.Abs(x))
                        direction = NavigationDirection.Left;
                    else
                        direction = y > 0 ? NavigationDirection.Up : NavigationDirection.Down;
                }

                else
                {
                    direction = y > 0 ? NavigationDirection.Up : NavigationDirection.Down;
                }
            }

            else
                direction = NavigationDirection.None;

            _isUp = direction == NavigationDirection.Up;
            _isDown = direction == NavigationDirection.Down;
            _isLeft = direction == NavigationDirection.Left;
            _isRight = direction == NavigationDirection.Right;
        }
    }
}
