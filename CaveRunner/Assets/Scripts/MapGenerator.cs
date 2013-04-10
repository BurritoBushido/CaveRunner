using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour {

	const int MAP_LAYER = 8;
	int maxSegments = 10;
	
	float prevSegmentHeight = 0;
	float farthestX = 0;
	float maxLength = 100;
	float maxHeight = 50;
	float maxDistanceSqrd;
	Transform map;
	
	List<GameObject> segments;
	void Awake()
	{
		segments = new List<GameObject>();
		map = GameObject.Find("Map").transform;	
		maxDistanceSqrd = (maxHeight * maxHeight) + (maxLength * maxLength);
		StartCoroutine(CR_GenerateMap());
		StartCoroutine(CR_DestroySegments());
	}
	void CreateSegment()
	{
		float gapLength = Random.Range(maxLength  * .4f, maxLength);
		float gapHeight = Random.Range(-maxHeight, maxHeight);
		
		float width = Random.Range(80, 160);
		float height = Random.Range(100, 200);
		
		float x = farthestX + gapLength + width * .5f;
		float y = prevSegmentHeight + gapHeight - height * .5f;
		print(y);
		
		GameObject newSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
		newSegment.transform.localScale = new Vector3(width, height, 20);
		newSegment.transform.position = new Vector3(x, y, 0);
		newSegment.transform.parent = map;
		newSegment.layer = MAP_LAYER;
		farthestX = newSegment.collider.bounds.max.x;
		
		prevSegmentHeight = newSegment.collider.bounds.max.y;
		segments.Add(newSegment);
	}
	
	IEnumerator CR_GenerateMap()
	{
		
		while(true)
		{
			if(segments.Count < maxSegments)
				CreateSegment();
			
			yield return 0;
		}
	}
	
	IEnumerator CR_DestroySegments()
	{
		Transform player = GameObject.Find("Player").transform;
		while(true)
		{
			if( (segments[0].collider.bounds.max.x + 200) < player.position.x)
			{
				Destroy(segments[0]);
				segments.RemoveAt(0);
			}
			yield return 0;
		}
	}
	
}
