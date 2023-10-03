namespace Assets.Scripts.Interfaces
{
    public interface IBattleEntity : IEntityWithPosition
    {
        void Kill();
        void Resurrect();
    }
}
