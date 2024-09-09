using EntityComponent;
using JumpKing.Level;
using JumpKing.Mods;
using JumpKing.Player;
using TrapSand.Behaviours;
using TrapSand.Blocks;
using TrapSand.Factories;

namespace TrapSand
{
    [JumpKingMod("Zebra.TrapSand")]
    public static class ModEntry
    {
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            LevelManager.RegisterBlockFactory(new FactoryTrap());
        }

        [OnLevelStart]
        public static void OnLevelStart()
        {
            EntityManager entityManager = EntityManager.instance;
            PlayerEntity player = entityManager.Find<PlayerEntity>();

            if (player == null)
            {
                return;
            }

            BehaviourTrapDown behaviourTrapDown = new BehaviourTrapDown();
            player.m_body.RegisterBlockBehaviour(typeof(BlockTrapDown), behaviourTrapDown);

            BehaviourTrapUp behaviourTrapUp = new BehaviourTrapUp();
            player.m_body.RegisterBlockBehaviour(typeof(BlockTrapUp), behaviourTrapUp);
        }
    }
}
