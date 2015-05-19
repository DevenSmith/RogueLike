using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public float LevelStartDelay = 2f;
	public float turnDelay = .1f;
	public static GameManager Instance = null;
	public BoardManager BoardScript;
	public int PlayerFoodPoints = 100;
	[HideInInspector] public bool PlayerTurn = true;
	
	private Text levelText;
	private GameObject levelImage;
	private int level = 1;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;
	
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
		enemies = new List<Enemy>();
		BoardScript = GetComponent<BoardManager>();
		InitGame();
	}
	
	private void OnLevelWasLoaded(int index)
	{
		level++;
		InitGame();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(PlayerTurn || enemiesMoving || doingSetup)
		{
			return;
		}
		
		StartCoroutine(MoveEnemies());
	}
	
	void InitGame()
	{
		doingSetup = true;
		levelImage = GameObject.Find("LevelImage");
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		levelText.text = "Day " + level;
		levelImage.SetActive(true);
		Invoke("HideLevelImage", LevelStartDelay);
		enemies.Clear();
		BoardScript.SetUpScene(level);
	}
	
	private void HideLevelImage()
	{
		levelImage.SetActive(false);
		doingSetup = false;
	}
	
	public void AddEnemiesToList(Enemy script)
	{
		enemies.Add(script);
	}
	
	public void GameOver()
	{
		levelText.text = "After " + level + " days, you starved";
		levelImage.SetActive(true);
		enabled = false;
	}
	
	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if(enemies.Count == 0)
		{
			yield return new WaitForSeconds(turnDelay);
		}
		
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].MoveTime);
		}
		
		PlayerTurn = true;
		enemiesMoving = false;
	}
}
