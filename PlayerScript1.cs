using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerScript1 : MonoBehaviour
{
	//Controls the ball's speed
	public float speed;	
	//Accesses the ball's rigidBody
	[HideInInspector]
	public Rigidbody rBody;
	//Contorls the player's movement
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
	//Accesses the ball's transform 
	[HideInInspector]
	public Transform transform;
	//Keeps track of the ball's score
	[HideInInspector]
	public int score;
	//Acceses the player's initial position at the start of the game
	[HideInInspector]
	public Vector3 position;
	//Accesses the second player's gameobject
	public GameObject player2;
	//Accesses the second player's script
	[HideInInspector]
	public PlayerScript2 p2Script;
	//Text that keeps track of the score
	public TMP_Text scoreText;
	//Accesses the platform
	public GameObject platform;
	//The score that a player has to reach before the game ends
	public int maxScore;
	//Stores a button that can be pressed to restart the level and one that can go back to the menu
    public GameObject restartButton, menuButton;
    //The text that will tell the player that they won
    public TMP_Text winText;
	//Checks if the game is over
	[HideInInspector]
	public bool active;
	//Controls the collision audio source
	public AudioSource cSource;
	//Controls the collision sound effect
	public AudioClip collisionSE;
	//Controls how long the instruction text stays up
	public float iTextTimer;
	//Controls Player 1's instructions text and Player 2's instructions text
	public TMP_Text redInstructions, purpleInstructions;
	//Controls the game's pause menu
	public GameObject pauseMenu;
	//Controls the game's pause button
	public GameObject pauseButton;
	//Controls the jump audio source
	public AudioSource jSource;
	//Controls the jump sound effect
	public AudioClip jumpSE;
	//Controls how long the players cannot move after respawning
	public float timeAtStart;
	//Whether the players have just recently respawned
	[HideInInspector]
	public bool respawned;
	//Is used to contain the time the player sets the amount of time the players cannot move to, so the time can be reset to this variable
	[HideInInspector]
	public float setTimeAtStart;
	//The audio source for the click sound effect
	public AudioSource clickSource;
	//The sound effect that plays when a button is pressed
	public AudioClip clickSE;
	//The volume that the music is normally (without being paused)
	private float normalAMusicVolume;
	//The audio source for the music playing for the certain arena
	public AudioSource arenaMusicSource;


	//ONLY LEVEL 2 VARIABLES
	//Checks whether the ball is currently affected by the frozen powerup
	[HideInInspector]
	public bool frozen;
	//Checks whether the current level is level2
	[HideInInspector]
	public bool isLvl2;
	//The amount of time the ball is affected by the frozen powerup 
	public float timeFrozen;
	//Accesses the player's renderer
	[HideInInspector]
	public Renderer pRenderer;
	//Contains the ball's normal color when not affected by a powerup
	public Material startingColor;
	//Contains the color that the ball turns to when affected by the frozen powerup 
	public Material frozenColor;
	//Is used to contain the time the player sets the frozen powerup to so the time can be reset to this variable
	private float setTimeFrozen;
	//Accesses the frozen collectible
	public GameObject frozenCollectible;
	//Accesses the inverted collectible
	public GameObject invertedCollectible;
	//Accesses the collectible that increases the knockback of the players
	public GameObject knockbackCollectible;
	//Used to accesses the minimum x and z values in the area that the collectible can spwawn in 
	public GameObject collectiblePos1;
	//Used to accesses the maximum  x and z values in the area that the collectible can spwawn in 
	public GameObject collectiblePos2;
	//The x value that the collectible will spawn in 
	private float spawningXVal;
	//The y value that the collectible will spawn in
	private float spawningZVal;
	//The y-value that the collectible will spawn 
	private float yValue; 
	//The time that will be set to the time the collectible will take to spawn
	private float collectibleSpawnTime;
	//The number of freezeCollectibles that will spawn throughout the match
	public int numOfCollectibles;
	//Whether the player has their controls inverted from a collectible
	[HideInInspector]
	public bool inverted;
	//The amount of time the player will be inverted
	public float timeInverted;
	//Used to store the time the player should be inverted so the timeInverted can be set back after being depleted
	[HideInInspector]
	public float setTimeInverted;
	//Stores the current collectible that will be spawned
	private GameObject currentCollectible;
	//Stores the collectible script
	[HideInInspector]
	public Rotator cScript;
	//Stores what color the player will turn after the inverted powerup is picked up
	public Material invertedColor;
	//Whether the gane is currently paused
	private bool paused;
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
	//Detects whether the game is over
	[HideInInspector]
	public bool gameOver;
	//The current quadrant the player is in
	private int quadrant;
	//The quadrant that the collectible will spawn in
	private GameObject spawningQuad;
	//The quadrantst that a collectible can spawn in 
	public GameObject quad1, quad2, quad3, quad4, quad5, quad6;
	//Stores the quadrants of the platform to spawn the powerups in
	private GameObject[] quadrants;
	//Stores where the random number will fetch an index
	private int collectibleIndex;
	//Stores the number of the current quadrant that is about to be spawned
	private int currentSpawningQuadNum;
	//The audio source of the ice break sound effect
	public AudioSource iceBreakSource;
	//The ice break sound effect
	public AudioClip iceBreakSE;
	//Controls whether the player has been affected by the knockbackCollectible (they can be pushed farther back)
	[HideInInspector]
	public bool knockable;
	//The value of the knockback the players experience on collision
	public float knockback;
	//Controls how long the player is knockable for
	public float timeKnocked;
	//Used to set the knockback time back after it is depleted
	[HideInInspector]
	public float setTimeKnocked;
	//The color the players are set to when they are knockable (have increased knockback)
	public Material knockedColor;
	//Makes it so there are only frozen collectibles (mainly used for testing)
	public bool onlyFrozen;
	//Makes it so there are only inverted collectibles (mainly used for testing)
	public bool onlyInverted;
	//Makes it so there are only inverted collectibles (mainly used for testing)
	public bool onlyKnockback;
	//The AudioSource for the knocked sound effect
	public AudioSource knockedSource;
	//The sound effect for when the player is affected by the knocked collectible
	public AudioClip knockedSE;

	void Awake()
	{	
		active = true;
		paused = false;
		rBody = GetComponent<Rigidbody>();
		grounded = true;
		transform = GetComponent<Transform>();
		position = transform.position;
		p2Script = player2.GetComponent<PlayerScript2>();
		scoreText.text = "Score: " + score;
		winText.text = "";
		pauseButton.SetActive(true);
		setTimeAtStart = timeAtStart;
		normalAMusicVolume = arenaMusicSource.volume;
		//Checks whether the current scene is level 2
		isLvl2 = SceneManager.GetActiveScene().name == "Lvl2";	
		//The following will only happen during the second level
		if(isLvl2)
		{
			pRenderer = GetComponent<Renderer>();
			setTimeFrozen = timeFrozen;
			yValue = frozenCollectible.GetComponent<Transform>().position.y;
			setTimeInverted = timeInverted;
			setTimeKnocked = timeKnocked;
			quadrants = new GameObject[] {quad1, quad2, quad3, quad4, quad5, quad6};
		}
		
	}
	void Update()
	{	
		if(active)
		{		
			if(!inverted)
			{
				moveHorizontal = Input.GetAxis("Horizontal");	
				moveVertical = Input.GetAxis("Vertical");
			}
			else
				Invert();
			if(numOfCollectibles > 0)
				SpawnCollectible();
			if(iTextTimer >= 0)
				iTextTimer -= Time.deltaTime;
			else
			{
				redInstructions.text = "";
				purpleInstructions.text = "";
			}				
			movement = new Vector3(moveHorizontal, 0.0f, moveVertical);	//Vector3s deal with movement in 3D space.  X, Y, and Z aspects.  In this case the Y is zero.  Vector3s take floats.	
			//rBody.AddForce(movement * speed * Time.deltaTime); //This accesses the rigidbody component and adds force ot get it moving
			acceleration = movement * speed;
			rBody.velocity += acceleration * Time.deltaTime;
			//rBody.velocity = movement * speed * Time.deltaTime;
			//This will make the player not able to jump if they are affected by the knocked powerup
			if(Input.GetKey(KeyCode.Space) && grounded && !knockable)
				Jump();	
			if(knockable)
			{
				Knocked();
			}
		}
		else if(frozen)
			Freeze();
		else if(respawned)
			PlayersDontMove();
		else
		{
			rBody.velocity = new Vector3(0f, 0f, 0f);
			if( (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R)) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.R)))
				RestartLevel();
				
		}
		if(score == maxScore || p2Script.score == maxScore)
			GameOver();
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
		{
			if(paused)
				ResumeGame();
			else
				PauseGame();
			paused = !paused;
		}
		/*if(paused && Input.GetKeyDown(KeyCode.Escape))
			ResumeGame();
		else if(!paused && Input.GetKeyDown(KeyCode.Escape))
		{
			Debug.Log("ESCAPE KEYCODE1 WORKS");
			PauseGame();
			Debug.Log("PauseMenuActive: " + pauseMenu.activeSelf);
		} */
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
		{
			
			FallOff();	
		}				
		else if(c.gameObject.tag == "Player" && (frozen || p2Script.frozen))
		{	
			iceBreakSource.PlayOneShot(iceBreakSE);
			Unfreeze();	
		}
		else if(c.gameObject.tag == "Player")
		{
			cSource.PlayOneShot(collisionSE);	
		}
		if(c.gameObject.tag == "Player" && p2Script.knockable)
		{
			p2Script.ApplyKnockback();
		}
	}
	private void FallOff()
	{
		active = false;
		p2Script.active = false;
		respawned = true;
		//This will only happen on level2 as level1 does not have powerups
		if(isLvl2)
		{
			if(p2Script.frozen)
				p2Script.Unfreeze();
			else if(frozen)
				Unfreeze();
			if(p2Script.inverted)
				p2Script.Revert();	
			else if(inverted)
				Revert();
			else if(knockable)
				ResetKnocked();
			else if(p2Script.knockable)
				p2Script.ResetKnocked();
							
		}	
		transform.position = position;
		p2Script.transform.position = p2Script.position;
		rBody.velocity = new Vector3(0f, 0f, 0f);
		p2Script.rBody.velocity = new Vector3(0f, 0f, 0f);
		p2Script.score++;
		p2Script.scoreText.text = "Score: " + p2Script.score;
		platform.GetComponent<PlatformScript>().resetPlatform();
		
	}
	private void GameOver()
	{
		gameOver = true;
		active = false;
		p2Script.active = false;		
		restartButton.SetActive(true);
		menuButton.SetActive(true);
		pauseButton.SetActive(false);
		if(currentCollectible != null)
			currentCollectible.SetActive(false);
		if(score > p2Script.score )
			winText.text = "Red wins! Play again?";
		else
			winText.text = "Purple wins! Play again?";
	}
	public void RestartLevel()
	{
		//Makes sure the game is no longer paused when loading in from another level
		Time.timeScale = 1;
		clickSource.PlayOneShot(clickSE);
		if(SceneManager.GetActiveScene().name == "Lvl2")
			SceneManager.LoadScene("Lvl2");	
		else
			SceneManager.LoadScene("Lvl1");			
	}
	public void BackToMenu()
	{
		//Makes sure the game is no longer paused when loading in from another level
		Time.timeScale = 1;
		clickSource.PlayOneShot(clickSE);
		SceneManager.LoadScene("Menu");
	}
	public void PauseGame()
	{
		//Makes the current volume 1 tenth of what it was 
		arenaMusicSource.volume *= .1f;
		clickSource.PlayOneShot(clickSE);
		Time.timeScale = 0;
		active = false;
		pauseButton.SetActive(false);
		pauseMenu.SetActive(true);
	}
	public void ResumeGame()
	{
		arenaMusicSource.volume = normalAMusicVolume;
		clickSource.PlayOneShot(clickSE);
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
		pauseButton.SetActive(true);
		active = true;
	}
	void OnTriggerEnter(Collider c){
		//If this player touches the freezeCollectible, then the other player will not be able to move
        if(c.gameObject.tag == "freezeCollectible")
		{
			p2Script.active = false;
			p2Script.frozen = true;
		}    
		else if(c.gameObject.tag == "invertedCollectible")
		{
			p2Script.inverted = true;
		}
		else if(c.gameObject.tag == "knockbackCollectible")
			p2Script.knockable = true;
    }
	public void Freeze()
	{
		if(!played)
		{
			fSource.PlayOneShot(freezeSE);
			//AudioSource.PlayClipAtPoint(freezeSE, transform.position);
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
	public void SpawnCollectible()
	{
		if(collectibleSpawnTime == 0)
			collectibleSpawnTime = Random.Range(3, 10);		
		if(collectibleSpawnTime > 0)
		{
			collectibleSpawnTime -= Time.deltaTime;
		}	
		else
		{
			//Determines whether it is the first time the variable is being spawned (by checking if the currentCollectible value has been set yet)
			//This happens because you do not want a nullReferenceException when accessing its script
			bool firstTime = currentCollectible == null;
			if(firstTime)
				SetCollectible();
			else
			{
				cScript = currentCollectible.GetComponent<Rotator>();
				//If the player has collected the collectible, then it can change from frozen to inverted (this prevents the collectible from switching)
				if(cScript.collected)
				{
					SetCollectible();
					cScript.collected = false;
				}					
			}
			SetSpawn();
			
			//Use the random function to get a random index
			//Set the spawningCollectible variable to the index of that array
			//Check if the quad number is equal to the quadrant from the player 1 or player2, and if it is, then restart the function [recursion]

			//int quadNumber = int.Parse(quad1.name.Substring(quad1.name.Length - 1));
			//currentCollectible.GetComponent<Transform>().position = new Vector3(Random.Range(minXValue, maxXValue), yValue, Random.Range(minZValue, maxZValue));
			currentCollectible.GetComponent<Transform>().position = new Vector3(spawningXVal, yValue, spawningZVal);
			currentCollectible.SetActive(true);

			numOfCollectibles--;
			collectibleSpawnTime = Random.Range(3, 10);
		}		
	}
	
	public void SetCollectible()
	{
		int collectibleNum = 5;
		//Will determine if none of the testing options are checked, and then it will randomize the collectible
		if(!onlyFrozen && !onlyInverted && !onlyKnockback)
		{
			collectibleNum = Random.Range(0, 3);
		}
		else if(onlyFrozen)
			collectibleNum = 0;
		else if(onlyInverted)
			collectibleNum = 1;
		else if(onlyKnockback)
			collectibleNum = 2;
		if(collectibleNum == 0)
			currentCollectible = frozenCollectible;
		else if (collectibleNum == 1)
			currentCollectible = invertedCollectible;
		else
			currentCollectible = knockbackCollectible;		
	}
	public void Invert()
	{
		if(!played)
		{
			iSource.PlayOneShot(invertedSE);
			//AudioSource.PlayClipAtPoint(invertedSE, platform.GetComponent<Transform>().position);
			played = true;
		}
		
		moveHorizontal = Input.GetAxis("InvHorizontal");		
		moveVertical = Input.GetAxis("InvVertical"); 
		pRenderer.material = invertedColor;
		if(timeInverted > 0)
		{
			timeInverted -= Time.deltaTime;
		}
		else
			Revert();
	}
	//This will make the controls no longer inverted
	public void Revert()
	{
		played = false;
		inverted = false;
		timeInverted = setTimeInverted;
		pRenderer.material = startingColor;
	}
	public void PlayersDontMove()
	{
		if(timeAtStart > 0)
		{
			rBody.velocity = new Vector3(0f, 0f, 0f);
			p2Script.rBody.velocity = new Vector3(0f, 0f, 0f);
			timeAtStart -= Time.deltaTime;
		}		
		else
		{
			active = true;
			p2Script.active = true;
			respawned = false;
			timeAtStart = setTimeAtStart;
		}
	}
	public void SetSpawn()
	{
		//Debug.Log("SETSPAWN IS WORKING!");
		//Use the random function to get a random index
		//Set the spawningQuad variable to the index of that array
		//Check if the quad number is equal to the quadrant from the player 1 or player2, and if it is, then restart the function [recursion]
		//If it is not, then set the spawning x and y values
		collectibleIndex = Random.Range(0, 6);
		spawningQuad = quadrants[collectibleIndex];
		currentSpawningQuadNum = int.Parse(spawningQuad.name.Substring(quad1.name.Length - 1));
		if(currentSpawningQuadNum == quadrant || currentSpawningQuadNum == p2Script.quadrant)
			SetSpawn();
		else
		{
			spawningXVal = spawningQuad.transform.position.x;
			spawningZVal = spawningQuad.transform.position.z;
		}
			
	}
	public void ApplyKnockback()
	{
		//Sets the force in the direction that the other player is facing, and is multipled by the float set in the inspector
		Vector3 knockbackForce = p2Script.rBody.transform.forward * knockback;
		rBody.AddForce(knockbackForce);
	}
	public void Knocked()
	{
		//At the very start, change the player's color to black (if the timeKnocked is the same as the setTime, 
		//then it will be the start of the method. This will prevent it from being called from the Update() function multiple times)
		if(timeKnocked == setTimeKnocked)
		{
			knockedSource.PlayOneShot(knockedSE);
			pRenderer.material = knockedColor;
		}
		if(timeKnocked > 0)
		{
			timeKnocked -= Time.deltaTime;
		}
		//After the timer is over, the player will no longer be "knockable"
		else
			ResetKnocked();
	}
	//This will reset the effects of the knockback powerup
	public void ResetKnocked()
	{
		timeKnocked = setTimeKnocked;
		pRenderer.material = startingColor;
		knockable = false;		
	}
}