namespace TrapSand.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockTrapUp : BoxBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_TRAP_UP = new Color(255, 69, 69);

        public BlockTrapUp(Rectangle collider) : base(collider) { }

        public Color DebugColor => BLOCKCODE_TRAP_UP;

        protected override bool canBlockPlayer => false;
    }
}

