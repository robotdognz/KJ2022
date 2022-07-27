using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] Rigidbody2D doorBody;
    [SerializeField] float doorSpeed = 1;
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

    [Tooltip("Constant will always trigger OnActivate, OnOff will will switch between triggering OnActivate and OnDeactivate")]
    public DoorType doorType = DoorType.Up;

    public DoorState currentDoorState = DoorState.Closing;

    private void Start()
    {
        doorHeight = doorBody.transform.localScale.y;
        // doorY = doorBody.gameObject.transform.position.y;
        doorLocalY = doorBody.transform.localPosition.y;
        doorGlobalY = doorBody.transform.position.y;
    }

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
                        if (doorBody.transform.localPosition.y > doorLocalY)
                        {
                            // doorBody.MovePosition(doorBody.transform.localPosition + new Vector3(0, -doorSpeed * Time.fixedDeltaTime));

                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Max(newPosition.y, doorGlobalY);
                            doorBody.MovePosition(newPosition);

                        }
                        else
                        {
                            // Debug.Log(new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime)));
                            // Debug.Log(doorBody.transform.localPosition.y);
                            currentDoorState = DoorState.Closed;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.localPosition.y < doorLocalY)
                        {
                            // doorBody.MovePosition(doorBody.transform.localPosition + new Vector3(0, doorSpeed * Time.fixedDeltaTime));

                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Min(newPosition.y, doorGlobalY);
                            doorBody.MovePosition(newPosition);
                        }
                        else
                        {
                            // Debug.Log(new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime)));
                            // Debug.Log(doorBody.transform.localPosition.y);
                            currentDoorState = DoorState.Closed;
                        }
                        break;
                }
                break;
            case DoorState.Opening:
                switch (doorType)
                {
                    case DoorType.Up:
                        if (doorBody.transform.localPosition.y < doorLocalY + doorHeight)
                        {
                            // doorBody.MovePosition(doorBody.transform.localPosition + new Vector3(0, doorSpeed * Time.fixedDeltaTime));

                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Min(newPosition.y, doorGlobalY + doorHeight);
                            doorBody.MovePosition(newPosition);
                        }
                        else
                        {
                            // Debug.Log(new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, doorSpeed * Time.fixedDeltaTime)));
                            // Debug.Log(doorBody.transform.localPosition.y);
                            currentDoorState = DoorState.Opened;
                        }
                        break;
                    case DoorType.Down:
                        if (doorBody.transform.localPosition.y > doorLocalY - doorHeight)
                        {
                            // doorBody.MovePosition(doorBody.transform.localPosition + new Vector3(0, -doorSpeed * Time.fixedDeltaTime));

                            Vector3 newPosition = new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime));
                            newPosition.y = Mathf.Max(newPosition.y, doorGlobalY - doorHeight);
                            doorBody.MovePosition(newPosition);
                        }
                        else
                        {
                            // Debug.Log(new Vector3(doorBody.position.x, doorBody.position.y) + transform.TransformDirection(new Vector3(0, -doorSpeed * Time.fixedDeltaTime)));
                            // Debug.Log(doorBody.transform.localPosition.y);
                            currentDoorState = DoorState.Opened;
                        }
                        break;
                }
                break;
        }
    }
}
