public class CharacterManager
{
    public Character character;
    public CharacterController characterController;
    public CharacterInputHandler characterHandler;

    public Player player;

    public void Init()
    {
        if (player == null)
            player = new(10,10,1,10,10,1,10,10,10,10,10);
        if (character == null)
            character = new();

        characterController = Utils.InstantiateResource("Character").GetComponent<CharacterController>();
        characterHandler = characterController.GetComponent<CharacterInputHandler>();

        character.Init(player);
        characterController.Init(character);
        characterHandler.Init();
    }
}
