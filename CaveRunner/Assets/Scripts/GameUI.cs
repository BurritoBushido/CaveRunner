using UnityEngine;
using System.Collections;

public class GameUI : MonoBehaviour {

	public GUIText distance, best;
	// Use this for initialization
	void Start () {
		StartCoroutine(CR_CalculateDistance());
		best.text = "best: " + GameObject.Find("Highscore").GetComponent<Highscore>().GetHighestScore();
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator CR_CalculateDistance()
	{
		Transform player = GameObject.Find("Player").transform;
		while(true)
		{
			distance.text = Mathf.CeilToInt(player.position.x).ToString();
			yield return 0;
		}
	}
}
