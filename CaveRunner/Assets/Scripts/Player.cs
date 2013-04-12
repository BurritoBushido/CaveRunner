//uncomment below to make player auto run.
#define AUTO_RUN  
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	const int IDLE = 0;
	const int RUN = 1;
	const int JUMP = 2;
	const int FALL = 3;
	
	Spritesheet sprite;
	PlayerMovement movement;
	int idleFrameIndex = -1;
	int state;
	
	bool isInvunerable = false;
	Renderer blinker;
	
	void Awake()
	{
		sprite = GetComponent<Spritesheet>();	
		movement = GetComponent<PlayerMovement>();
		blinker = GameObject.Find("Blinker").renderer;
		
		NotificationCenter.AddObserver(this, "StartFalling");
		NotificationCenter.AddObserver(this, "Landed");
	}
	void Start () 
	{
		
		idleFrameIndex = 10;
		StartCoroutine(CR_CheckFallDeath());
	}

	// Update is called once per frame
	void Update () 
	{
		#if UNITY_IPHONE
			HandleMobileInput();
		#else
			HandleInput();
		#endif
	}
	
	void JumpPress()
	{
		movement.isJumpPressed = true;
		if(movement.isGrounded)
		{
			movement.Jump();
			state = JUMP;
			StartJumpAnim();
			print("JUMP" + state);
		}	
	}
	
	void JumpRelease()
	{
		movement.isJumpPressed = false;
	}
	
	void BlinkPress()
	{
		blinker.renderer.enabled = true;
		isInvunerable = true;	
	}
	
	void BlinkRelease()
	{
		isInvunerable = false;
		blinker.renderer.enabled = false;
	}
	
	void HandleMobileInput()
	{
  		foreach (Touch touch in Input.touches) 
		{
			
			
			if(touch.position.x < Screen.width * .5f)	//Left side of screen handles jump
			{
            	if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					JumpRelease();
				else
					JumpPress();
			}
			else //right side handles blink
			{ 
            	if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					BlinkRelease();
				else
					BlinkPress();
			}
            
        }
	}
	
	void HandleInput()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			BlinkPress();
		}
		else if(Input.GetKeyUp(KeyCode.Space))
		{
			BlinkRelease();
		}
		
		if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			JumpRelease();
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			JumpPress();
		}
#if AUTO_RUN
		movement.MoveRight();
		
		//if(movement.isGrounded)
		//	StartWalkCycle();
		///else
		//	StartJumpAnim();

		switch(state)
		{
		case IDLE:
			state = RUN;
			StartWalkCycle();
			break;
		case JUMP:
			StartJumpAnim();
			break;
		case FALL:
			StartFalling();
			break;
		}	
#else
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			movement.MoveLeft();
			
			switch(state)
			{
			case IDLE:
				state = RUN;
				StartWalkCycle();
				break;
			case JUMP:
				StartJumpAnim();
				break;
			case FALL:
				StartFalling();
				break;
			}
		}
		else if(Input.GetKey(KeyCode.RightArrow))
		{
			movement.MoveRight();
			
			//if(movement.isGrounded)
			//	StartWalkCycle();
			///else
			//	StartJumpAnim();
	
			switch(state)
			{
			case IDLE:
				state = RUN;
				StartWalkCycle();
				break;
			case JUMP:
				StartJumpAnim();
				break;
			case FALL:
				StartFalling();
				break;
			}
		}
		
		else
		{
			movement.Idle();
			//sprite.isAnimating = false;
			//if(movement.isGrounded)
			//	sprite.SetFrame(idleFrameIndex);	
			
			switch(state)
			{
			default:
				state = IDLE;
				sprite.SetFrame(idleFrameIndex);	
				sprite.isAnimating = false;
				break;
			case JUMP:
				StartJumpAnim();
				break;
			case FALL:
				StartFalling();
				break;
			}
		}
#endif
	}
	
	void StartJumpAnim()
	{
		sprite.StopCurrentAnimation();
		sprite.SetFrame(14);	
	}
	
	void StartFalling()
	{
		state = FALL;
		sprite.SetFrame(18);	
		sprite.StopCurrentAnimation();
	}
	
	void Landed()
	{
		sprite.SetFrame(idleFrameIndex);	
		state = IDLE;
	}
	
	void StartWalkCycle()
	{
		sprite.PlayAnimation(19, 29, true);
	}
	
	
	IEnumerator CR_CheckFallDeath()
	{
		while(true)
		{
			if(transform.position.y < -200)
			{
				Hashtable data = new Hashtable();
				data["Score"] = transform.position.x;
				NotificationCenter.PostNotification(this, "CheckForHighscore", data);
				Application.LoadLevel(0);
				//transform.position = new Vector3(0, 20, 0);	
			}
			yield return new WaitForSeconds(1.0f);
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if(isInvunerable)
			return;
		
		movement.HitByMonster();
		//print("Collided with " + collider.transform.name);	
	}
}
