using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    // definition variables
    public float runSpeed; //17f
    public float climbSpeed; //14f

    // misc variables
    private int playerLayer = 3;
    private int groundLayer = 6;
    private Rigidbody2D myRB2D;

    // helper variables
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private bool jump = false;
    private bool inLadder = false;
    private bool onLadder = false;
    private bool inLadderTop = false;
    private float ladderTileXPosition = 0f;

    // default variables
    private float originalGravityScale;

    void Start () {
        // set all varaibles that need to be set
        myRB2D = this.GetComponent<Rigidbody2D>();
        originalGravityScale = this.myRB2D.gravityScale;
    }

    // Update is called once per frame
    void Update () {
        // set movement variables
        // TODO: if PlayerController.stunned == true, then no movement
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;

        // Jumping input
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            if (onLadder) { onLadder = false; }
        }

    }

    void FixedUpdate()
    {
        // On ladder physics
        // TODO: be able to face different directions on ladder
        if (onLadder)
        {
            // Just to make sure
            GetOnLadder();
            // TODO: this code is to prevent the bug of okayu not properly being positioned when getting on the ladder at high velocities.
            // Not sure why this bug exists
            if (this.transform.position.x != ladderTileXPosition) { this.transform.position = new Vector3(ladderTileXPosition, this.transform.position.y,this.transform.position.z); }

            // Getting off ladder on bottom: touching ground
            // Note this code must come before getting on ladder code
            if (controller.Grounded)
            {
                GetOffLadder();
            }
            // Getting off ladder on the top
            // TODO: is this bad logic?
            if (onLadder && !inLadder)
            {
                this.myRB2D.velocity = new Vector3(0,0,0);
                GetOffLadder();
                this.transform.position = new Vector3(  this.transform.position.x, 
                                                        this.transform.position.y + 0.5f, 
                                                        this.transform.position.z);
            }

            // Call to CharacterController2D for climbing
            controller.Climb(verticalMove * Time.fixedDeltaTime);
        }

        // Off ladder physics
        if (!onLadder)
        {
            // Just to make sure
            GetOffLadder();

            // Getting on ladder going up: positive vertical input, in ladder, and grounded
            if (verticalMove > 0 && inLadder && controller.Grounded)
            {
                this.myRB2D.velocity = new Vector3(0,0,0);
                GetOnLadder();
                this.transform.position = new Vector3(  ladderTileXPosition, 
                                                        this.transform.position.y + 0.05f, 
                                                        this.transform.position.z);
            }

            // Getting on ladder going down: negative vertical input, ladder is nearby, and grounded
            if (verticalMove < 0 && inLadderTop && controller.Grounded)
            {
                this.myRB2D.velocity = new Vector3(0,0,0);
                GetOnLadder();
                this.transform.position = new Vector3(  ladderTileXPosition, 
                                                        this.transform.position.y - 0.5f, 
                                                        this.transform.position.z);
            }

            // Call to CharacterController2D for movement
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        }

        jump = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder") 
        { 
            inLadder = true; 
            // TODO: hardcodd the small difference between entering ladder from left vs right
            ladderTileXPosition = col.ClosestPoint(this.transform.position).x;
            ladderTileXPosition = Mathf.Round(ladderTileXPosition * 4) / 4;
        }
        if (col.gameObject.tag == "LadderTop")
        {
            inLadderTop = true;
            ladderTileXPosition = col.ClosestPoint(this.transform.position).x;
            ladderTileXPosition = Mathf.Round(ladderTileXPosition * 4) / 4;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Ladder") { inLadder = false; }
        if (col.gameObject.tag == "LadderTop") { inLadderTop = false; }
    }

    // helper methods
    void GetOnLadder()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        this.myRB2D.gravityScale = 0;
        onLadder = true;
    }
    void GetOffLadder()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        this.myRB2D.gravityScale = originalGravityScale;
        onLadder = false;
    }


}
