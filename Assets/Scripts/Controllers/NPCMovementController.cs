using Assets.Scripts.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class NPCMovementController
    {
        private IMovableEntity npc;
        private IMovableEntity player;
        private MapController map;

        private System.Random random = RandomUtils.Random;

        private readonly float timeBeforeSteps;
        private readonly float timeForRespawn;

        public float RespawnWaitingTime { get; private set; }
        private float moveWaitingTime;

        public bool IsMoving { get; private set; } = false;
        public bool IsAlive { get; private set; } = false;

        public NPCMovementController(IMovableEntity npc, IMovableEntity player, MapController map, float timeBeforeSteps = 2, float timeForRespawn = 5)
        {
            this.npc = npc;
            this.player = player;
            this.map = map;
            this.timeBeforeSteps = timeBeforeSteps;
            this.timeForRespawn = timeForRespawn;
            moveWaitingTime = timeBeforeSteps;
        }

        public void OnUpdate()
        {
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
                        IsMoving = true;
                        SetMoveTarget();
                        moveWaitingTime = timeBeforeSteps;
                    }
                }
            }
            else
            {
                RespawnWaitingTime -= Time.deltaTime;
            }
        }

        private void SetMoveTarget()
        {
            while (true)
            {
                var target = npc.Position2D + random.GetRandomDirection();
                if (map.IsCanMove(target))
                {
                    var target3D = new Vector3(target.x, 0, target.y);
                    npc.SetTarget(target3D);
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

                if (map.IsCanMove(x, y) && playerPos.x != x && playerPos.y != y)
                {
                    npc.Transform.position = new Vector3(x, 0, y);
                    npc.GameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
