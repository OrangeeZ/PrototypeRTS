
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

    public int SetPopularity(int popularity)
    {
        Popularity = popularity > _playerInfo.MinPopularity
            ? popularity < _playerInfo.MaxPopularity
                ? popularity
                : _playerInfo.MaxPopularity
            : _playerInfo.MinPopularity;
        return Popularity;
    }

    #endregion

}

