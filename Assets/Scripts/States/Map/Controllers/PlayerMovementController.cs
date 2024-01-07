using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Controllers
{
    public class PlayerMovementController
    {
        private SmoothMoveComponent _player;
        private SmoothMoveComponent _camera;

        private FloorController floor;

        public bool IsOnPause { get; private set; } = false;
        public bool IsMoving { get; private set; } = false;

        public bool IsAlive => true;

        public Vector2Int Position2D => _player.Position2D;

        public PlayerMovementController(SmoothMoveComponent player, SmoothMoveComponent camera, FloorController floor)
        {
            _player = player;
            _camera = camera;

            this.floor = floor;
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

                        _player.SetTarget(_player.Transform.position + offset);
                        _camera.SetTarget(_camera.Transform.position + offset);
                    }
                    else
                    {
                        _player.SetTarget(_player.Transform.position);
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

        public void Kill()
        {
            IsMoving = false;
            Resurrect();
        }

        public void Resurrect()
        {
            var offset = new Vector3(floor.Map.StartX, 0, floor.Map.StartY) - _player.Transform.position;
            _player.Transform.position += offset;
            _camera.Transform.position += offset;
        }
    }
}
