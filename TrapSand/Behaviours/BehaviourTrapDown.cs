namespace TrapSand.Behaviours
{
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using TrapSand.Blocks;

    public class BehaviourTrapDown : IBlockBehaviour
    {
        private ICollisionQuery CollisionQuery { get; }
        private bool IsMuted { get; }
        private bool HasPlayed { get; set; }

        public float BlockPriority => 0.5f;

        public bool IsPlayerOnBlock { get; set; }

        public BehaviourTrapDown(ICollisionQuery collisionQuery, bool isMuted)
        {
            this.CollisionQuery = collisionQuery;
            this.IsMuted = isMuted;
        }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockTrapDown>() && !this.IsPlayerOnBlock)
            {
                var bodyComp = behaviourContext.BodyComp;
                var playerPosition = bodyComp.GetHitbox();
                foreach (var block in info.GetCollidedBlocks<BlockTrapDown>())
                {
                    var blockRect = block.GetRect();
                    // See the other behaviour for why magic number.
                    if ((playerPosition.Top - blockRect.Bottom) >= -3)
                    {
                        continue;
                    }
                    return false;
                }
                return bodyComp.Velocity.Y < 0;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            var hitbox = bodyComp.GetHitbox();
            _ = this.CollisionQuery.CheckCollision(hitbox, out var _, out AdvCollisionInfo info);
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockTrapDown>();
            if (!this.IsPlayerOnBlock)
            {
                this.HasPlayed = false;
                return true;
            }

            if (!this.IsMuted && !this.HasPlayed)
            {
                Game1.instance?.contentManager?.audio?.player?.SandLand?.Play();
                this.HasPlayed = true;
            }

            bodyComp.Velocity.X *= 0.25f;
            bodyComp.Velocity.Y = 3.5f;

            Camera.UpdateCamera(hitbox.Location);

            return true;
        }
    }
}
