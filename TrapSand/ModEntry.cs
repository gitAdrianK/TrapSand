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
            var level = Game1.instance.contentManager.level;
            if (level == null
                || level.ID != FactoryTrap.LastUsedMapId)
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            var muteSandUp = false;
            var muteSandDown = false;
            foreach (var tag in level.Info.Tags)
            {
                if (tag == "MuteTrapSandUp")
                {
                    muteSandUp = true;
                }
                if (tag == "MuteTrapSandDown")
                {
                    muteSandDown = true;
                }
                if (muteSandDown && muteSandUp)
                {
                    break;
                }
            }

            // Functions as a ICollisionQuery.
            var levelManager = LevelManager.Instance;

            if (level.ID == FactoryTrap.LastUsedMapIdDown)
            {
                _ = player.m_body.RegisterBlockBehaviour(
                    typeof(BlockTrapDown),
                    new BehaviourTrapDown(levelManager, muteSandDown));
            }

            if (level.ID == FactoryTrap.LastUsedMapIdUp)
            {
                _ = player.m_body.RegisterBlockBehaviour(
                    typeof(BlockTrapUp),
                    new BehaviourTrapUp(levelManager, muteSandUp));
            }
        }
    }
}
