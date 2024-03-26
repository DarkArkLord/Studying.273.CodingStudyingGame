namespace Assets.Scripts.States.Map.Controllers.Interfaces
{
    public interface INpcController : IMapEntityController
    {
        public NpcType Type { get; }
        public void SetInteractive(bool interactive);
        public void OnUpdate();
    }
}