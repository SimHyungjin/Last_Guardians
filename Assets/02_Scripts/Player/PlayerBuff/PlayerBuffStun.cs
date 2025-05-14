public class PlayerBuffStun : IPlayerBuff<PlayerStatus>
{
    public float? Value => null;
    public float Duration { get; private set; }

    private PlayerHandler controller;
    private PlayerModelView view;

    public PlayerBuffStun(float duration, PlayerHandler controller)
    {
        Duration = duration;
        this.controller = controller;
        view = controller.playerView;
    }

    public void Apply(PlayerStatus playerData)
    {
        controller.attackController.AutoAttackStop();
        controller.moveController.SetCanMove(false);
        view.OnStun();
    }

    public void Remove(PlayerStatus playerData)
    {
        controller.attackController.AutoAttackStart();
        controller.moveController.SetCanMove(true);
        view.OnStateEnd();
    }

    public bool IsStrongerThan(IPlayerBuff<PlayerStatus> other)
    {
        if (other is not PlayerBuffStun o) return true;
        return this.Duration > o.Duration;
    }
}