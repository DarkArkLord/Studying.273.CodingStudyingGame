using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controllers
{
    public class PlayerMovementController
    {
        private SmoothMoveComponent _player;
        private SmoothMoveComponent _camera;

        private FloorController floor;

        public bool IsOnPause { get; private set; }
        public bool IsMoving { get; private set; }

        public PlayerMovementController(SmoothMoveComponent player, SmoothMoveComponent camera, FloorController floor)
        {
            _player = player;
            _camera = camera;

            this.floor = floor;

            IsOnPause = false;
            IsMoving = false;
        }

        public void SetStartPosition()
        {
            var offset = new Vector3(floor.Map.StartX, _player.transform.position.y, floor.Map.StartY) - _player.transform.position;
            _player.transform.position += offset;
            _camera.transform.position += offset;
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
                    var isCanMove = floor.Map.IsCanMove(_player.Position2D + movementVector);
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

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
        }
    }
}
