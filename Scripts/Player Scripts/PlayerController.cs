using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour

{
    
    public float speed; //calculating the speed at which the player is moving
    Rigidbody2D Player; //method that controls physics for 2D sprites
    public Vector2 MovementVelocity;
    public GameObject SnotShot;
    public float SnotShotSpeed;
    public float SnotShotDelay;
    private float LastFired;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<Rigidbody2D>(); //specifying which rigid body the RigBod variable is equal to
        //Within Unity set the RigidBody2D component from dyanmic to kinematic so that the player is only affected by user input and no external forces such as gravity
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
             {
                 SceneManager.LoadScene(0); //escape key brings user back to main menu
             }

        Vector2 MovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); //Raw function added to create snappy effect by responding to key touches
        //function used to determine where the player wants to move

        float SnotShotHor = Input.GetAxis("ShootHorizontal");
        float SnotShotVer = Input.GetAxis("ShootVertical");

        if((SnotShotHor != 0 || SnotShotVer != 0) && Time.time > LastFired + SnotShotDelay)
        {
            SnotShoot(SnotShotHor, SnotShotVer);
            LastFired = Time.time;
        }

        MovementVelocity = MovementInput.normalized * speed; //The relative coordinate of the vector from velocity to input are equal to one another
        //Movement speed should always be 1 and the normalized function makes it so that if two keys are pressed simultaneously to move diagonally the speed will stay at 1 instead of 2
    }

    void FixedUpdate() //all code related to adjusting in game physics falls within this method
    {
        Player.MovePosition(Player.position + MovementVelocity * Time.fixedDeltaTime); //every physics step is being processed while the game runs as long as it receives input
    }

    public void SnotShoot(float x, float y)
    {
        GameObject Tear = Instantiate(SnotShot, transform.position, transform.rotation) as GameObject;
        Tear.AddComponent<Rigidbody2D>().gravityScale = 0;
        Tear.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * SnotShotSpeed : Mathf.Ceil(x) * SnotShotSpeed, //ternary operator which acts as a boolean only checking for a true or false value
            (y < 0) ? Mathf.Floor(y) * SnotShotSpeed : Mathf.Ceil(y) * SnotShotSpeed,
            0
        );
    }

}