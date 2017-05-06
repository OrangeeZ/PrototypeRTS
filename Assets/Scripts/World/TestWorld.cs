using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestWorld : MonoBehaviour
{
    private List<Entity> _entities = new List<Entity>();
	
    private Queue<Actor> _freeCitizens = new Queue<Actor>();

    [SerializeField]
    private List<Stockpile> _stockpiles = new List<Stockpile>();

    [SerializeField]
    private Transform _fireplace;

    [SerializeField]
    private ActorView _testActorView;

    [SerializeField]
    private ActorView _testBuildingView;

    public void AddCitizen()
    {
        var citizen = new Actor(this);

        citizen.SetView(Instantiate(_testActorView));
        citizen.SetBehaviour(new CitizenBehaviour());
        citizen.SetPosition(_fireplace.position);

        _entities.Add(citizen);
        _freeCitizens.Enqueue(citizen);
    }

    public void AddBuilding()
    {
        var building = new Workplace(this);

        building.SetView(Instantiate(_testBuildingView));

        var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
        randomPosition.y = 0;

        building.SetPosition(_fireplace.position + randomPosition);

        _entities.Add(building);
    }

    public void GetClosestStockpileWithResource(ResourceType resourceType)
    {

    }

    public Stockpile GetClosestStockpile(Vector3 position)
    {
        return _stockpiles.First();
    }

    public Actor GetFreeCitizen()
    {
        if (_freeCitizens.Any())
        {
            return _freeCitizens.Dequeue();
        }

        return null;
    }
    
    public void UpdateStep(float deltaTime)
    {
        foreach (var each in _entities)
        {
            each.Update(deltaTime);
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Add citizen"))
        {
            AddCitizen();
        }

        if (GUILayout.Button("Add building"))
        {
            AddBuilding();
        }
    }
}
