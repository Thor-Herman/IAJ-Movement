using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.Arbitration;

public class MainCharacterController : MonoBehaviour
{
    // Default values
    private float worldSizeX = 55;
    private float worldSizeZ = 32.5f;
    private const float MAX_ACCELERATION = 40.0f;

    public KeyCode stopKey = KeyCode.S;
    public KeyCode priorityKey = KeyCode.P;
    public KeyCode blendedKey = KeyCode.B;

    public DynamicCharacter character;

    public PriorityMovement priorityMovement;
    public BlendedMovement blendedMovement;
    private DynamicPatrol patrolMovement;


    //early initialization
    void Awake()
    {
        this.character = new DynamicCharacter(this.gameObject);

        worldSizeX = ObstacleSceneManager.X_WORLD_SIZE;
        worldSizeZ = ObstacleSceneManager.Z_WORLD_SIZE;


        this.priorityMovement = new PriorityMovement
        {
            Character = this.character.KinematicData
        };

        this.blendedMovement = new BlendedMovement
        {
            Character = this.character.KinematicData
        };
    }


    public void InitializeMovement(GameObject[] obstacles, List<DynamicCharacter> characters)
    {
        foreach (var obstacle in obstacles)
        {
            //TODO: add your AvoidObstacle movement here
            var avoidObstacleMovement = new DynamicAvoidObstacle(obstacle)
            {
                Character = this.character.KinematicData,
                MaxAcceleration = MAX_ACCELERATION,
            };

            // Add it to the Blend and Priority Movement here
            priorityMovement.Movements.Add(avoidObstacleMovement);
            blendedMovement.Movements.Add(new MovementWithWeight(avoidObstacleMovement, 0.5f));
        }

        foreach (var otherCharacter in characters)
        {
            if (otherCharacter != this.character)
            {
                //TODO: add your AvoidCharacter movement here
                var avoidCharacter = new DynamicAvoidCharacter(otherCharacter.KinematicData)
                {
                    Character = this.character.KinematicData,
                };
            }
        }

        // Where should the character move to when patrolling 
        var targetPosition = this.character.KinematicData.Position + (Vector3.zero - this.character.KinematicData.Position) * 2;
        this.patrolMovement = new DynamicPatrol(this.character.KinematicData.Position, targetPosition)
        {
            Character = this.character.KinematicData,
            MaxAcceleration = MAX_ACCELERATION,
        };

        this.priorityMovement.Movements.Add(patrolMovement);
        this.blendedMovement.Movements.Add(new MovementWithWeight(patrolMovement, 1));
        this.character.Movement = this.priorityMovement;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.patrolMovement.ChangeTarget();
        }
        if (Input.GetKeyDown(this.stopKey))
        {
            this.character.Movement = null;
        }
        else if (Input.GetKeyDown(this.blendedKey))
        {
            this.character.Movement = this.blendedMovement;
        }
        else if (Input.GetKeyDown(this.priorityKey))
        {
            this.character.Movement = this.priorityMovement;
        }

        this.UpdateMovingGameObject();
    }

    private void UpdateMovingGameObject()
    {
        if (this.character.Movement != null)
        {
            this.character.Update();

            //Making sure the character stays within World Limits
            this.character.KinematicData.ApplyWorldLimit(this.worldSizeX, this.worldSizeZ);
        }
    }
}
