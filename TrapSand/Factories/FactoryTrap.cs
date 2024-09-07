using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TrapSand.Blocks;

namespace TrapSand.Factories
{
    public class FactoryTrap : IBlockFactory
    {
        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            BlockTrapDown.BLOCKCODE_TRAP_DOWN,
            BlockTrapUp.BLOCKCODE_TRAP_UP,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == BlockTrapDown.BLOCKCODE_TRAP_DOWN:
                case var _ when blockCode == BlockTrapUp.BLOCKCODE_TRAP_UP:
                    return true;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            switch (blockCode)
            {
                case var _ when blockCode == BlockTrapDown.BLOCKCODE_TRAP_DOWN:
                    return new BlockTrapDown(blockRect);
                case var _ when blockCode == BlockTrapUp.BLOCKCODE_TRAP_UP:
                    return new BlockTrapUp(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryTrap).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
