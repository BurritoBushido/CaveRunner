using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	Spritesheet sprite;
	public float speed = 100;
	public float riseRate = 4;	//y movement for CR_ChasePlayer
	public float destroySelfDistance = 200;	//distance from player
	
	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<Spritesheet>();
		sprite.PlayAnimation(0, 2, true);
		
		StartCoroutine(CR_ChasePlayer());
		StartCoroutine(CR_CheckDeath());
		Physics.IgnoreLayerCollision(8, 10);
	}
	
	//Rotates torwards character
	/*
	IEnumerator CR_ChasePlayer()
	{
		Vector3 direction;
		Vector3 toPlayerVector;
		Transform player = GameObject.Find("Player").transform;
		
		//Updates direction one per 4 frames
		while(true)
		{
			toPlayerVector = (player.position - transform.position);
			direction = toPlayerVector.normalized;
			if(direction.x > 0)															//ensures always going left
			{
				direction = new Vector3(-direction.x, direction.y, direction.z);
				if(Vector3.Dot(toPlayerVector, toPlayerVector) > 	200*200)		//checks distance from player
				{
					Destroy(gameObject);
				}
			}
			float angle = Vector3.Angle(transform.right, direction);

			if(direction.y < transform.right.y)
				transform.RotateAroundLocal(transform.up, -angle * .5f* Time.deltaTime);
			else
				transform.RotateAroundLocal(transform.up, angle * .5f* Time.deltaTime);

		
			transform.position += transform.right * speed * Time.deltaTime;
			yield return 0;
			
			transform.position += transform.right * speed * Time.deltaTime;
			yield return 0;			
			
			transform.position += transform.right * speed * Time.deltaTime;
			yield return 0;
			
			transform.position += transform.right * speed * Time.deltaTime;
			yield return 0;
		}
	}
	*/
	
	//No rotation, just moves horizontally and updates y
	IEnumerator CR_ChasePlayer()
	{
		Transform player = GameObject.Find("Player").transform;
		
		//Updates direction one per 4 frames
		while(true)
		{
			if(transform.position .x > player.position.y)
			{
				if(transform.position.y < player.position.y)
				{
					transform.position += Vector3.up  * Mathf.Abs(transform.position.y - player.position.y)* Time.deltaTime * riseRate;
				}
				else
					transform.position -= Vector3.up *(transform.position.y - player.position.y)  * Time.deltaTime * riseRate;
			}
			transform.position -= Vector3.right * speed * Time.deltaTime;
			yield return 0;
		}
	}
	
	IEnumerator CR_CheckDeath()
	{
		Vector3 toPlayerVector;
		Transform player = GameObject.Find("Player").transform;
		while(true)
		{
			toPlayerVector = (player.position - transform.position);
			if(transform.position.x - player.position.x < -200)		//checks distance from player
				Destroy(gameObject);
			
			yield return 0;
		}
	}

	public void OnHit()
	{
		collider.enabled = false;	
	}
}
