using System;
using System.Collections;
using System.Collections.Generic;
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
            model.StartCoroutine(UseStateCoroutine(stateId));
        }

        public void PushState(T stateId)
        {
            model.StartCoroutine(PushStateCoroutine(stateId));
        }

        public void PopState()
        {
            model.StartCoroutine(PopStateCoroutine());
        }

        public void ClearStatesStack()
        {
            while(UsingStates.Count > 0)
            {
                var state = UsingStates.Pop();
                model.StartCoroutine(state.OnStateDestroy());
            }
        }

        public void OnUpdate()
        {
            CurrentState?.OnUpdate();
        }

        private IEnumerator UseStateCoroutine(T stateId)
        {
            var newState = model.GetState(stateId);
            if (!newState)
            {
                Debug.LogError($"No state {stateId}");
                yield break;
            }

            if (CurrentState)
            {
                yield return CurrentState.OnStateDestroy();
            }

            CurrentState = newState;
            newState.SetController(this);

            yield return newState.OnStateCreating();
        }

        private IEnumerator PushStateCoroutine(T stateId)
        {
            var newState = model.GetState(stateId);
            if (!newState)
            {
                Debug.LogError($"No state {stateId}");
                yield break;
            }

            if (CurrentState)
            {
                yield return CurrentState.OnStatePush();
                UsingStates.Push(CurrentState);
            }

            CurrentState = newState;
            newState.SetController(this);

            yield return newState.OnStateCreating();
        }

        private IEnumerator PopStateCoroutine()
        {
            if (UsingStates.Count < 1)
            {
                throw new InvalidOperationException("Попытка достать состояние из пустого стека состояний");
            }

            var oldState = CurrentState;
            var newState = CurrentState = UsingStates.Pop();
            newState.SetController(this);

            yield return oldState.OnStateDestroy();
            yield return newState.OnStatePop();
        }
    }
}
