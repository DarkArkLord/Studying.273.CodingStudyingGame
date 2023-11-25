using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StatesMachine
{
    public class StatesController<T> where T : Enum
    {
        private BaseStatesListModel<T> model;

        public StatesController(BaseStatesListModel<T> model)
        {
            this.model = model;
        }

        public Stack<BaseState<T>> UsingStates { get; private set; } = new Stack<BaseState<T>>();
        public BaseState<T> CurrentState { get; private set; } = null;

        public void UseState(T stateId)
        {
            if (CurrentState != null)
            {
                CurrentState.OnStateDestroy();
            }

            CurrentState = model.GetState(stateId);
            CurrentState.SetController(this);
            CurrentState.OnStateCreating();
        }

        public void PushState(T stateId)
        {
            if (CurrentState != null)
            {
                CurrentState.OnStatePush();
                UsingStates.Push(CurrentState);
            }

            CurrentState = model.GetState(stateId);
            CurrentState.SetController(this);
            CurrentState.OnStateCreating();
        }

        public void PopState()
        {
            if (UsingStates.Count < 1)
            {
                throw new InvalidOperationException("Попытка достать состояние из пустого стека состояний");
            }

            var oldState = CurrentState;
            var newState = CurrentState = UsingStates.Pop();
            newState.SetController(this);

            oldState.OnStateDestroy();
            newState.OnStatePop();
        }

        public void OnUpdate()
        {
            CurrentState?.OnUpdate();
        }
    }
}
