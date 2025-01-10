namespace TrapSand.Behaviours
{
    using System;
    using System.Linq;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using TrapSand.Blocks;

    public class BehaviourTrapUp : IBlockBehaviour
    {
        private enum Direction
        {
            None,
            Top,
            Other,
        }

        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private bool hasEntered;
        private Rectangle prevPosition = Rectangle.Empty;
        private Direction direction = Direction.None;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockTrapUp>()
                && !this.hasEntered
                && this.direction == Direction.Top
                && behaviourContext.BodyComp.Velocity.Y > 0.0f)
            {
                return true;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                this.direction = Direction.None;
                this.prevPosition = Rectangle.Empty;
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var bodyComp = behaviourContext.BodyComp;

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockTrapUp>();

            if (!this.IsPlayerOnBlock)
            {
                this.direction = Direction.None;
                this.prevPosition = bodyComp.GetHitbox();
                return true;
            }

            if (this.direction == Direction.Top)
            {
                this.prevPosition = bodyComp.GetHitbox();
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b => b.GetType() == typeof(BlockTrapUp));
            var playerRect = this.prevPosition;

            foreach (var block in blocks)
            {
                var blockRect = block.GetRect();

                var bottomDiff = blockRect.Bottom - playerRect.Top;
                var topDiff = playerRect.Bottom - blockRect.Top;

                if (topDiff < bottomDiff)
                {
                    this.direction = Direction.Top;
                    this.hasEntered = false;
                }
                else
                {
                    if (!this.hasEntered)
                    {
                        Game1.instance?.contentManager?.audio?.player?.SandLand?.Play();
                    }
                    this.direction = Direction.Other;
                    this.hasEntered = true;
                    break;
                }
            }

            if (this.hasEntered)
            {
                bodyComp.Velocity.X *= 0.25f;
                bodyComp.Velocity.Y = -2.0f;
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
                bodyComp.Position.Y -= 2.5f;
                Camera.UpdateCamera(new Point(bodyComp.GetHitbox().Left, bodyComp.GetHitbox().Top));
            }
            this.prevPosition = bodyComp.GetHitbox();
            return true;
        }
    }
}
