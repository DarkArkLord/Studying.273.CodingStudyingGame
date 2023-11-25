using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StatesMachine
{
    public class BaseStatesListModel<T> : BaseModel where T : Enum
    {
        [SerializeField]
        private List<BaseState<T>> _statesList;
        public IEnumerable<BaseState<T>> StatesList => _statesList;

        [SerializeField]
        private BaseState<T> _initialState;

        private StatesController<T> _controller;

        public void OnInit()
        {
            _controller = new StatesController<T>(this);
            if (_initialState != null)
            {
                _controller.PushState(_initialState.Id);
            }
        }

        public BaseState<T> GetState(T stateId) => StatesList.FirstOrDefault(state => state.Id.Equals(stateId));
    }
}
