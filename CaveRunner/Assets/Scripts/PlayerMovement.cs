using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float MAX_SPEED = 50f;
	public float speedX = 0.0f;
	public float speedY = 0.0f;
	public float acceleration = 80;
	public float friction = .1f;
	public float airFriciton = .5f;
	public float gravity = 100;
	public float jumpSpeed = 80.0f;
	
	public bool isGrounded {get { return isCollidingDown;}}
	
	public float physicScale = 2.0f;	//scales physics easily
		
	Vector3 avgNormal = Vector3.zero;
	Vector3 avgPoint = Vector3.zero;
	
	bool isCollidingUp = false;
	bool isCollidingDown = false;
	bool isCollidingLeft = false;
	bool isCollidingRight = false;
	
	// left, right, top, bottom ... bits
	int collisionIndex = 0;
	
	// Use this for initialization
	void Start () {
		
		MAX_SPEED *= physicScale;
		acceleration *= physicScale;
		friction *= physicScale;
		airFriciton *= physicScale;
		gravity *= physicScale;
		jumpSpeed *= physicScale;
	}
	
	public void Jump()
	{
		//print("Jump " + isCollidingDown);
		if(isCollidingDown)
		{
			isCollidingDown = false;
			speedY += jumpSpeed;
			collisionIndex &= ~8;
			
			StartCoroutine(CR_Jump());
		}
	}
	
	IEnumerator CR_Jump()
	{
		while(speedY > 0)
		{
			yield return 0;	
		}
		NotificationCenter.PostNotification(this, "StartFalling");
		
		while(!isCollidingDown)
		{
			yield return 0;
		}
		NotificationCenter.PostNotification(this, "Landed");
	}

	public void Idle()
	{
		//Apply friction and slow down player
		if(isCollidingDown)
			speedX -= speedX * friction *  Time.deltaTime;
		else
			speedX -= speedX * airFriciton *  Time.deltaTime;
	}

	public void MoveRight()
	{
		//print("Colliding Right? " + isCollidingRight);
		if(isCollidingRight)
			return;
		
		speedX +=  acceleration * Time.deltaTime;
		if(speedX > MAX_SPEED)
			speedX = MAX_SPEED;
		//print("Moving Right " + speedX);
		
		transform.localScale = new Vector3(1, 1, 1);
	}
	public void MoveLeft()
	{
		//print("Colliding Left? " + isCollidingLeft);
		if(isCollidingLeft)
			return;
		
		speedX -=  acceleration * Time.deltaTime;
		if(speedX < -MAX_SPEED)
			speedX = -MAX_SPEED;
		//print("Moving Left " + speedX);
		
		transform.localScale = new Vector3(-1, 1, 1);
	}
	
	void UpdateCollisionBits(Vector3 _CollisionNormal)
	{
		//print(_CollisionNormal);
		if(_CollisionNormal.x > .9f)
		{
			isCollidingLeft = true;
			collisionIndex |= 1;
			//print("COLLIDING LEFT " + collisionIndex);
		}
		
		if(_CollisionNormal.x < -.9f)
		{
			isCollidingRight = true;
			collisionIndex |= 2;
			//print("COLLIDING RIGHT " + collisionIndex);
		}
		
		if(_CollisionNormal.y < -.9f)
		{
			isCollidingUp = true;
			collisionIndex |= 4;
		}
		
		if(_CollisionNormal.y > .9f)
		{
			//if(!isCollidingDown)
			//	NotificationCenter.PostNotification(this, "Landed");
			
			isCollidingDown = true;
			collisionIndex |= 8;
			//print("COLLIDING DOWN " + collisionIndex);
		}
	}
	
	void HandleCollision(Collision collision)
	{
		int contactPtCount = collision.contacts.Length;
		avgNormal = Vector3.zero;
		avgPoint = Vector3.zero;
		for(int i = 0; i < contactPtCount; i++)
		{
			avgNormal += collision.contacts[i].normal;
			avgPoint += collision.contacts[i].point;
		}		
		avgNormal /= contactPtCount;
		avgPoint /= contactPtCount;
		
		UpdateCollisionBits(avgNormal);
		//print("Collision Index " + collisionIndex);
		
		if(isCollidingLeft && speedX < 0)
		{
			speedX = 0;
			//rigidbody.velocity -= Vector3.right * rigidbody.velocity.x;
		}
		if(isCollidingRight && speedX > 0)
			speedX = 0;
		
		if(isCollidingUp && speedY > 0)
		{
			speedY = 0;
		}
		if(isCollidingDown && speedY < 0)
		{
			speedY = 0;
		}
	
		//Draws collision lines
        //foreach (ContactPoint contact in collision.contacts) {
            //Debug.DrawLine(contact.point, contact.point + contact.normal, Color.green, 2, false);
        //}
	}
	
	void Update()
	{
		speedY -= gravity * Time.deltaTime;	
		rigidbody.velocity = new Vector3(speedX, speedY, 0);
	}

	void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision);
	}
	void OnCollisionStay(Collision collision)
	{
		HandleCollision(collision);
		
	}
	
	void OnCollisionExit(Collision collision)
	{
		//print("Collision Index " + collisionIndex);
		int contactPtCount = collision.contacts.Length;
		avgNormal = Vector3.zero;
		avgPoint = Vector3.zero;
		for(int i = 0; i < contactPtCount; i++)
		{
			avgNormal += collision.contacts[i].normal;
			avgPoint += collision.contacts[i].point;
			//print(collision.contacts[i].normal);
		}		
		avgNormal /= contactPtCount;
		avgPoint /= contactPtCount;
		
		/*
		print("Check Left " + (collisionIndex & 1) + isCollidingLeft);
		print("Check Right " + (collisionIndex & 2));
		print("Check Top " + (collisionIndex & 4));
		print("Check Bottom " + (collisionIndex & 8));
		*/
		
		//Checks which surface we are leaving based off 
		if( (collisionIndex & 1) == 1)
		{
			if(avgNormal.x > .9f)
			{
				isCollidingLeft = false;
				collisionIndex &= ~1;
			}
		}
		if( (collisionIndex & 2) == 2)
		{
			if(avgNormal.x < -.9f)
			{
				isCollidingRight = false;
				collisionIndex &= ~2;
			}
		}
		if( (collisionIndex & 4) == 4)
		{
			if(avgNormal.y < -.9f)
			{
				isCollidingUp = false;
				collisionIndex &= ~4;
			}
		}
		if( (collisionIndex & 8) == 8)
		{
			if(avgNormal.y > .9f)
			{
				isCollidingDown = false;
				collisionIndex &= ~8;
				print("LEAPT OFF " + collisionIndex);
			}
		}
	
	}
}
