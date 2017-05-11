
public class Player
{
    private readonly PlayerInfo _playerInfo;

    #region constructor

    public Player(PlayerInfo playerInfo)
    {
        _playerInfo = playerInfo;
        Popularity = _playerInfo.MaxPopularity;
    }

    #endregion

    #region public properties

    public int Popularity { get; protected set; }

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

