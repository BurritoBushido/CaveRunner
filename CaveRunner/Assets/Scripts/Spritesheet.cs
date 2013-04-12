//NOTE 
//origin is bottom left and the direction is torwards top right
using UnityEngine;
using System.Collections;

public class Spritesheet : MonoBehaviour {
	
	public int collumns;
	public int rows;
	
	public int sheetWidth;
	public int sheetHeight;
	int frameCount;
	
	int spriteWidth;
	int spriteHeight;
	
	public float spriteU {get; set;}
	public float spriteV {get; set;}
	
	public int startAnimIndex, endAnimIndex;
	
	public bool isAnimating {get; set;}
	
	int frameIndex;
	
	void Awake()
	{
		spriteWidth = sheetWidth/collumns;	
		spriteHeight = sheetHeight/rows;	
		
		spriteU = (float)spriteWidth/sheetWidth;
		spriteV = (float)spriteHeight/sheetHeight;
		
		frameCount = collumns * rows;
		
		isAnimating = false;
	
		renderer.material.SetTextureScale("_MainTex", new Vector2(spriteU, spriteV));
	}
	
	public void SetFrame(int _Index)
	{
		int rowIndex = _Index/collumns;
		int colIndex = _Index - rowIndex * collumns;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(spriteU * colIndex, spriteV * (rowIndex)));
	}
	
	void Start () {
		SetFrame(0);
	}
	
	public void PlayAnimation(int _StartIndex, int _EndIndex, bool _IsLooping)
	{
		startAnimIndex = _StartIndex;
		endAnimIndex = _EndIndex;
		
		if(!isAnimating)
			StartCoroutine(CR_Animate());
		else if(frameIndex < startAnimIndex || frameIndex > endAnimIndex)
			frameIndex = startAnimIndex;
	}
	
	public void StopCurrentAnimation()
	{
		isAnimating = false;
		StopCoroutine("CR_Animate");
	}
	
	IEnumerator CR_Animate()
	{
		frameIndex = startAnimIndex;
		isAnimating = true;
		while(isAnimating)
		{
			frameIndex++;
			SetFrame(frameIndex);
				
			if(frameIndex == endAnimIndex)
				frameIndex = startAnimIndex;
			
			yield return new WaitForSeconds(.1f);
		}
	}
}


