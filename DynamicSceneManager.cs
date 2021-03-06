using Assets.Scripts.IAJ.Unity.Movement;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;
using UnityEngine.UI;

public class DynamicSceneManager : MonoBehaviour
{

    [Header("World Size Settings")]
    public int X_WORLD_SIZE = 55;
    public int Z_WORLD_SIZE = 32;

    [Header("Wander Settings")]
    public float WanderOffset = 3.0f;
    public float WanderRadius = 3.0f, WanderRate = 1.0f;

    [Header("Arrive Settings")]
    public float StopRadius = 1.0f;
    public float SlowRadius = 5.0f;

    private const float MAX_ACCELERATION = 20.0f;
    private const float MAX_SPEED = 20.0f;
    private const float DRAG = 0.9f;

    public DynamicCharacter BlueCharacter { get; set; }
    public DynamicCharacter GreenCharacter { get; set; }

    private Text BlueMovementText { get; set; }
    private Text GreenMovementText { get; set; }

    private DynamicWander BlueDynamicWander { get; set; }
    private DynamicWander GreenDynamicWander { get; set; }
    private DynamicSeek BlueDynamicSeek { get; set; }
    private DynamicSeek GreenDynamicSeek { get; set; }
    private DynamicFlee BlueDynamicFlee { get; set; }
    private DynamicFlee GreenDynamicFlee { get; set; }
    private DynamicArrive BlueDynamicArrive { get; set; }
    private DynamicArrive GreenDynamicArrive { get; set; }


    [Header("Debug objects")]
    public GameObject blueDebugTarget;
    public GameObject greenDebugTarget;


    // Use this for initialization
    void Start()
    {
        var textObj = GameObject.Find("InstructionsText");
        if (textObj != null)
        {
            textObj.GetComponent<Text>().text =
                "Instructions\n\n" +
                "Blue Character\n" +
                "Q - Stationary\n" +
                "W - Seek\n" +
                "E - Flee\n" +
                "R - Arrive\n" +
                "T - Wander\n\n" +
                "Green Character\n" +
                "A - Stationary\n" +
                "S - Seek\n" +
                "D - Flee\n" +
                "F - Arrive\n" +
                "G - Wander\n";
        }

        var blueObj = GameObject.Find("Blue");
        if (blueObj != null) this.BlueCharacter = new DynamicCharacter(blueObj)
        {
            Drag = DRAG,
            MaxSpeed = MAX_SPEED
        };
        var greenObj = GameObject.Find("Green");
        if (greenObj != null) this.GreenCharacter = new DynamicCharacter(greenObj)
        {
            Drag = DRAG,
            MaxSpeed = MAX_SPEED
        };

        this.BlueMovementText = GameObject.Find("BlueMovement").GetComponent<Text>();
        this.GreenMovementText = GameObject.Find("GreenMovement").GetComponent<Text>();

        #region movement initialization

        var blueKinematicData = new KinematicData(new StaticData(this.BlueCharacter.GameObject.transform));
        var greenKinematicData = new KinematicData(new StaticData(this.GreenCharacter.GameObject.transform));

        this.BlueDynamicSeek = new DynamicSeek
        {
            Character = blueKinematicData,
            Target = this.GreenCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.BlueDynamicArrive = new DynamicArrive
        {
            Character = blueKinematicData,
            ArriveTarget = this.GreenCharacter.KinematicData,
            DebugTarget = this.blueDebugTarget,
            MaxSpeed = MAX_SPEED,
            MaxAcceleration = MAX_ACCELERATION,
            StopRadius = StopRadius,
            SlowRadius = SlowRadius,
        };

        this.BlueDynamicFlee = new DynamicFlee
        {
            Character = blueKinematicData,
            Target = this.GreenCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };


        this.BlueDynamicWander = new DynamicWander

        {
            Character = blueKinematicData,
            DebugTarget = this.blueDebugTarget,
            MaxAcceleration = MAX_ACCELERATION,
            WanderOffset = WanderOffset,
            WanderRadius = WanderRadius,
            WanderRate = WanderRate,
        };


        this.GreenDynamicSeek = new DynamicSeek
        {
            Character = greenKinematicData,
            Target = this.BlueCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.GreenDynamicArrive = new DynamicArrive
        {
            Character = greenKinematicData,
            ArriveTarget = this.BlueCharacter.KinematicData,
            DebugTarget = this.greenDebugTarget,
            MaxSpeed = MAX_SPEED,
            StopRadius = StopRadius,
            SlowRadius = SlowRadius,
        };

        this.GreenDynamicFlee = new DynamicFlee
        {
            Character = greenKinematicData,
            Target = this.BlueCharacter.KinematicData,
            MaxAcceleration = MAX_ACCELERATION
        };

        this.GreenDynamicWander = new DynamicWander()
        {
            Character = greenKinematicData,
            DebugTarget = this.greenDebugTarget,
            MaxAcceleration = MAX_ACCELERATION
        };


        #endregion
    }

    void Update()
    {
        #region input
        // Dealing with user's input
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.BlueCharacter.Movement = null;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            this.BlueCharacter.Movement = this.BlueDynamicSeek;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            this.BlueCharacter.Movement = this.BlueDynamicFlee;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            this.BlueCharacter.Movement = this.BlueDynamicArrive;
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            this.BlueCharacter.Movement = this.BlueDynamicWander;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            this.GreenCharacter.Movement = null;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            this.GreenCharacter.Movement = this.GreenDynamicSeek;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            this.GreenCharacter.Movement = this.GreenDynamicFlee;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            this.GreenCharacter.Movement = this.GreenDynamicArrive;
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.GreenCharacter.Movement = this.GreenDynamicWander;
        }
        #endregion

        // Updating each of the characters
        this.UpdateMovingGameObject(this.BlueCharacter);
        this.UpdateMovingGameObject(this.GreenCharacter);

        //Making sure the Debug objects are also updated
        if (this.blueDebugTarget != null && this.BlueCharacter.Movement != null)
        {
            this.blueDebugTarget.transform.position = this.BlueCharacter.Movement.Target.Position;
        }

        if (this.greenDebugTarget != null && this.GreenCharacter.Movement != null)
        {
            this.greenDebugTarget.transform.position = this.GreenCharacter.Movement.Target.Position;
        }

        this.UpdateMovementText();
    }

    private void UpdateMovingGameObject(DynamicCharacter movingCharacter)
    {
        if (movingCharacter.Movement != null)
        {
            movingCharacter.Update();
            movingCharacter.KinematicData.ApplyWorldLimit(X_WORLD_SIZE, Z_WORLD_SIZE);
            movingCharacter.GameObject.transform.position = movingCharacter.KinematicData.Position;
        }
    }

    private void UpdateMovementText()
    {
        if (this.GreenCharacter.Movement == null)
        {
            this.GreenMovementText.text = "Green Movement: Stationary";
        }
        else
        {
            this.GreenMovementText.text = "Green Movement: " + this.GreenCharacter.Movement.Name;
        }

        if (this.BlueCharacter.Movement == null)
        {
            this.BlueMovementText.text = "Blue Movement: Stationary";
        }
        else
        {
            this.BlueMovementText.text = "Blue Movement: " + this.BlueCharacter.Movement.Name;
        }
    }
}
