using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour 
{
	[Serializable]
	public class Count
	{
		public int Minimum;
		public int Maximum;
		
		public Count (int min, int max)
		{
			Minimum = min;
			Maximum = max;
		}
	}
	
	public int Columns = 8;
	public int Rows = 8;
	public Count WallCount = new Count (5,9);
	public Count FoodCount = new Count (1,5);
	public GameObject Exit;
	public GameObject[] FloorTiles;
	public GameObject[] WallTiles;
	public GameObject[] FoodTiles;
	public GameObject[] EnemyTiles;
	public GameObject[] OuterWallTiles;
	
	private Transform boardHolder;
	private List<Vector3> gridpositions = new List<Vector3>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetUpScene(int level)
	{
		BoardSetup();
		InitialiseList();
		LayoutObjectAtRandom(WallTiles, WallCount.Minimum, WallCount.Maximum);
		LayoutObjectAtRandom(FoodTiles, FoodCount.Minimum, FoodCount.Maximum);
		int enemyCount = (int)Mathf.Log(level, 2f);
		LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
		Instantiate(Exit, new Vector3(Columns -1, Rows -1, 0f), Quaternion.identity);
		
	}
	
	
	void InitialiseList()
	{
		gridpositions.Clear();
		
		for(int x = 1; x < Columns - 1; x++)
		{
			for(int y = 1; y < Rows -1; y++)
			{
				gridpositions.Add(new Vector3(x, y, 0f));
			}
		}
	}
	
	void BoardSetup()
	{
		boardHolder = new GameObject ("Board").transform;
		
		for(int x = -1; x < Columns + 1; x++)
		{
			for(int y = -1; y < Rows + 1; y++)
			{
				GameObject toInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
				if(x == -1 || x == Columns || y == -1 || y == Rows)
				{
					toInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];
				}
				
				GameObject instance = Instantiate(toInstantiate, new Vector3(x,y,0f), Quaternion.identity) as GameObject;
				
				instance.transform.SetParent(boardHolder);
			}
		}
	}
	
	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range(0, gridpositions.Count);
		Vector3 randomPosition = gridpositions[randomIndex];
		gridpositions.RemoveAt(randomIndex);
		return randomPosition;
	}
	
	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range(minimum, maximum+1);
		
		for(int i = 0; i < objectCount; i++)
		{
			Vector3 randomPosition = RandomPosition();
			GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}
}
