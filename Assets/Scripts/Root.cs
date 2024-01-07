using Assets.Scripts.StatesMachine;
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

        // Some data

        private void Awake()
        {
            // Entry point
            States.OnInit();
        }
    }
}