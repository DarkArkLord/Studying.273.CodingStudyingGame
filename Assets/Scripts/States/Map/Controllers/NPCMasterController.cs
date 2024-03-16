﻿using Assets.Scripts.CommonComponents;
using Assets.Scripts.States.Map.Components;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine;

namespace Assets.Scripts.States.Map.Controllers
{
    public class NPCMasterController
    {
        private int npcCount;
        private ObjectPoolComponent pool;
        private NPCMovementController[] npcs;
        private PlayerMovementController player;

        public ObjectPoolComponent Pool => pool;

        public bool IsOnPause { get; private set; } = false;

        public IReadOnlyList<NPCMovementController> NPCs => npcs;

        public NPCMasterController(int npcCount, ObjectPoolComponent pool, PlayerMovementController player, MapController map)
        {
            this.player = player;

            this.npcCount = npcCount;
            this.pool = pool;

            npcs = new NPCMovementController[npcCount];
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
    }
}
