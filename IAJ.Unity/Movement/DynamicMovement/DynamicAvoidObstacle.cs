﻿using Assets.Scripts.IAJ.Unity.Util;
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
            this.MaxLookAhead = 7.0f;
            this.AvoidMargin = 3.0f;
            this.FanAngle = 0.75f;
        }

        private class CustomRay
        {
            public Ray ray { get; set; }
            public float length { get; set; }
            public Vector3 direction { get; set; }

            public CustomRay(KinematicData Character, float FanAngle, float length, int i)
            {
                direction = MathHelper.Rotate2D(Character.velocity.normalized, FanAngle * i);
                ray = new Ray(Character.Position, direction);
                this.length = length;
            }
        }

        public override MovementOutput GetMovement()
        {
            RaycastHit hit = new RaycastHit();
            bool collision = false;

            CustomRay[] rays = GenerateCustomRays();

            foreach (CustomRay ray in rays)
            {
                collision = ObstacleCollider.Raycast(ray.ray, out hit, ray.length);
                if (collision)
                {
                    Debug.Log(hit.point);
                    Debug.Break();
                    base.Target.Position = hit.point - hit.normal * AvoidMargin;

                    Vector3 debugray = base.Target.Position - this.Character.Position;
                    Debug.DrawRay(this.Character.Position, ray.direction * ray.length, Color.red);
                    Debug.DrawRay(this.Character.Position, debugray, Color.green);
                    Debug.DrawRay(hit.point, hit.normal * AvoidMargin, Color.magenta);

                    return base.GetMovement();
                }
            }

            return new MovementOutput();
        }

        private CustomRay[] GenerateCustomRays()
        {
            CustomRay leftRay = new CustomRay(Character, FanAngle, 3.0f, -1);
            CustomRay middleRay = new CustomRay(Character, FanAngle, MaxLookAhead, 0);
            CustomRay rightRay = new CustomRay(Character, FanAngle, 3.0f, 1);
            CustomRay[] rays = { middleRay, leftRay, rightRay };
            return rays;
        }
    }
}
