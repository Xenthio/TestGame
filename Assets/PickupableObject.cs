using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupableObject : MonoBehaviour {

    Transform playerTransform;          //player's Transform component  
    Rigidbody rb;                       //object's rigidbody
    FirstPersonAIO playerController;
   	GameObject holdPos;
   	Collider plyr; 

        
    float throwPower = 10f;             //amount of throw power
    bool pickedUp = false;              //bools which tells us if the objects was picked up

    private void Awake()
    {
    	holdPos = new GameObject("HoldPosition");
    	holdPos.transform.SetParent(Camera.main.transform);                                              //turns Kinematic on, so object will not interfere when picked up
	    holdPos.transform.localPosition = new Vector3(0f, 0f, 1.5f);
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = transform.GetComponent<Rigidbody>();
        playerController = GameObject.Find("Player").GetComponent<FirstPersonAIO>();
        plyr = GameObject.Find("Player").GetComponent<Collider>();
    }

    private void Update()
    {
        HandleObjectThrow();
        HandleObjectInteract();
        HandleObjectRotation();
        Debug.Log(Vector3.Distance(transform.position, holdPos.transform.position));
    }


    //Picks up the object from ground
    private void HandleObjectInteract()
    {
    	if (Input.GetKeyDown(KeyCode.E))
    	{
	        if (Vector3.Distance(playerTransform.position, transform.position) < 2.2f && pickedUp == false)      //If player is close enough
	        {
	            //transform.SetParent(Camera.main.transform);                                       //sets the parent of the object to camera (object will be always in front of it)
	            //rb.isKinematic = true;                                                      //turns Kinematic on, so object will not interfere when picked up
	            transform.position = holdPos.transform.position;                      //sets local position in front of player, so it seems player is holding it
	            transform.rotation = Quaternion.Euler(0, 0f, 0);                       //rotates the object
	            rb.useGravity = false;
	            pickedUp = true;                                                            //tell's that object was picked up
	        }
	      	else if (pickedUp == true)                            //if player clicked 'E' and picked up object before
       		{
            	transform.SetParent(null);                                                  //unleashes object from its parent
            	//rb.isKinematic = false;                                                     //turns kinematic off, so object can lightly fall on the ground
            	rb.useGravity = true;
            	pickedUp = false;                                                           //object is not picked up

        	}
	    }
    }
    private void FixedUpdate() 
    {
    	if (pickedUp == true)                            //if player clicked 'E' and picked up object before
       	{
       		//transform.localPosition = new Vector3(0f, 0f, 1.5f);
       		rb.MovePosition(holdPos.transform.position); 
       		//transform.position = Vector3.MoveTowards(transform.position, holdPos.transform.position, 4 * Time.deltaTime);
       		transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0); 
       	}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && pickedUp == true)
        {
            Physics.IgnoreCollision(plyr, GetComponent<Collider>());
        }
    }
    private void OnCollisionStay(Collision CollisionData) // fuck unity physics, can't do decent collisions everything clips through with enough force.
    {
    	if (pickedUp == true)                            //if player clicked 'E' and picked up object before
       	{
    	    if (Vector3.Distance(transform.position, holdPos.transform.position) > 0.15) // so the dumbass thing doesn't clip through blocks
    	    {
    	    	
            	rb.useGravity = true;
            	pickedUp = false;  
    	    }
        }
    }
    //Handles object throw
    void HandleObjectThrow()
    {
            if (Input.GetMouseButtonDown(0) && pickedUp == true)                        // throw on click
            {

                transform.SetParent(null);                                              //unleashes object from its parent
                rb.useGravity = true;                                              //turns off kinematic, so object can be physical again
                rb.AddForce(new Vector3(Camera.main.transform.forward.x,0.2f, Camera.main.transform.forward.z) * throwPower, ForceMode.Impulse);    //Adds force in direction where camera is looking with 'throwpower' power and Impulse forcemode
                pickedUp = false;                                                       //object is not picked up

            }
    }


    //Handles putting object lightly on the ground
    void HandleObjectPut()
    {
        if (Input.GetKeyDown(KeyCode.E) && pickedUp == true)                            //if player clicked 'E' and picked up object before
        {
            transform.SetParent(null);                                                  //unleashes object from its parent
            rb.isKinematic = false;                                                     //turns kinematic off, so object can lightly fall on the ground
            pickedUp = false;                                                           //object is not picked up
        }
    }


    //Handles rotating picked up object
    void HandleObjectRotation()
    {
        if (pickedUp == true)
        {
            if (Input.GetKey(KeyCode.R))
            {
                float rotationX = Input.GetAxis("Mouse X") * 2f;
                float rotationY = Input.GetAxis("Mouse Y") * 2f;

                transform.RotateAround(Camera.main.transform.up, -Mathf.Deg2Rad * rotationX);
                transform.RotateAround(Camera.main.transform.right, Mathf.Deg2Rad * rotationY);
            }
        }
    }
}