using Assets.Scripts.World;

public class WorldEvent : IWorldUpdateBehaviour
{

    protected readonly IWorld _gameWorld;

    public WorldEvent(IWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public virtual void Update(float deltaTime)
    {
        
    }

}
