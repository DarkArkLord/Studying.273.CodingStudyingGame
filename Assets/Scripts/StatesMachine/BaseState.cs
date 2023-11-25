using System;
using UnityEngine;

namespace Assets.Scripts.StatesMachine
{
    public abstract class BaseState<T> : BaseModel where T : Enum
    {
        [SerializeField]
        private T _id;
        public T Id => _id;

        protected StatesController<T> controller { get; private set; }

        public void SetController(StatesController<T> controller)
        {
            this.controller = controller;
        }

        public virtual void OnStateCreating()
        {
            // Вызывается перед push
        }

        public virtual void OnStatePop()
        {
            // Вызывается после pop, если этот стейт станет current
        }

        public virtual void OnStatePush()
        {
            // Вызывается перед push, если этот стейт current
        }

        public virtual void OnStateDestroy()
        {
            // Вызывается после pop, если этот стейт current
        }

        public abstract void OnUpdate();
    }
}