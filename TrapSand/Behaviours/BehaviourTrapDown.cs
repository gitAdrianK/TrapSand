using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using System;
using TrapSand.Blocks;

namespace TrapSand.Behaviours
{
    public class BehaviourTrapDown : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

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
            if (info.IsCollidingWith<BlockTrapDown>() && !IsPlayerOnBlock)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0.0f;
            }

            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            BodyComp bodyComp = behaviourContext.BodyComp;

            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockTrapDown>();

            if (!IsPlayerOnBlock)
            {
                return true;
            }

            bodyComp.Velocity.X *= 0.25f;
            bodyComp.Velocity.Y = 4.5f;
            bodyComp.Velocity.Y = Math.Max(0.75f, bodyComp.Velocity.Y);
            Camera.UpdateCamera(new Point(bodyComp.GetHitbox().Left, bodyComp.GetHitbox().Bottom));

            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}
