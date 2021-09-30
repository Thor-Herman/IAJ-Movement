using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidObstacle : DynamicSeek
    {
        public override string Name
        {
            get { return "Avoid Obstacle"; }
        }

        private GameObject obstacle;

        public GameObject Obstacle
        {
            get { return this.obstacle; }
            set
            {
                this.obstacle = value;
                this.ObstacleCollider = value.GetComponent<Collider>();
            }
        }

        private Collider ObstacleCollider { get; set; }
        public float MaxLookAhead { get; set; }

        public float AvoidMargin { get; set; }

        public float FanAngle { get; set; }

        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.Obstacle = obstacle;
            this.Target = new KinematicData();

            // Don't forget to initialize the variables, you can do it here or in the MainCharacterController
            this.MaxLookAhead = 5.0f;
            this.AvoidMargin = 1.0f;
        }

        public override MovementOutput GetMovement()
        {
            RaycastHit hit;
            bool collision = false;

            // Now, how does Unity deals with colisions? Each obstacle has a collider...
            Vector3 direction = MathHelper.ConvertOrientationToVector(this.Character.Orientation);

            for (int i = -1; i < 2; i++)
            {
                Ray ray = new Ray(this.Character.Position, this.Character.velocity);
                // Debug.DrawRay(this.Character.Position, this.Character.velocity, Color.red);
                collision = ObstacleCollider.Raycast(ray, out hit, MaxLookAhead);

                if (collision)
                {
                    Debug.Break();
                    //base.Target.Position = hit.point + hit.normal * AvoidMargin;
                    // Debug.DrawRay(this.Character.Position, hit.point + hit.normal * AvoidMargin, Color.green);
                    //ebug.DrawRay(hit.point, hit.point + hit.normal * AvoidMargin, Color.magenta);
                    return base.GetMovement();
                }
            }

            return new MovementOutput();
        }
    }
}
