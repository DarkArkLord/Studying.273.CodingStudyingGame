using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StatesMachine
{
    public abstract class BaseState<T> : BaseModel where T : Enum
    {
        public abstract T Id { get; }

        protected StatesController<T> controller { get; private set; }

        public void SetController(StatesController<T> controller)
        {
            this.controller = controller;
        }

        // TODO: Сделать ивент активации/деактивации вместо флага?
        protected bool IsActive { get; private set; } = false;

        public virtual IEnumerator OnStateCreating()
        {
            // Вызывается перед push
            Debug.Log($"Create {Id} State");
            IsActive = true;
            yield return null;
        }

        public virtual IEnumerator OnStatePop()
        {
            // Вызывается после pop, если этот стейт станет current
            Debug.Log($"Pop {Id} State");
            IsActive = true;
            yield return null;
        }

        public virtual IEnumerator OnStatePush()
        {
            // Вызывается перед push, если этот стейт current
            Debug.Log($"Push {Id} State");
            IsActive = false;
            yield return null;
        }

        public virtual IEnumerator OnStateDestroy()
        {
            // Вызывается после pop, если этот стейт current
            Debug.Log($"Destroy {Id} State");
            IsActive = false;
            yield return null;
        }

        public abstract void OnUpdate();
    }
}