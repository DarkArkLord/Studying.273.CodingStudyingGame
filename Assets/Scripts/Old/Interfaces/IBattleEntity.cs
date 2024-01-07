namespace Assets.Scripts.Interfaces
{
    public interface IBattleEntity : IEntityWithPosition
    {
        bool IsAlive { get; }
        void Kill();
        void Resurrect();
    }
}
