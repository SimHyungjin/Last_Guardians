using UnityEngine;

public class CharacterManager
{
    public Character character;
    public CharacterController characterController;
    public CharacterHandler characterHandler;

    public Player player;

    public void Init()
    {
        if (player == null)
            player = new(10,10,1,10,10,1,10,10,10,10,10);

        character = Utils.InstantiateResource("Character").GetComponent<Character>();
        characterController = character.gameObject.GetComponent<CharacterController>();
        characterHandler = character.gameObject.GetComponent<CharacterHandler>();

        character.Init(player);
        characterController.Init(character);
        characterHandler.Init(characterController);
    }
}
