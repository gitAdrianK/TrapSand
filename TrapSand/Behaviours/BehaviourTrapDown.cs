using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TrapSand.Blocks;

namespace TrapSand.Behaviours
{
    public class BehaviourTrapDown : IBlockBehaviour
    {
        public float BlockPriority => 0.5f;

        public bool IsPlayerOnBlock { get; set; }

        private bool hasEntered = false;
        private bool hasPlayed = false;

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
            if (IsPlayerOnBlock
                || !info.IsCollidingWith<BlockTrapDown>())
            {
                hasEntered = false;
                return false;
            }

            Rectangle hitbox = behaviourContext.BodyComp.GetHitbox();
            IReadOnlyCollection<IBlock> blocks = info.GetCollidedBlocks<BlockTrapDown>();
            foreach (IBlock block in blocks)
            {
                Rectangle blockRect = block.GetRect();
                int top = blockRect.Bottom - hitbox.Top;
                int bottom = hitbox.Bottom - blockRect.Top;
                int left = blockRect.Right - hitbox.Left;
                int right = hitbox.Right - blockRect.Left;
                if (top < bottom && top < left && top < right)
                {
                    hasEntered = false;
                    return true;
                }
            }
            hasEntered = true;
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockTrapDown>();
            if (!IsPlayerOnBlock)
            {
                hasPlayed = false;
                return true;
            }

            if (hasEntered)
            {
                if (!hasPlayed)
                {
                    Game1.instance?.contentManager?.audio?.player?.SandLand?.Play();
                }
                hasPlayed = true;

                BodyComp bodyComp = behaviourContext.BodyComp;
                bodyComp.Velocity.X *= 0.25f;
                bodyComp.Velocity.Y = 3.5f;
            }

            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}
