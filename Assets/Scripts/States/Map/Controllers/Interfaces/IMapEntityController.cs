namespace Assets.Scripts.States.Map.Controllers.Interfaces
{
    public interface IMapEntityController : IObjectWithPosition2D
    {
        public bool IsAlive { get; }
        public bool IsInteractive { get; }

        public void Kill();
        public void Resurrect();

        public void SetPause(bool pause);
    }
}
