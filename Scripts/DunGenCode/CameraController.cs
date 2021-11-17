using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

{
    public Transform Player;
    public float TimeDamp = 0.4f;
    private Vector3 PositionOfCamera;
    private Vector3 velocity = Vector3.zero;

    public static CameraController instance; //instance used to store object data
    public Room CurrentRoom; //method to hold the data for which room the player is in
    public float CameraMovementSpeed; //method to control the speed at which the camera moves

    void awake ()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        PositionOfCamera = new Vector3(Player.position.x, Player.position.y, -10f);
        transform.position = Vector3.SmoothDamp(gameObject.transform.position, PositionOfCamera, ref velocity, TimeDamp);

        PositionAfterUpdate(); //updating the position of the camera so the predefined method is called here
    }

    void PositionAfterUpdate()
    {
        if(CurrentRoom == null)
        {
            return; //If the room isn't updated then the position of the camera just needs to stay the same and so it returns null
        }

        Vector3 PositionOfTarget = FetchTargetCameraPosition();
        transform.position = Vector3.MoveTowards(transform.position, PositionOfTarget, Time.deltaTime * CameraMovementSpeed);
        //using the position of the player and the movement speed to decide the speed at which the camera moves

    }

        Vector3 FetchTargetCameraPosition()
        {
            if(CurrentRoom == null)
            {
                return Vector3.zero;
            }

            Vector3 PositionOfTarget = CurrentRoom.GetRoomCentre(); //finding the position of the target within the current room whilst finding the value of the vector for the centre
            PositionOfTarget.z = transform.position.z;
            
            Debug.Log("Got centre");
            return PositionOfTarget;
        }

        public bool IsSwtichingScene() //bool to check when the player is transitioning between scenes so the camera can be updated
        {
            return transform.position.Equals( FetchTargetCameraPosition()) == false;
        }
    }

