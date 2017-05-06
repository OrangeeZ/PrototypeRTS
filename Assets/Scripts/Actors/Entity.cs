using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    public Vector3 Position { get; protected set; }

    public TestWorld World { get; private set; }

    protected ActorView ActorView;

    public abstract void Update(float deltaTime);

    public Entity(TestWorld world)
    {
        World = world;
    }

    public void SetView(ActorView actorView)
    {
        ActorView = actorView;
    }

    public virtual void SetPosition(Vector3 position)
    {
        Position = position;
        ActorView.transform.position = position;
    }
}
