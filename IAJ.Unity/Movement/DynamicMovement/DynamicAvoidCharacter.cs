using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidCharacter : DynamicMovement
    {
        public override string Name
        {
            get { return "Avoid Character"; }
        }

        public float MaxTimeLookAhead { get; set; }
        public float AvoidMargin { get; set; }


        public DynamicAvoidCharacter(KinematicData target)
        {
            this.Target = target;
            this.Output = new MovementOutput();
            this.MaxTimeLookAhead = 2.0f;
            this.AvoidMargin = 6.0f;
        }

        public override MovementOutput GetMovement()
        {
            this.Output.Clear();

            // Something's missing here...
            Vector3 deltaPos = Target.Position - Character.Position;
            Vector3 deltaVel = Target.velocity - Character.velocity;
            float deltaSqrSpeed = deltaVel.sqrMagnitude;

            if (deltaSqrSpeed == 0) return new MovementOutput();

            float timeToClosest = -Vector3.Dot(deltaPos, deltaVel) / deltaSqrSpeed;

            if (timeToClosest > MaxTimeLookAhead) return new MovementOutput();

            /* Less efficient version */
            Vector3 closestPointTarget = Target.Position + Target.velocity * timeToClosest;
            Vector3 closestPointCharacter = Character.Position + Character.velocity * timeToClosest;
            Vector3 avoidVelocity = closestPointCharacter - closestPointTarget;

            if (avoidVelocity.magnitude > 2 * AvoidMargin) return new MovementOutput();
            if (avoidVelocity.magnitude <= 0)
                this.Output.linear = Character.Position - Target.Position;
            else
                this.Output.linear = avoidVelocity;

            this.Output.linear = this.Output.linear.normalized * MaxAcceleration;

            return this.Output;

        }
    }
}
