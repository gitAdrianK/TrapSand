namespace TrapSand
{
    using EntityComponent;
    using JumpKing;
    using JumpKing.Level;
    using JumpKing.Mods;
    using JumpKing.Player;
    using TrapSand.Behaviours;
    using TrapSand.Blocks;
    using TrapSand.Factories;

    [JumpKingMod("Zebra.TrapSand")]
    public static class ModEntry
    {
        [BeforeLevelLoad]
        public static void BeforeLevelLoad() => LevelManager.RegisterBlockFactory(new FactoryTrap());

        [OnLevelStart]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            if (contentManager.level == null)
            {
                return;
            }
            if (contentManager.level.ID != FactoryTrap.LastUsedMapId)
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockTrapDown), new BehaviourTrapDown());
            _ = player.m_body.RegisterBlockBehaviour(typeof(BlockTrapUp), new BehaviourTrapUp());
        }
    }
}
