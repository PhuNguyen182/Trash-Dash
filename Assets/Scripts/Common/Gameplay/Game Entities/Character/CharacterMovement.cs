using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrashDash.Scripts.Common.Enumerations;

namespace TrashDash.Scripts.Common.Gameplay.GameEntities.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Transform blobShadow;
        [SerializeField] private Transform characterPoint;

        [Header("Player Components")]
        [SerializeField] private CharacterInput characterInput;
        [SerializeField] private CharacterCollider characterCollider;
        [SerializeField] private CharacterAnimation characterAnimation;

        [Header("Movement Config")]
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float slideDuration = 0.25f;
        [SerializeField] private float jumpDuration;

        private const float LeftSide = -1.5f;
        private const float RightSide = 1.5f;

        private bool _canJump = true;
        private bool _canSlide = true;
        private bool _hasSoundComponent = false;

        private Vector3 _toPosition;
        private Vector3 _startPosition, _endPosition;
        private CharacterSound _characterSound;

        private Coroutine _moveCoroutine;
        private Coroutine _jumpCoroutine;
        private Coroutine _slideCoroutine;

        private YieldInstruction _waitForSlide = new WaitForSeconds(0.25f);

        private void Awake()
        {
            _characterSound = GetComponentInChildren<CharacterSound>();
            _hasSoundComponent = _characterSound != null;

            _toPosition = characterPoint.localPosition;
            characterAnimation.PlayRunStart();
            characterAnimation.SetJumpSpeed(1);
        }

        private void Update()
        {
            if (IsMobilePlatform())
                MobileInputHandle();
            else
                StandaloneInputHandle();

            blobShadow.localPosition = new Vector3(characterPoint.localPosition.x, 0.01f, 0);
        }

        private bool IsMobilePlatform()
        {
#if UNITY_ANDROID || UNITY_IOS
            return true;
#else
            return false;
#endif
        }

        private void StandaloneInputHandle()
        {
            if (characterInput.IsPressed)
            {
                Move(characterInput.SwipeDirection);
            }
        }

        private void MobileInputHandle()
        {
            if (characterInput.IsPressed && !characterInput.HasSwiped)
                characterInput.HasSwiped = true;

            if (characterInput.HasSwiped && characterInput.CanSwipe())
            {
                characterInput.CalculateSwipe();
                characterInput.HasSwiped = false;
                Move(characterInput.SwipeDirection);
            }

            if (characterInput.IsReleased)
                characterInput.HasSwiped = false;
        }

        private void Move(Vector2Int direction)
        {
            if (direction == Vector2Int.left)
                MoveLeft();

            if (direction == Vector2Int.right)
                MoveRight();

            if (direction == Vector2Int.up && _canJump)
                JumpUp();

            if (direction == Vector2Int.down && _canSlide)
                SlideDown();
        }

        private void MoveLeft()
        {
            if (characterPoint.localPosition.x > LeftSide)
                _toPosition += Vector3.left * RightSide;

            if (_toPosition.x < LeftSide)
                _toPosition += Vector3.right * RightSide;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveLocalTo(_toPosition));
        }

        private void MoveRight()
        {
            if (characterPoint.localPosition.x < RightSide)
                _toPosition += Vector3.right * RightSide;

            if (_toPosition.x > RightSide)
                _toPosition += Vector3.left * RightSide;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveLocalTo(_toPosition));
        }

        private void JumpUp()
        {
            if (_slideCoroutine != null)
                StopCoroutine(_slideCoroutine);

            _canSlide = true;

            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);

            if (_hasSoundComponent)
                _characterSound.PlayJump();

            _jumpCoroutine = StartCoroutine(Jump());
        }

        private void SlideDown()
        {
            if (_jumpCoroutine != null)
                StopCoroutine(_jumpCoroutine);

            if (_slideCoroutine != null)
                StopCoroutine(_slideCoroutine);

            if (_hasSoundComponent)
                _characterSound.PlaySlide();

            _slideCoroutine = StartCoroutine(Slide());
        }

        private IEnumerator MoveLocalTo(Vector3 toLocalPosition)
        {
            float ratio;
            float elapsedTime = 0;
            Vector3 movePosition;

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                ratio = elapsedTime / moveDuration;
                movePosition = new Vector3
                {
                    x = toLocalPosition.x,
                    y = characterPoint.localPosition.y,
                    z = toLocalPosition.z
                };

                characterPoint.localPosition = Vector3.Lerp(characterPoint.localPosition, movePosition, ratio);

                yield return null;
            }

            movePosition = new Vector3
            {
                x = toLocalPosition.x,
                y = characterPoint.localPosition.y,
                z = toLocalPosition.z
            };

            characterPoint.localPosition = movePosition;
        }

        private IEnumerator Jump()
        {
            _canJump = false;
            characterCollider.SetCollider(HighColliderEnum.High);
            characterAnimation.PlayJump(true);
            characterAnimation.SetJumpSpeed(1);

            float height;
            float elapsedTime = 0;
            Vector3 movePosition;

            while (elapsedTime < jumpDuration)
            {
                elapsedTime += Time.deltaTime * 2;
                height = Mathf.Sin(elapsedTime * Mathf.PI) * jumpHeight;
                movePosition = new Vector3
                {
                    x = characterPoint.localPosition.x,
                    y = height,
                    z = characterPoint.localPosition.z
                };

                characterPoint.localPosition = movePosition;
                yield return null;
            }

            movePosition = new Vector3
            {
                x = characterPoint.localPosition.x,
                y = 0,
                z = characterPoint.localPosition.z
            };

            characterAnimation.PlayJump(false);
            characterPoint.localPosition = movePosition;

            _canJump = true;
        }

        private IEnumerator Slide()
        {
            _canSlide = false;
            _canJump = false;

            characterCollider.SetCollider(HighColliderEnum.Low);
            characterAnimation.PlaySlide(true);
            characterAnimation.PlayJump(false);

            float ratio;
            float elapsedTime = 0;

            Vector3 movePosition = new Vector3
            {
                x = characterPoint.localPosition.x,
                y = 0,
                z = characterPoint.localPosition.z
            };

            while (elapsedTime < slideDuration)
            {
                elapsedTime += Time.deltaTime;
                ratio = elapsedTime / slideDuration;
                characterPoint.localPosition = Vector3.Lerp(characterPoint.localPosition, movePosition, ratio);

                yield return null;
            }

            yield return _waitForSlide;

            movePosition = new Vector3
            {
                x = characterPoint.localPosition.x,
                y = 0,
                z = characterPoint.localPosition.z
            };

            characterAnimation.PlaySlide(false);
            characterPoint.localPosition = movePosition;
            characterCollider.SetCollider(HighColliderEnum.High);

            _canSlide = true;
            _canJump = true;
        }
    }
}
