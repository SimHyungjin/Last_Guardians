public class Player
{
    public PlayerData playerData {  get; private set; }

    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
    }
}
