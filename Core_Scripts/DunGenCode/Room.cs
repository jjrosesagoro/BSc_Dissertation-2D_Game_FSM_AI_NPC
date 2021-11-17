using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (RoomController.instance == null) //ensuring that the player is starting within the correct scene
        {
            Debug.Log("Game initiliased from incorrect scene");
            return; //if game is initialised in wrong scene, return to Ground room
        }

        RoomController.instance.RoomRegistered(this);
    }

    void OnDrawGizmos() //creates a gizmo which allows for objects within the scene to be manipulated with ease
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre() //finds the vector in the centre of the room so the player can spawn in it
    {
        return new Vector3(X * Width, Y * Height); //returning the values for the centre of the room
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        {
            Debug.Log("Collided");
            if (other.tag == "Player")
            {
                Debug.Log("PlayerCollision");
                RoomController.instance.PlayerWithinRoom(this);
            }
        }
    }
}

