using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Components;
using System.Collections.Generic;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NPCMasterController
    {
        private int enemyCount;
        private ObjectPoolComponent pool;
        private NPCMovementController[] npcs;
        private JumpComponent player;

        public bool IsOnPause { get; private set; } = false;

        public IReadOnlyList<NPCMovementController> NPCs => npcs;

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
            foreach (var npc in npcs)
            {
                npc.SetPause(pause);
            }
        }

        public NPCMasterController(int enemyCount, ObjectPoolComponent pool, JumpComponent player, MapController map)
        {
            this.player = player;

            this.enemyCount = enemyCount;
            this.pool = pool;

            npcs = new NPCMovementController[enemyCount];
            for (int i = 0; i < npcs.Length; i++)
            {
                var npc = pool.GetObject();
                var movableNpc = npc.GetComponent<JumpComponent>();
                if (movableNpc is null)
                {
                    movableNpc = npc.AddComponent<JumpComponent>();
                }

                npcs[i] = new NPCMovementController(movableNpc, player, map);

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
    }
}
