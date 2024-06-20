using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace TrapSand.Blocks
{
    public class BlockTrap : IBlock, IBlockDebugColor
    {
        public static readonly Color BLOCKCODE_TRAP = new Color(255, 69, 69);

        private readonly Rectangle collider;

        public BlockTrap(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return BLOCKCODE_TRAP; }
        }

        public Rectangle GetRect()
        {
            return collider;
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;

        }
    }
}

