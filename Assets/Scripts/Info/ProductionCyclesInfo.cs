using csv;
using UnityEngine;

public class ProductionCyclesInfo : ScriptableObject, ICsvConfigurable
{
    [RemoteProperty]
    public string Id;

    [RemoteProperty]
    public string Name;

    [RemoteProperty]
    public ResourceInfo InputResource;

    [RemoteProperty]
    public int InputResourceQuantity;

    [RemoteProperty]
    public int ProductionDuration;

    [RemoteProperty]
    public ResourceInfo OutputResource;

    [RemoteProperty]
    public int OutputResourceQuantity;
<<<<<<< f8148aa54878b436da711513ce755c353fa6977b

=======
    
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286
    public void Configure(Values values)
    {
    }
}
