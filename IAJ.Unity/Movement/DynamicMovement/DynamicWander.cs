using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

    public class DynamicWander : DynamicSeek
    {
        public float WanderOffset { get; set; }
        public float WanderRadius { get; set; }
        public float WanderRate { get; set; }
        public float TurnAngle { get; set; }

        public Vector3 CircleCenter { get; private set; }

        public GameObject DebugTarget { get; set; }

        protected float WanderOrientation { get; set; }

        public DynamicWander()
        {
            this.Target = new KinematicData();
            WanderOrientation = 0;
        }

        public override string Name
        {
            get { return "Wander"; }
        }

        public override MovementOutput GetMovement()
        {           
            if(this.DebugTarget != null)
            {
                this.DebugTarget.transform.position = this.Target.Position;
            }
            WanderOrientation += RandomHelper.RandomBinomial() * WanderRate;
            float targetOrientation = WanderOrientation + Character.Orientation;
            CircleCenter = Character.Position + WanderOffset * MathHelper.ConvertOrientationToVector(Character.Orientation);
            base.Target.Position = CircleCenter + WanderRadius * MathHelper.ConvertOrientationToVector(targetOrientation);
            return base.GetMovement();
        }
    }
}
