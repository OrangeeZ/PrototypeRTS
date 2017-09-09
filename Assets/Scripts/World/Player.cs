public class Player
{

    #region constructor

    public Player(BaseWorld world)
    {
        World = world;
    }

    #endregion

    #region public properties

    public BaseWorld World { get; protected set; }

    #endregion
    

}

