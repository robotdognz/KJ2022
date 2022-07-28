using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLight : MonoBehaviour
{
    [SerializeField] GameObject[] points;
    int pointsIndex = 0;

    [SerializeField] GameObject theLight;
    public bool startAtEnd; // false means start at beginning, true means start at end 
    public float moveSpeed = 1;


    // public bool testMePlease = false;

    private Vector3 currentPosition;

    enum MoveState
    {
        Increase, // moving up
        Decrease, // moving down
    }

    MoveState moveState = MoveState.Increase;


    void Start()
    {
        // move light to the position of the first point if startEnd is false, otherwise move it to the last point
        if (startAtEnd)
        {
            theLight.transform.position = points[points.Length - 1].transform.position;
            // Rigidbody2D lightBody = theLight.GetComponent<Rigidbody2D>();
            // lightBody.MovePosition(points[points.Length - 1].transform.position);

            currentPosition = theLight.transform.position;
            pointsIndex = points.Length - 1;
            moveState = MoveState.Increase;
        }
        else
        {
            theLight.transform.position = points[0].transform.position;
            // Rigidbody2D lightBody = theLight.GetComponent<Rigidbody2D>();
            // lightBody.MovePosition(points[0].transform.position);

            currentPosition = theLight.transform.position;
            pointsIndex = 0;
            moveState = MoveState.Decrease;
        }
    }

    [ContextMenu("Test Movement")]
    public void DoMove()
    {
        switch (moveState)
        {
            case MoveState.Increase:
                moveState = MoveState.Decrease;
                break;
            case MoveState.Decrease:
                moveState = MoveState.Increase;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (testMePlease)
        // {
        //     testMePlease = false;
        //     DoMove();
        // }

        Vector3 targetPosition = points[pointsIndex].transform.position;
        float delta = moveSpeed * Time.deltaTime;
        theLight.transform.position = Vector2.MoveTowards(theLight.transform.position, targetPosition, delta);
        // Vector3 newPosition = Vector2.MoveTowards(theLight.transform.position, targetPosition, delta);
        // Rigidbody2D lightBody = theLight.GetComponent<Rigidbody2D>();
        // lightBody.MovePosition(newPosition);

        currentPosition = theLight.transform.position;

        switch (moveState)
        {
            case MoveState.Increase:
                if (pointsIndex < points.Length - 1)
                {
                    if (theLight.transform.position.x == targetPosition.x && theLight.transform.position.y == targetPosition.y)
                    // if (lightBody.position.x == targetPosition.x && lightBody.position.y == targetPosition.y)
                    {
                        // Debug.Log("Forward");
                        pointsIndex++;
                    }
                }
                break;
            case MoveState.Decrease:
                if (pointsIndex > 0)
                {
                    if (theLight.transform.position.x == targetPosition.x && theLight.transform.position.y == targetPosition.y)
                    // if (lightBody.position.x == targetPosition.x && lightBody.position.y == targetPosition.y)
                    {
                        // Debug.Log("Backward");
                        pointsIndex--;
                    }
                }
                break;
        }

    }
}
