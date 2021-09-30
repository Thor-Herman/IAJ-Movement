﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicPatrol : DynamicSeek
    {
        public override string Name
        {
            get
            {
                return "Patrol";
            }
        }

        public KinematicData PatrolPosition1 { get; set; }
        public KinematicData PatrolPosition2 { get; set; }
        public Vector3 TargetPosition { get; protected set; }
        public bool IsTarget1 { get; set; } 

        public DynamicPatrol(Vector3 PatrolPosition1, Vector3 PatrolPosition2)
        {
            this.PatrolPosition1 = new KinematicData { Position = PatrolPosition1 };
            this.PatrolPosition2 = new KinematicData { Position = PatrolPosition2 };
            this.IsTarget1 = true;
        }

        public void ChangeTarget()
        {
            this.IsTarget1 = !this.IsTarget1;
        }

        public override MovementOutput GetMovement()
        {
            if (IsTarget1)
            {
                base.Target = this.PatrolPosition1;
            }
            else
            {
                base.Target = this.PatrolPosition2;
            }
            return base.GetMovement();
        }
    }
}
