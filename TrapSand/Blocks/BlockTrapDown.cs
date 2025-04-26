namespace TrapSand.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockTrapDown : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_TRAP_DOWN = new Color(255, 68, 68);

        public BlockTrapDown(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_TRAP_DOWN;

        protected override bool canBlockPlayer => false;
    }
}

