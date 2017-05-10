using csv;
using UnityEngine;

public class PlayerInfo : ScriptableObject, ICsvConfigurable
{
    public int MaxPopularity = 100;
    public int MinPopularity = 0;

    public void Configure(Values values)
    {
    }

}
