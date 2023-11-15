namespace TrashDash.Scripts.Common.Interfaces
{
    public interface IObstacleMoveable
    {
        public void SetMoveable(bool moveable);
    }

    public interface ICreatureObstacle
    {
        public void Death();
    }
}
