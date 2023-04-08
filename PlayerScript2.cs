using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerScript2 : MonoBehaviour
{
	//Controls the ball's speed
	public float speed;	
	//Accesses the ball's rigidBody
	[HideInInspector]
	public Rigidbody rBody;
	//Controls the player's movement
	private Vector3 movement;
	//Controls the player's acceleration
	private Vector3 acceleration;
	//Accesses how the player moves from left to right
	private float moveHorizontal;
	//Acceses how the player moves up and down
	private float moveVertical;
	//Checks to see if the ball is touching the ground
	private bool grounded;
	//The height that the ball jumps at
	public float jumpHeight;
	//Keeps track of the ball's score
	[HideInInspector]
	public int score;
	//Accesses the ball's transform 
	[HideInInspector]
	public Transform transform;
	//Acceses the player's initial position at the start of the game
	[HideInInspector]
	public Vector3 position;
	//Accesses the first player's gameobject
	public GameObject player1;
	//Accesses the first player's script
	[HideInInspector]
	public PlayerScript1 p1Script;
	//Text that keeps track of the score
	public TMP_Text scoreText;
	//Accesses the platform
	public GameObject platform;
	//Controls the jump audio source
	public AudioSource jSource;
	//Controls the junp sound effect
	public AudioClip jumpSE;
	//Decides whether the player can move (will stop if powerup collected or game is over)
	[HideInInspector]
	public bool active;

	//ONLY LEVEL2 VARIABLES
	//Checks whether the player is frozen (happens by collectible)
	[HideInInspector]
	public bool frozen;
	//The amount of time the ball is affected by the frozen powerup 
	private float timeFrozen;
	//Accesses the player's renderer
	[HideInInspector]
	public Renderer pRenderer;
	//Contains the ball's normal color when not affected by a powerup
	public Material startingColor;
	//Contains the color that the ball turns to when affected by the frozen powerup 
	public Material frozenColor;
	//Is used to contain the time the player sets the frozen powerup to so the time can be reset to this variable
	private float setTimeFrozen;
	//Whether the player has their controls inverted from a collectible
	[HideInInspector]
	public bool inverted;
	//Stores what color the player will turn after the inverted powerup is picked up
	public Material invertedColor;
	//The audio source for the freeze sound effect
	public AudioSource fSource;
	//The freeze sound effect
	public AudioClip freezeSE;
	//The audio source for the inverted sound effect
	public AudioSource iSource;
	//The inverted sound effect
	public AudioClip invertedSE;
	//Whether the sound effect was already played
	private bool played;
	//The current quadrant the player is in
	[HideInInspector]
	public int quadrant;
	//Controls whether the player has been affected by the knockbackCollectible (they can be pushed farther back)
	[HideInInspector]
	public bool knockable;
	//The AudioSource for the knocked sound effect
	public AudioSource knockedSource;
	//The sound effect for when the player is affected by the knocked collectible
	public AudioClip knockedSE;


	
	void Start()
	{
		active = true;
		rBody = GetComponent<Rigidbody>();
		grounded = true;	
		transform = GetComponent<Transform>();
		position = transform.position;
		score = 0;
		p1Script = player1.GetComponent<PlayerScript1>();
		scoreText.text = "Score: " + score;
		if(p1Script.isLvl2)
		{
			pRenderer = GetComponent<Renderer>();
			timeFrozen = p1Script.timeFrozen;
			setTimeFrozen = timeFrozen;
		}		
	}
	void Update()
	{	
		if(active)
		{
			if(!inverted)
			{
				moveHorizontal = Input.GetAxis("Horizontal2");		
				moveVertical = Input.GetAxis("Vertical2"); 
			}
			else
				Invert();
			movement = new Vector3(moveHorizontal, 0.0f, moveVertical);	//Vector3s deal with movement in 3D space.  X, Y, and Z aspects.  In this case the Y is zero.  Vector3s take floats.	
			acceleration = movement * speed;
			rBody.velocity += acceleration * Time.deltaTime;
			//rBody.AddForce(movement * speed * Time.deltaTime); //This accesses the rigidbody component and adds force ot get it moving
			//This will make the player not able to jump if they are affected by the knocked powerup
			if(Input.GetKey(KeyCode.RightShift) && grounded && !knockable)
				Jump();
			if(knockable)
			{
				Knocked();
			}
		}
		//This will check if the player is frozen, and if it is, it will call the Freeze() function
		else if (frozen)
			Freeze();
		else if (p1Script.respawned)
			p1Script.PlayersDontMove();
		else
			rBody.velocity = new Vector3(0f, 0f, 0f);
	}
	private void Jump()
	{
		jSource.PlayOneShot(jumpSE);
		grounded = false; 
		rBody.AddForce(0, jumpHeight, 0);
	}
	void OnCollisionStay(Collision c)
	{
		if(c.gameObject.tag == "ground")
			grounded = true; 
		if(c.gameObject.tag == "quad1")
			quadrant = 1;
		else if (c.gameObject.tag == "quad2")
			quadrant = 2;
		else if (c.gameObject.tag == "quad3")
			quadrant = 3;
		else if (c.gameObject.tag == "quad4")
			quadrant = 4;
		else if(c.gameObject.tag == "quad5")
			quadrant = 5;
		else if(c.gameObject.tag == "quad6")
			quadrant = 6;
	}	
	void OnCollisionEnter(Collision c)
	{
		if(c.gameObject.tag == "boundary")
			FallOff();	
		if(c.gameObject.tag == "Player" && frozen)
		{
			Unfreeze();
		}
		if(c.gameObject.tag == "Player" && p1Script.knockable)
		{
			p1Script.ApplyKnockback();
		}
			
		
	}
	private void FallOff()
	{
		active = false;
		p1Script.active = false;
		p1Script.respawned = true;
		if(p1Script.isLvl2)
		{
			if(p1Script.frozen)
				p1Script.Unfreeze();
			if(frozen)
				Unfreeze();
			if(p1Script.inverted)
				p1Script.Revert();
			else if(inverted)
				Revert();
			else if(knockable)
				ResetKnocked();
			else if(p1Script.knockable)
				p1Script.ResetKnocked();
		}
		//Unfreeze();
		transform.position = position;
		p1Script.transform.position = p1Script.position;
		rBody.velocity = new Vector3(0f, 0f, 0f);
		p1Script.rBody.velocity = new Vector3(0f, 0f, 0f);
		p1Script.score++;
		p1Script.scoreText.text = "Score: " + p1Script.score;
		platform.GetComponent<PlatformScript>().resetPlatform();
	}
	void OnTriggerEnter(Collider c){
		//If this player touches the freezeCollectible, then the other player will not be able to move
        if(c.gameObject.tag == "freezeCollectible")
		{
			p1Script.active = false;
			p1Script.frozen = true;
		}  
		else if (c.gameObject.tag == "invertedCollectible")
			p1Script.inverted = true;
		else if(c.gameObject.tag == "knockbackCollectible")
			p1Script.knockable = true;
    }
	public void Freeze()
	{
		if(!played)
		{
			fSource.PlayOneShot(freezeSE);
			played = true;
		}	
		rBody.velocity = new Vector3(0f, 0f, 0f);
		pRenderer.material = frozenColor;
		if(timeFrozen > 0)
			timeFrozen -= Time.deltaTime;
		else
			Unfreeze();
	}
	public void Unfreeze()
	{
		played = false;
		active = true;
		frozen = false;
		pRenderer.material = startingColor;
		timeFrozen = setTimeFrozen;
	}
	public void Invert()
	{
		if(!played)
		{
			iSource.PlayOneShot(invertedSE);
			//AudioSource.PlayClipAtPoint(invertedSE,  p1Script.platform.GetComponent<Transform>().position);
			played = true;
		}
		//AudioSource.PlayClipAtPoint(invertedSE, transform.position);
		moveHorizontal = Input.GetAxis("InvHorizontal2");		
		moveVertical = Input.GetAxis("InvVertical2"); 
		pRenderer.material = invertedColor;
		if(p1Script.timeInverted > 0)
		{
			p1Script.timeInverted -= Time.deltaTime;
		}
		else
			Revert();
	}
	public void Revert()
	{
		played = false;
		inverted = false;
		p1Script.timeInverted = p1Script.setTimeInverted;
		pRenderer.material = startingColor;
	}
	public void ApplyKnockback()
	{
		Vector3 knockbackForce = p1Script.rBody.transform.forward * p1Script.knockback;
		rBody.AddForce(knockbackForce);
	}
	public void Knocked()
	{
		//At the very start, change the player's color to black (if the timeKnocked is the same as the setTime, 
		//then it will be the start of the method. This will prevent it from being called from the Update() function multiple times)
		if(p1Script.timeKnocked == p1Script.setTimeKnocked)
		{
			knockedSource.PlayOneShot(knockedSE);
			pRenderer.material = p1Script.knockedColor;
		}
		if(p1Script.timeKnocked > 0)
		{
			p1Script.timeKnocked -= Time.deltaTime;
		}
		//After the timer is over, the player will no longer be "knockable"
		else
			ResetKnocked();				
	}
	//This will reset the effects of the knockback powerup
	public void ResetKnocked()
	{		

		p1Script.timeKnocked = p1Script.setTimeKnocked;
		pRenderer.material = startingColor;
		knockable = false;
	}
}
