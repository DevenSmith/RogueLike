using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance = null;
	public BoardManager BoardScript;
	public int PlayerFoodPoints = 100;
	[HideInInspector] public bool PlayerTurn = true;
	
	private int level = 3;
	
	void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
		}
		else if(Instance != this)
		{
			Destroy(gameObject);
		}
	
		DontDestroyOnLoad(gameObject);
	
		BoardScript = GetComponent<BoardManager>();
		InitGame();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void InitGame()
	{
		BoardScript.SetUpScene(level);
	}
	
	public void GameOver()
	{
		enabled = false;
	}
}
