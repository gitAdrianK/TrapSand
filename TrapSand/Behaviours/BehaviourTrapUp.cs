namespace TrapSand.Behaviours
{
    using System;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using TrapSand.Blocks;

    public class BehaviourTrapUp : IBlockBehaviour
    {
        private ICollisionQuery CollisionQuery { get; }
        private bool IsMuted { get; }
        private bool HasPlayed { get; set; }

        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        public BehaviourTrapUp(ICollisionQuery collisionQuery, bool isMuted)
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
            if (info.IsCollidingWith<BlockTrapUp>() && !this.IsPlayerOnBlock)
            {
                var bodyComp = behaviourContext.BodyComp;
                var playerPosition = bodyComp.GetHitbox();
                foreach (var block in info.GetCollidedBlocks<BlockTrapUp>())
                {
                    var blockRect = block.GetRect();
                    // I have absolutely NO clue why it is -1/-2/-3,
                    // standing on top is a lot of -2, sometimes -1, landing from a jump can result in -3,
                    // maybe depending on player speed it might be -4 at some point.
                    // Addendum: Yes, depending on fallspeed it can be more than -3.
                    if ((blockRect.Top - playerPosition.Bottom) >= -5)
                    {
                        continue;
                    }
                    return false;
                }
                return bodyComp.Velocity.Y >= 0;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            var hitbox = bodyComp.GetHitbox();
            _ = this.CollisionQuery.CheckCollision(hitbox, out var _, out AdvCollisionInfo info);
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockTrapUp>();
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
            bodyComp.Velocity.Y = -2.0f;
            bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            bodyComp.Position.Y -= 2.5f;

            Camera.UpdateCamera(hitbox.Location);

            return true;
        }
    }
}
