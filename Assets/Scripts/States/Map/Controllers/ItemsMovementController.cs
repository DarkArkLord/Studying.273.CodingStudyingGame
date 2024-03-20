using Assets.Scripts.States.Map.Components;
using Assets.Scripts.States.Map.Controllers.Interfaces;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class ItemsMovementController : INpcController
    {
        public static INpcController Create(GameObject obj, IObjectWithPosition2D player, MapController map)
        {
            var movableItem = obj.GetComponent<JumpComponent>();
            if (movableItem is null)
            {
                movableItem = obj.AddComponent<JumpComponent>();
            }

            return new ItemsMovementController(movableItem, player, map);
        }

        private JumpComponent item;
        private IObjectWithPosition2D player;
        private MapController map;

        private System.Random random = RandomUtils.Random;

        private bool isInteractive = false;

        public bool IsAlive { get; private set; } = false;
        public bool IsInteractive => IsAlive && isInteractive;
        public bool IsOnPause { get; private set; } = false;

        public Vector2Int Position2D => item.Position2D;

        public ItemsMovementController(JumpComponent item, IObjectWithPosition2D player, MapController map)
        {
            this.item = item;
            this.player = player;
            this.map = map;
        }

        public void OnUpdate()
        {
            //
        }

        public void Kill()
        {
            IsAlive = false;
            item.GameObject.SetActive(false);
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
                    item.Transform.position = new Vector3(x, 0, y);
                    item.GameObject.SetActive(true);
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
