using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] Rigidbody2D doorBody;
    [SerializeField] float doorSpeed = 1;
    private float doorHeight;
    private float doorY;

    public enum DoorType
    {
        Up,
        Down
    }

    public enum DoorState
    {
        Closing,
        Closed,
        Opening,
        Opened
    }

    [Tooltip("Constant will always trigger OnActivate, OnOff will will switch between triggering OnActivate and OnDeactivate")]
    public DoorType doorType = DoorType.Up;

    public DoorState currentDoorState = DoorState.Closing;

    private void Start()
    {
        doorHeight = doorBody.gameObject.transform.localScale.y;
        doorY = doorBody.gameObject.transform.position.y;
    }

    // public void DoDoor()
    // {
    //     switch (currentDoorState)
    //     {
    //         case DoorState.Closed:
    //         case DoorState.Closing:
    //             currentDoorState = DoorState.Opening;
    //             break;
    //         case DoorState.Opened:
    //         case DoorState.Opening:
    //             currentDoorState = DoorState.Closing;
    //             break;
    //     }
    // }

    public void OpenDoor()
    {
        switch (currentDoorState)
        {
            case DoorState.Closed:
            case DoorState.Closing:
                currentDoorState = DoorState.Opening;
                break;
        }
    }

    public void CloseDoor()
    {
        switch (currentDoorState)
        {
            case DoorState.Opened:
            case DoorState.Opening:
                currentDoorState = DoorState.Closing;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentDoorState)
        {
            case DoorState.Closing:
                switch (doorType)
                {
                    case DoorType.Up:
                        if (doorBody.transform.position.y > doorY)
                        {
                            doorBody.MovePosition(doorBody.position + new Vector2(0, -doorSpeed * Time.fixedDeltaTime));
                        }
                        else
                        {
                            currentDoorState = DoorState.Closed;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.position.y < doorY)
                        {
                            doorBody.MovePosition(doorBody.position + new Vector2(0, doorSpeed * Time.fixedDeltaTime));
                        }
                        else
                        {
                            currentDoorState = DoorState.Closed;
                        }
                        break;
                }
                break;
            case DoorState.Opening:
                switch (doorType)
                {
                    case DoorType.Up:
                        if (doorBody.transform.position.y < doorY + doorHeight)
                        {
                            doorBody.MovePosition(doorBody.position + new Vector2(0, doorSpeed * Time.fixedDeltaTime));
                        }
                        else
                        {
                            currentDoorState = DoorState.Opened;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.position.y > doorY - doorHeight)
                        {
                            doorBody.MovePosition(doorBody.position + new Vector2(0, -doorSpeed * Time.fixedDeltaTime));
                        }
                        else
                        {
                            currentDoorState = DoorState.Opened;
                        }
                        break;
                }
                break;
        }
    }
}
