using Actors;

public class EntitiesController : WorldBehaviourController<Entity>
{
    private readonly EntityMapping _entityMapping;

    public EntitiesController(EntityMapping entityMapping)
    {
        _entityMapping = entityMapping;
    }

    public override void Add(Entity behaviour)
    {
        base.Add(behaviour);

        _entityMapping.AddEntity(behaviour);
    }

    public override void Remove(Entity behaviour)
    {
        base.Remove(behaviour);

        _entityMapping.RemoveEntity(behaviour);
    }
}