using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class PlayerMovementController
    {
        private SmoothMoveComponent _player;
        private SmoothMoveComponent _camera;

        public bool IsOnPause { get; private set; }
        public bool IsMoving { get; private set; }

        public PlayerMovementController(SmoothMoveComponent player, SmoothMoveComponent camera)
        {
            _player = player;
            _camera = camera;

            IsOnPause = false;
            IsMoving = false;
        }

        public void OnUpdate(MoveDirection? moveDirection)
        {
            if (IsOnPause) return;

            if (!IsMoving)
            {
                var movementVector = moveDirection.DirectionToVectorInt();
                if (movementVector.magnitude > 0)
                {
                    IsMoving = true;
                    var isCanMove = true; // ??
                    if (isCanMove)
                    {
                        var offset = new Vector3(movementVector.x, 0, movementVector.y);

                        _player.SetTarget(_player.transform.position + offset);
                        _camera.SetTarget(_camera.transform.position + offset);
                    }
                    else
                    {
                        _player.SetTarget(_player.transform.position);
                    }
                }
            }
            else
            {
                var isPlayerMoved = _player.Move();
                var isCameraMoved = _camera.Move();

                IsMoving = isPlayerMoved || isCameraMoved;
            }
        }
    }
}
