
using UnityEngine;

public class Player
{
    private readonly PlayerInfo _playerInfo;

    #region constructor

    public Player(PlayerInfo playerInfo,BaseWorld world)
    {
        _playerInfo = playerInfo;
        Popularity = _playerInfo.MaxPopularity;
        World = world;
    }

    #endregion

    #region public properties

    public int Popularity { get; protected set; }

    public BaseWorld World { get; protected set; }

    #endregion

    #region public methods

    public int ChangePopularity(int popularity)
    {
        var result = Popularity + popularity;
        Popularity = Mathf.Clamp(result, _playerInfo.MinPopularity, _playerInfo.MaxPopularity);
        return Popularity;
    }

    #endregion

}

