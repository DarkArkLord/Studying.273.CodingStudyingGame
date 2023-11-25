namespace Assets.Scripts.StatesMachine
{
    public abstract class BaseState
    {
        protected MainStatesController MainController { get; private set; }

        public void SetMainController(MainStatesController mainController)
        {
            MainController = mainController;
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

        public abstract void Update();
    }
}
