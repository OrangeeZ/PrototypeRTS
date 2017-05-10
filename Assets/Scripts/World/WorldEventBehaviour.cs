using Assets.Scripts.World;

public class WorldEventBehaviour {

    protected readonly GameWorld _gameWorld;

    public WorldEventBehaviour(GameWorld gameWorld)
    {
        _gameWorld = gameWorld;
    }

    public virtual void Update(float deltaTime)
    {
        
    }

}
