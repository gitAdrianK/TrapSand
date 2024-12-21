namespace TrapSand.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockTrapDown : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_TRAP_DOWN = new Color(255, 68, 68);

        private readonly Rectangle collider;

        public BlockTrapDown(Rectangle collider) => this.collider = collider;

        public Color DebugColor => BLOCKCODE_TRAP_DOWN;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}

