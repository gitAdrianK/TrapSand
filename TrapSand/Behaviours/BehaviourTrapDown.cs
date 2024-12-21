namespace TrapSand.Behaviours
{
    using System.Collections.Generic;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using TrapSand.Blocks;

    public class BehaviourTrapDown : IBlockBehaviour
    {
        public float BlockPriority => 0.5f;

        public bool IsPlayerOnBlock { get; set; }

        private bool hasEntered = false;
        private bool hasPlayed = false;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (this.IsPlayerOnBlock
                || !info.IsCollidingWith<BlockTrapDown>())
            {
                this.hasEntered = false;
                return false;
            }

            var hitbox = behaviourContext.BodyComp.GetHitbox();
            IReadOnlyCollection<IBlock> blocks = info.GetCollidedBlocks<BlockTrapDown>();
            foreach (var block in blocks)
            {
                var blockRect = block.GetRect();
                var top = blockRect.Bottom - hitbox.Top;
                var bottom = hitbox.Bottom - blockRect.Top;
                var left = blockRect.Right - hitbox.Left;
                var right = hitbox.Right - blockRect.Left;
                if (top < bottom && top < left && top < right)
                {
                    this.hasEntered = false;
                    return true;
                }
            }
            this.hasEntered = true;
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockTrapDown>();
            if (!this.IsPlayerOnBlock)
            {
                this.hasPlayed = false;
                return true;
            }

            if (this.hasEntered)
            {
                if (!this.hasPlayed)
                {
                    Game1.instance?.contentManager?.audio?.player?.SandLand?.Play();
                }
                this.hasPlayed = true;

                var bodyComp = behaviourContext.BodyComp;
                bodyComp.Velocity.X *= 0.25f;
                bodyComp.Velocity.Y = 3.5f;
            }

            return true;
        }
    }
}
