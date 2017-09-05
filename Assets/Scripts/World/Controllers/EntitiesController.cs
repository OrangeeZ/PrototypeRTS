using Actors;

public class EntitiesController : WorldBehaviourController<Entity>
{
    private readonly EntityMapping _entityMapping;

    public EntitiesController(EntityMapping entityMapping) : base()
    {
        _entityMapping = entityMapping;
    }

    public override void Add(Entity behaviour)
    {
        base.Add(behaviour);

        if (behaviour is Actor)
        {
            _entityMapping.AddActor(behaviour as Actor);
        }
    }

    public override void Remove(Entity behaviour)
    {
        base.Remove(behaviour);

        if (behaviour is Actor)
        {
            _entityMapping.RemoveActor(behaviour as Actor);
        }
    }
}