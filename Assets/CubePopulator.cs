using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MapGenerator))]
public class CubePopulator : MonoBehaviour 
{
	public GameObject wallPfb;
	public GameObject emptyPfb;
	public MapGenerator mapGenerator;

	Transform cubeHolder;

	void Awake ()
	{
		cubeHolder = new GameObject ("CubeHolder").transform;
		mapGenerator = GetComponent <MapGenerator> ();
	}

	void Start ()
	{
		Invoke ("GenerateCubeGrid", 2f);
	}

	void LateUpdate ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			GenerateCubeGrid ();
		}
	}

	public void GenerateCubeGrid ()
	{
		if (mapGenerator.MAP == null || mapGenerator.MAP.Length != mapGenerator.width * mapGenerator.height)
			return;

		int width = mapGenerator.width;
		int height = mapGenerator.height;
		int[,] map = mapGenerator.MAP;

		Destroy (cubeHolder.gameObject);
		cubeHolder = new GameObject ("CubeHolder").transform;
		
		for (int x=0; x<width; x++) 
		{
			for (int y=0; y<height; y++)
			{
				Vector3 pos = new Vector3 (-width/2 + x + 0.5f, -height/2 + y + 0.5f);
				if (map [x, y] == 1 && wallPfb != null) 
				{
					(Instantiate (wallPfb, pos, Quaternion.identity) as GameObject).transform.SetParent (cubeHolder);
				}
				else if (emptyPfb != null)
				{
					(Instantiate (emptyPfb, pos, Quaternion.identity) as GameObject).transform.SetParent (cubeHolder);
				}
			}
		}
	}
}
