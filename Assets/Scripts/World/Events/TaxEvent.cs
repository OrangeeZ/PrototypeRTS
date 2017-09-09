public class TaxEvent : WorldEvent
{
    private readonly Player _player;
    private float _timeLeft;

    public TaxEvent(BaseWorld world,Player player, float updatePeriod) :
        base(world)
    {
        _player = player;
        Period = updatePeriod;
    }

    #region public properties

    public float Period { get; set; }

    #endregion

    #region public methods

    public override void Update(float deltaTime)
    {
        _timeLeft += deltaTime;
        if (_timeLeft < Period)
            return;
        _timeLeft = 0;
        ApplyDebt();
    }

    #endregion

    #region

    private void ApplyDebt()
    {
        var income = _world.MaxPopulation * _world.Tax;
        _player.World.ChangePopularity(-_world.Tax);
        _world.SetGold(_world.Gold + income);
    }

    #endregion
}
