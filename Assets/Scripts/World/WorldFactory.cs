using Assets.Scripts.World;
using UnityEngine;

public class WorldFactory : IFactory<TestWorldData>
{

    public TestWorldData Create()
    {
        //todo initialize simualtion gmae world state machine
        //use test game world from scene
        return Object.FindObjectOfType<TestWorldData>();
    }

}
