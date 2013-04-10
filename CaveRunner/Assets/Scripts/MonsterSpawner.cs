using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	
	public GameObject prefabMonster;
	public float spawnIntervals = 3.0f;
	
	Transform player;
	float halfScreenHeight = 75;
	
	void Awake()
	{
		player = GameObject.Find("Player").transform;
	}
	// Use this for initialization
	void Start () {
		StartCoroutine(CR_SpawnMonsters());
	}
	
	IEnumerator CR_SpawnMonsters()
	{
		float currentPlayerY;
		float spawnY = -1;
		while(true)
		{
			currentPlayerY = player.position.y;
			spawnY = Random.Range(currentPlayerY - halfScreenHeight, currentPlayerY + halfScreenHeight);
			GameObject newMonster = Instantiate(prefabMonster) as GameObject;
			newMonster.transform.position = new Vector3(player.position.x + 200, spawnY, prefabMonster.transform.position.z);
			yield return new WaitForSeconds(spawnIntervals);
		}
		
	}

}
