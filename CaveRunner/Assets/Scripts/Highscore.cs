using UnityEngine;
using System.Collections;

public class Highscore : MonoBehaviour {
	
	
	void Awake()
	{
		NotificationCenter.AddObserver(this, "CheckForHighscore");	
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void CheckForHighscore(Notification notif)
	{
		print("CHECK HIGH SCORE");
		int currentScore = System.Convert.ToInt32(notif.data["Score"]);
		if(currentScore > PlayerPrefs.GetInt("Highscore", 0))
		{
			PlayerPrefs.SetInt("Highscore", currentScore);
		}	
	}
	
	public int GetHighestScore()
	{
		return PlayerPrefs.GetInt("Highscore", 0);	
	}
}
