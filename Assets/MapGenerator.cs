using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour 
{
	public bool drawGizmos = true;
	public Color activeColor = Color.black;
	public Color deactiveColor = Color.white;

	public int width = 60;
	public int height = 80;
	[Range (0, 100)]
	public int randomFillPercent = 45;
	public string seed = "";
	public bool useRandomSeed;
	public int mapSmoothing = 5;

	int[,] map;

	public int[,] MAP { get { return map; } }

	void Start ()
	{
		GenerateMap ();
	}

	void Update ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			Start ();
		}
	}

	void GenerateMap ()
	{
		map = new int[width, height];
		RandomFillMap ();

		for (int i=0; i<mapSmoothing; i++)
		{
			SmoothMap ();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator> ();
		meshGen.GenerateMesh (map, 1);
	}

	void RandomFillMap ()
	{
		if (useRandomSeed) 
		{
			seed = Time.time.ToString ();		
		}

		System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

		for (int x=0; x<width; x++) 
		{
			for (int y=0; y<height; y++)
			{
				if (x == 0 || x == width-1 || y == 0 || y == height-1)
					map [x, y] = 1;
				else
					map [x, y] = (pseudoRandom.Next (0, 100) < randomFillPercent) ? 1 : 0;
			}
		}
	}

	void SmoothMap ()
	{
		for (int x=0; x<width; x++) {
			for (int y=0; y<height; y++) 
			{
				int neighbourWallTiles = GetSurroundingWallCount (x, y);

				if (neighbourWallTiles > 4)
				{
					map [x, y] = 1;
				}
				else if (neighbourWallTiles < 4)
				{
					map [x, y] = 0;
				}
			}
		}
	}

	int GetSurroundingWallCount (int gridX, int gridY)
	{
		int wallCount = 0;

		for (int neighbourX=gridX - 1; neighbourX <= gridX + 1; neighbourX++) {
			for (int neighbourY=gridY - 1; neighbourY <= gridY + 1; neighbourY++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >=0 && neighbourY < height)
				{
					if (neighbourX != gridX || neighbourY != gridY)
						wallCount += map [neighbourX, neighbourY];
				}
				else
				{
					wallCount++;
				}
			}
		}
		return wallCount;
	}

	void OnDrawGizmos ()
	{
		if (map != null && drawGizmos)
		{
			for (int x=0; x<width; x++) {
				for (int y=0; y<height; y++) {
					Gizmos.color = (map [x, y] == 1) ? activeColor : deactiveColor;
					Vector3 pos = new Vector3 (-width/2 + x + 0.5f, -1f, -height/2 + y + 0.5f);
					Gizmos.DrawCube (pos, Vector3.one);
				}
			}
		}
	}
}
