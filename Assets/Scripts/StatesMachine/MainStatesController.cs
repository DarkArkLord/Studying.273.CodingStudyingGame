using System;
using System.Collections.Generic;

namespace Assets.Scripts.StatesMachine
{
    public class MainStatesController
    {
        public Stack<BaseState> States { get; private set; } = new Stack<BaseState>();
        public BaseState CurrentState { get; private set; } = null;

        public void PushState(BaseState newState)
        {
            var oldState = CurrentState;

            if (oldState != null)
            {
                oldState.OnStatePush();
                States.Push(oldState);
            }

            CurrentState = newState;
            newState.OnStateCreating();
        }

        public void PopState()
        {
            if (States.Count < 1)
            {
                throw new InvalidOperationException("Попытка достать состояние из пустого стека состояний");
            }

            var oldState = CurrentState;
            var newState = CurrentState = States.Pop();

            oldState.OnStateDestroy();
            newState.OnStatePop();
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}
