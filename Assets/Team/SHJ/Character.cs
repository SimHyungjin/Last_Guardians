using UnityEngine;

public class Character : MonoBehaviour
{
    public Player player {  get; private set; }

    public void Init(Player _player)
    {
        player = _player;
    }
    public void GetExp(float exp)
    {
        player.exp += exp;
    }
}
