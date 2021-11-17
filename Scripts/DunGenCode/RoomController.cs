using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInformation //figure out where the player is within procedural rooms
{
    public string name;
    public int X; //X coorindate location with reference to the scene
    public int Y; //Y coordinate location with reference to the scene
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance; //singleton instance created to ensure a class has only a single globally accessible instance available

    string CurrentRoomName = "Ground"; //name of room within currently

    RoomInformation CurrentLoadRoomData; //fetching data of the room which is currently loaded

    Room CurrentRoom;

    Queue<RoomInformation> LoadRoomQueue = new Queue<RoomInformation>(); //queue is a first in, first out structure, used for loading scenes so they load in order

    public List<Room> LoadedRooms = new List<Room>();

    bool IsLoadingRoom = false;

    void Awake() //awake method called when project is initliased rather than using Start method which is only called once the script is initiliased
    {
        instance = this;
    }

    void Start() //loading rooms for test purposes once the scene loads
    {
        RoomLoad("Start", 0, 0);
        RoomLoad("Empty", 1, 0);
        RoomLoad("Empty", -1, 0);
        RoomLoad("Empty", 0, 1);
        RoomLoad("Empty", 0, -1);
    }

    void Update() //update method called to reload the roomqueue method
    {
        RoomQueueUpdate();
    }

    void RoomQueueUpdate()
    {
        if(IsLoadingRoom) //if the room is loading then return the value
        {
            return;
        }

        if(LoadRoomQueue.Count == 0) //if there is nothing in the queue to be loaded then null will be returned because nothing is waiting
        {
            return;
        }

        CurrentLoadRoomData = LoadRoomQueue.Dequeue(); //if there are things in the queue we need to grab the load room data and update it and or remove it
        IsLoadingRoom = true;

        StartCoroutine(RoutineRoom(CurrentLoadRoomData)); //the coroutine method returns upon the first yield return. Will not stand execution until result is yielded
    }

    public void RoomLoad (string name, int x, int y) //method created to deal with loading scenes
    {
        if(ExistingRoom(x, y) == true)
        {
            return; //returning because I do not want to load the next chunk of code if the room already exists
        }

        RoomInformation NewData = new RoomInformation(); //the information from the current scene being applied to the new scene
        NewData.name = name;
        NewData.X = x;
        NewData.Y = y;

        LoadRoomQueue.Enqueue(NewData); //adding new room data to the queue of things being initilaised
    }

    IEnumerator RoutineRoom(RoomInformation info) //scenes take a while to load so this method loads the current scene without the others
    {
        string FloorName = CurrentRoomName + info.name;

        AsyncOperation LoadRoom = SceneManager.LoadSceneAsync(FloorName, LoadSceneMode.Additive); /*additive function makes it so that scenes can overlap. 
        This needs to be done so all scenes can be loaded in the same room*/
        //Asyncoperation is used to allow scenes to be activated once ready, checks whether the operation has completed, and gives feedback on operation process.

        while(LoadRoom.isDone == false) //once the scene has been loaded then a value will be returned. Not before.
        {
            yield return null; //return nothing until scene has been loaded.
        }
    }

    public void RoomRegistered(Room floor) //new public void declared as room to work as a room controller
    {
        if(!ExistingRoom(CurrentLoadRoomData.X, CurrentLoadRoomData.Y))
        {
            floor.transform.position = new Vector3( //setting the room within the scene with the correct coorindates
            CurrentLoadRoomData.X * floor.Width,
            CurrentLoadRoomData.Y * floor.Height,
            0 //Z coorindate set to zero
        );

        floor.X = CurrentLoadRoomData.X;
        floor.Y = CurrentLoadRoomData.Y;
        floor.name = CurrentRoomName + "-" + CurrentLoadRoomData.name + " " + floor.X + ", " + floor.Y; /*see within the naming convention, the coordinates to where it has 
        just been placed*/
        floor.transform.parent = transform; //telling it to transform without specifying position because it's already been given

        IsLoadingRoom = false; //room finished loading so set the isloading value to false
        
        if(LoadedRooms.Count == 0)
        {
            CameraController.instance.CurrentRoom = floor; //if no rooms have been loaded then the current instance of the camera needs to be set to the view of the current room
        }

        LoadedRooms.Add(floor); //adding to the list which have loaded for reference
        }   
        else
        {
            Destroy(floor.gameObject);
            IsLoadingRoom = false;
        }
}

   public bool ExistingRoom(int x, int y) //true or false value to determine whether this room exists based on player coordinates within instance
    {
        return LoadedRooms.Find(item => item.X == x && item.Y == y) != null; //lamda expression used to define an unnamed function that can be passed as a variable
    }

    public Room RoomFinder(int x, int y)
    {
        return LoadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void PlayerWithinRoom(Room floor)
    {
        CameraController.instance.CurrentRoom = floor;
        CurrentRoom = floor;

        StartCoroutine(CoroutineForRoom());
    }

    public IEnumerator CoroutineForRoom()
    {
        yield return new WaitForSeconds(0.2f);
        RoomUpdate();
    }

    public void RoomUpdate()
    {
        
    }
}
