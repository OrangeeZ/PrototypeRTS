using Assets.Scripts.World;

public class WorldEvent : IUpdateBehaviour
{

    protected readonly BaseWorld _gameWorld;

    public WorldEvent(BaseWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public virtual void Update(float deltaTime)
    {
        
    }

}
