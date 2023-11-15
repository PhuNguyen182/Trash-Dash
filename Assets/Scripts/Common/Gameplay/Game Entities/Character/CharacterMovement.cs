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
        [SerializeField] private CharacterInput characterInput;
        [SerializeField] private CharacterCollider characterCollider;
        [SerializeField] private CharacterAnimation characterAnimation;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private float jumpDuration;

        private bool _canJump = true;
        private bool _canSlide = true;
        private bool _hasSoundComponent = false;

        private Vector3 _toPosition;
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
            if (characterInput.IsLeft)
                MoveLeft();

            if (characterInput.IsRight)
                MoveRight();

            if (characterInput.IsUp && _canJump)
                JumpUp();

            if (characterInput.IsDown && _canSlide)
                SlideDown();

            blobShadow.localPosition = new Vector3(characterPoint.localPosition.x, 0.01f, 0);
        }

        private void MoveLeft()
        {
            if (characterPoint.localPosition.x > -1.5f)
                _toPosition += Vector3.left * 1.5f;

            if (_toPosition.x < -1.5f)
                _toPosition += Vector3.right * 1.5f;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(MoveLocalTo(_toPosition));
        }

        private void MoveRight()
        {
            if (characterPoint.localPosition.x < 1.5f)
                _toPosition += Vector3.right * 1.5f;

            if (_toPosition.x > 1.5f)
                _toPosition += Vector3.left * 1.5f;

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
                height = Mathf.Sin(elapsedTime * Mathf.PI) * 1.2f;
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

            while (elapsedTime < 0.25f)
            {
                elapsedTime += Time.deltaTime;
                ratio = elapsedTime / 0.25f;
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
