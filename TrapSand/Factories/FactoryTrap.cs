namespace TrapSand.Factories
{
    using System;
    using System.Collections.Generic;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using TrapSand.Blocks;

    public class FactoryTrap : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;
        public static ulong LastUsedMapIdUp { get; private set; } = ulong.MaxValue;
        public static ulong LastUsedMapIdDown { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
            BlockTrapDown.BLOCKCODE_TRAP_DOWN,
            BlockTrapUp.BLOCKCODE_TRAP_UP,
        };

        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == BlockTrapDown.BLOCKCODE_TRAP_DOWN:
                case var _ when blockCode == BlockTrapUp.BLOCKCODE_TRAP_UP:
                    return true;
                default:
                    break;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == BlockTrapDown.BLOCKCODE_TRAP_DOWN:
                    LastUsedMapIdDown = level.ID;
                    return new BlockTrapDown(blockRect);
                case var _ when blockCode == BlockTrapUp.BLOCKCODE_TRAP_UP:
                    LastUsedMapIdUp = level.ID;
                    return new BlockTrapUp(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactoryTrap)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
