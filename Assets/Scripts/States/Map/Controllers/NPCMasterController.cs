using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Controllers.Interfaces;
using System.Collections.Generic;

using NpcInitFunc = System.Func<UnityEngine.GameObject,
    Assets.Scripts.States.Map.Controllers.Interfaces.IObjectWithPosition2D,
    Assets.Scripts.States.Map.Controllers.MapController,
    Assets.Scripts.States.Map.Controllers.NpcType,
    Assets.Scripts.States.Map.Controllers.Interfaces.INpcController>;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NpcMasterController
    {
        private int npcCount;
        private ObjectPoolComponent pool;
        private INpcController[] npcs;
        private IObjectWithPosition2D player;
        private NpcType npcType;

        public ObjectPoolComponent Pool => pool;

        public bool IsOnPause { get; private set; } = false;

        public IReadOnlyList<INpcController> NPCs => npcs;

        public NpcMasterController(int npcCount, ObjectPoolComponent pool, IObjectWithPosition2D player, MapController map, NpcType type, NpcInitFunc initFunc)
        {
            this.player = player;

            this.npcCount = npcCount;
            this.pool = pool;

            this.npcType = type;

            npcs = new INpcController[npcCount];
            for (int i = 0; i < npcs.Length; i++)
            {
                var npc = pool.GetObject();
                //var movableNpc = npc.GetComponent<JumpComponent>();
                //if (movableNpc is null)
                //{
                //    movableNpc = npc.AddComponent<JumpComponent>();
                //}

                //new NpcMovementController(movableNpc, player, map);

                npcs[i] = initFunc(npc, player, map, type);
                npcs[i].Resurrect();
            }
        }

        public void OnUpdate()
        {
            if (IsOnPause) return;

            foreach (var npc in npcs)
            {
                npc.OnUpdate();
            }
        }

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
            foreach (var npc in npcs)
            {
                npc.SetPause(pause);
            }
        }

        public void SetActive(bool active)
        {
            Pool.gameObject.SetActive(active);
        }

        public void OnDestroy()
        {
            Pool.FreeAllObjects();
        }
    }
}
