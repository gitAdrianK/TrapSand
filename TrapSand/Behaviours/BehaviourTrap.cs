using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using TrapSand.Blocks;

namespace TrapSand.Behaviours
{
    public class BehaviourTrap : IBlockBehaviour
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

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            return inputXVelocity;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            return inputYVelocity;
        }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockTrap>() && !hasEntered)
            {
                if (direction == Direction.Top && behaviourContext.BodyComp.Velocity.Y > 0.0f)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                direction = Direction.None;
                prevPosition = Rectangle.Empty;
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            BodyComp bodyComp = behaviourContext.BodyComp;

            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockTrap>();

            if (!IsPlayerOnBlock)
            {
                direction = Direction.None;
                prevPosition = bodyComp.GetHitbox();
                return true;
            }

            if (direction == Direction.Top)
            {
                prevPosition = bodyComp.GetHitbox();
                return true;
            }

            IBlock block = advCollisionInfo.GetCollidedBlocks().ToList().Find(b => b.GetType() == typeof(BlockTrap));
            Rectangle playerRect = prevPosition;
            Rectangle blockRect = block.GetRect();

            float bottomDiff = blockRect.Bottom - playerRect.Top;
            float topDiff = playerRect.Bottom - blockRect.Top;
            float leftDiff = playerRect.Right - blockRect.Left;
            float rightDiff = blockRect.Right - playerRect.Left;

            if (topDiff < bottomDiff && topDiff < leftDiff && topDiff < rightDiff)
            {
                direction = Direction.Top;
                hasEntered = false;
            }
            else
            {
                if (!hasEntered)
                {
                    Game1.instance?.contentManager?.audio?.player?.SandLand?.Play();
                }
                direction = Direction.Other;
                hasEntered = true;
            }

            if (hasEntered)
            {
                bodyComp.Velocity.X *= 0.25f;
                bodyComp.Velocity.Y = -2.0f;
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
                bodyComp.Position.Y -= 2.5f;
                Camera.UpdateCamera(bodyComp.GetHitbox().Center);
            }
            prevPosition = bodyComp.GetHitbox();
            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}
