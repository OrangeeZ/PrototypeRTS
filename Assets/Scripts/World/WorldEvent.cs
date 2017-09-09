public class WorldEvent : IUpdateBehaviour
{

    protected readonly BaseWorld _world;

    public WorldEvent(BaseWorld world)
    {
        _world = world;
    }

    public virtual void Update(float deltaTime)
    {

    }

}
