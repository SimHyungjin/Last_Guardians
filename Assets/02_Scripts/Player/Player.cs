public class Player
{
    public PlayerData playerData {  get; private set; }

    public void Init()
    {
        if(playerData == null) playerData = new();
    }
}
