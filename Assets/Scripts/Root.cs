using Assets.Scripts.DataKeeper;
using Assets.Scripts.DataKeeper.QuestsSystem;
using Assets.Scripts.StatesMachine;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Root : MonoBehaviour
    {
        #region SINGLETONE

        private static Root _instance;
        public static Root Instance => _instance ?? (_instance = FindObjectOfType<Root>());

        #endregion

        [SerializeField]
        private MainStatesListModel _statesModel;
        public MainStatesListModel States => _statesModel;

        private MainDataKeeper _dataKeeper;
        public MainDataKeeper Data => _dataKeeper;

        [SerializeField]
        private MissionConfigSO[] missionConfigs;
        public IReadOnlyList<MissionConfigSO> MissionConfigs => missionConfigs;

        // Some data

        private void Awake()
        {
            _dataKeeper = new MainDataKeeper();

            // Entry point
            States.OnInit();
        }
    }
}