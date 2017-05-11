using Assets.Scripts.World;

public class WorldEventBehaviour : IWorldUpdateBehaviour
{

    protected readonly IWorld _gameWorld;

    public WorldEventBehaviour(IWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public virtual void Update(float deltaTime)
    {
        
    }

}
