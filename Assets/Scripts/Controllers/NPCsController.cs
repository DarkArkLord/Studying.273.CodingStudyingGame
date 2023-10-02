using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{
    public class NPCsController
    {
        private int enemyCount;
        private IObjectPool pool;
        private NPCMovementController[] npcs;
        private IEntityWithPosition player;

        public bool IsOnPause { get; private set; } = false;

        public void SetPause(bool pause)
        {
            IsOnPause = pause;
            foreach (var npc in npcs)
            {
                npc.SetPause(pause);
            }
        }

        public NPCsController(int enemyCount, IObjectPool pool, IEntityWithPosition player, MapController map)
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
