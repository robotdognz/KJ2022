using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] Rigidbody2D doorBody;
    [SerializeField] float doorSpeed = 1;
    [Tooltip("Ration of door height to move when opening")]
    [SerializeField][Range(0, 1)] float movementAmount = 1;
    private float doorHeight;
    private float doorLocalY;
    private float doorGlobalY;

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

    public DoorType doorType = DoorType.Up;

    [HideInInspector] public DoorState currentDoorState = DoorState.Closing;

    private void Start()
    {
        doorHeight = doorBody.transform.localScale.y;
        doorLocalY = doorBody.transform.localPosition.y;
        doorGlobalY = doorBody.transform.position.y;
    }

    [ContextMenu("Open Door")]
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

    [ContextMenu("Close Door")]
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
                        if (doorBody.transform.localPosition.y > doorLocalY)
                        {
                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Max(newPosition.y, doorGlobalY);
                            doorBody.MovePosition(newPosition);

                        }
                        else
                        {
                            currentDoorState = DoorState.Closed;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.localPosition.y < doorLocalY)
                        {
                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Min(newPosition.y, doorGlobalY);
                            doorBody.MovePosition(newPosition);
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
                        if (doorBody.transform.localPosition.y < doorLocalY + doorHeight * movementAmount)
                        {
                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Min(newPosition.y, doorGlobalY + doorHeight * movementAmount);
                            doorBody.MovePosition(newPosition);
                        }
                        else
                        {
                            currentDoorState = DoorState.Opened;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.localPosition.y > doorLocalY - doorHeight * movementAmount)
                        {
                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Max(newPosition.y, doorGlobalY - doorHeight * movementAmount);
                            doorBody.MovePosition(newPosition);
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
