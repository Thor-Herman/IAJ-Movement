using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicArrive : DynamicVelocityMatch
    {

        public KinematicData ArriveTarget { get; set; }
        public GameObject DebugTarget { get; set; }
        public float StopRadius { get; set; }
        public float SlowRadius { get; set; }
        public float MaxSpeed { get; set; }
        protected float DesiredSpeed { get; set; }

        public DynamicArrive()
        {
            this.Target = new KinematicData();
        }

        public override string Name
        {
            get { return "Arrive"; }
        }

        public override MovementOutput GetMovement()
        {
            Vector3 direction = this.ArriveTarget.Position - this.Character.Position;
            float distance = direction.magnitude;

            if (distance < this.StopRadius) DesiredSpeed = 0;
            else if (distance > this.SlowRadius) DesiredSpeed = MaxSpeed;
            else DesiredSpeed = MaxSpeed * (distance / SlowRadius);

            base.Target.velocity = direction.normalized * DesiredSpeed;
            if (this.DebugTarget != null)
            {
                this.Target.Position = base.Target.velocity;
                this.DebugTarget.transform.position = base.Target.velocity;
            }
 
            return base.GetMovement();
        }

    }
}
