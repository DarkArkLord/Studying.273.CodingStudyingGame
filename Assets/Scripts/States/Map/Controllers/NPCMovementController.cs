using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Controllers.Interfaces;
using Assets.Scripts.States.Map.Utils;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NpcMovementController : INpcController
    {
        public static INpcController Create(GameObject obj, IObjectWithPosition2D player, MapController map)
        {
            var movableNpc = obj.GetComponent<JumpComponent>();
            if (movableNpc is null)
            {
                movableNpc = obj.AddComponent<JumpComponent>();
            }

            return new NpcMovementController(movableNpc, player, map);
        }

        private JumpComponent npc;
        private IObjectWithPosition2D player;
        private MapController map;

        private System.Random random = RandomUtils.Random;

        private readonly float timeBeforeSteps = 2;
        private readonly float timeForRespawn = 5;

        public float RespawnWaitingTime { get; private set; }
        private float moveWaitingTime;

        private bool isInteractive = false;

        public bool IsMoving { get; private set; } = false;
        public bool IsAlive { get; private set; } = false;
        public bool IsInteractive => IsAlive && !IsMoving && isInteractive;
        public bool IsOnPause { get; private set; } = false;

        public Vector2Int Position2D => npc.Position2D;

        public NpcMovementController(JumpComponent npc, IObjectWithPosition2D player, MapController map)
        {
            this.npc = npc;
            this.player = player;
            this.map = map;

            moveWaitingTime = timeBeforeSteps;
        }

        public void OnUpdate()
        {
            if (IsOnPause) return;

            if (IsAlive)
            {
                if (IsMoving)
                {
                    IsMoving = npc.Move();
                }
                else
                {
                    moveWaitingTime -= Time.deltaTime;
                    if (moveWaitingTime < 0)
                    {
                        isInteractive = true;
                        IsMoving = true;
                        SetMoveTarget();
                        moveWaitingTime = timeBeforeSteps;
                    }
                }
            }
            else
            {
                RespawnWaitingTime -= Time.deltaTime;
                if (RespawnWaitingTime < 0)
                {
                    RespawnWaitingTime = 0;
                    Resurrect();
                }
            }
        }

        private void SetMoveTarget()
        {
            while (true)
            {
                var direction = random.GetRandomDirection();
                var dirVector = direction.DirectionToVector2Int();
                var moveTarget = npc.Position2D + dirVector;
                if (map.IsCanMove(moveTarget))
                {
                    var moveTarget3 = new Vector3(moveTarget.x, 0, moveTarget.y);
                    var rotateTarget3 = new Vector3(dirVector.x, 0, dirVector.y);
                    npc.SetTarget(moveTarget3, rotateTarget3);
                    return;
                }
            }
        }

        public void Kill()
        {
            IsAlive = false;
            npc.GameObject.SetActive(false);
            RespawnWaitingTime = timeForRespawn;
        }

        public void Resurrect()
        {
            IsAlive = true;
            var playerPos = player.Position2D;

            while (true)
            {
                var x = random.Next(map.Width);
                var y = random.Next(map.Height);
                // if can move
                // and not current player position
                // and not start player position
                if (map.IsCanMove(x, y) && (playerPos.x != x || playerPos.y != y) && (map.StartX != x || map.StartY != y))
                {
                    npc.Transform.position = new Vector3(x, 0, y);
                    npc.GameObject.SetActive(true);
                    break;
                }
            }

            isInteractive = true;
        }

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
        }

        public void SetInteractive(bool interactive)
        {
            isInteractive = interactive;
        }
    }
}
