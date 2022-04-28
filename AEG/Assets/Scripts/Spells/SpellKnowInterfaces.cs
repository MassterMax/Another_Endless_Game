public interface IKnowMonsterController
{
    public MonsterController MonsterController { get; set; }

    public void SetMonsterController(MonsterController monsterController)
    {
        MonsterController = monsterController;
    }
}

public interface IKnowSpellManager
{
    public SpellManager SpellManager { get; set; }

    public void SetSpellManager(SpellManager spellManager)
    {
        SpellManager = spellManager;
    }
}

public interface IKnowPlayerController
{
    public PlayerController PlayerController { get; set; }

    public void SetPlayerController(PlayerController playerController)
    {
        PlayerController = playerController;
    }
}