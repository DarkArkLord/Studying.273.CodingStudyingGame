using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Components.MapGenerators;
using Assets.Scripts.States.Map.Controllers.Interfaces;
using Assets.Scripts.States.Map.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.States.Map.Controllers
{
    public class PlayerMovementController : IMapEntityController
    {
        private SmoothMoveComponent _player;
        private SmoothMoveComponent _camera;

        private FloorController floor;

        public bool IsOnPause { get; private set; } = false;
        public bool IsMoving { get; private set; } = false;

        public bool IsAlive => true;

        public Vector2Int Position2D => _player.Position2D;

        private Vector3 lastCameraPosition;
        private Vector3 prevPlayerPosition;

        public UnityEvent StandOnInputEvent { get; private set; } = new UnityEvent();
        public UnityEvent StandOnOutputEvent { get; private set; } = new UnityEvent();

        public PlayerMovementController(SmoothMoveComponent player, SmoothMoveComponent camera, FloorController floor)
        {
            _player = player;
            _camera = camera;

            lastCameraPosition = camera.transform.position;

            this.floor = floor;
        }

        public void OnUpdate(MoveDirection? moveDirection)
        {
            if (IsOnPause) return;

            if (!IsMoving)
            {
                var movementVector = moveDirection.DirectionToVector2Int();
                if (movementVector.magnitude > 0)
                {
                    prevPlayerPosition = _player.Transform.position;
                    IsMoving = true;
                    var isCanMove = floor.Map.IsCanMove(_player.Position2D + movementVector);
                    if (isCanMove)
                    {
                        var offset = new Vector3(movementVector.x, 0, movementVector.y);

                        _player.SetTarget(_player.Transform.position + offset, offset);
                        _camera.SetTarget(_camera.Transform.position + offset);
                    }
                    else
                    {
                        _player.SetTarget(_player.Transform.position);
                        _camera.SetTarget(_camera.Transform.position);
                    }
                }
            }
            else
            {
                var isPlayerMoved = _player.Move();
                var isCameraMoved = _camera.Move();

                IsMoving = isPlayerMoved || isCameraMoved;

                if (!IsMoving)
                {
                    lastCameraPosition = _camera.gameObject.transform.position;
                    if (prevPlayerPosition != _player.Transform.position)
                    {
                        var cell = floor.Map.GetMapCell(_player.Position2D);
                        if (cell == MapCellContent.Input)
                        {
                            StandOnInputEvent.Invoke();
                        }
                        else if (cell == MapCellContent.Output)
                        {
                            StandOnOutputEvent.Invoke();
                        }
                    }
                }
            }
        }

        public void Kill()
        {
            IsMoving = false;
            Resurrect();
        }

        public void Resurrect()
        {
            //var offset = new Vector3(floor.Map.StartX, _player.Transform.position.y, floor.Map.StartY) - _player.Transform.position;
            //_player.Transform.position += offset;
            //_camera.Transform.position += offset;
            _player.Transform.position = new Vector3(floor.Map.StartX, 0, floor.Map.StartY);
            _camera.Transform.position = new Vector3(floor.Map.StartX, 5, floor.Map.StartY - 10);

            lastCameraPosition = _camera.transform.position;
        }

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
        }

        public void SetActive(bool active)
        {
            _player.gameObject.SetActive(active);
            if (active)
            {
                _camera.gameObject.transform.position = lastCameraPosition;
            }
            else
            {
                lastCameraPosition = _camera.gameObject.transform.position;
            }
        }
    }
}
