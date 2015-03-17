using UnityEngine;
using System.Collections;

public class Player :  MovingObject 
{
	public int WallDamage = 1;
	public int PointsPerFood = 10;
	public int PointsPerSoda = 20;
	public float RestartLevelDelay = 1f;
	
	private Animator animator;
	private int food;
	
	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator>();
		
		food = GameManager.Instance.PlayerFoodPoints;
		
		base.Start();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!GameManager.Instance.PlayerTurn)
		{
			return;
		}
		
		int horizontal = 0;
		int vertical = 0;
		
		horizontal = (int) Input.GetAxisRaw("Horizontal");
		vertical = (int) Input.GetAxisRaw("Vertical");
		
		if(horizontal != 0)
		{
			vertical = 0;
		}
		
		if(horizontal != 0 && vertical != 0)
		{
			AttemptMove<Wall>(horizontal, vertical);
		}
	}
	
	private void OnDisable()
	{
		GameManager.Instance.PlayerFoodPoints = food;
	}
	
	private void CheckIfGameOver()
	{
		if(food <= 0)
		{
			GameManager.Instance.GameOver();
		}
	}
	
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		food --;
		
		base.AttemptMove <T> (xDir,yDir);
		
		RaycastHit2D hit;
		
		if(Move(xDir, yDir, out hit))
		{
		
		}
		
		CheckIfGameOver();
		
		GameManager.Instance.PlayerTurn = false;
	}
	
	protected override void OnCantMove <T> (T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall(WallDamage);
		animator.SetTrigger("playerChop");
	}
	
	private void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
	
	private void LoseFood(int loss)
	{
		animator.SetTrigger("playerHit");
		food -= loss;
		CheckIfGameOver();
	}
	
	private void OnTriggerEnter2D (Collider2D other)
	{
		if(other.tag == "Exit")
		{
			Invoke("Restart", RestartLevelDelay);
			enabled = false;
		}
		else if(other.tag == "Food")
		{
			food += PointsPerFood;
			other.gameObject.SetActive(false);
		}
		else if(other.tag == "Soda")
		{
			food += PointsPerSoda;
			other.gameObject.SetActive(false);
		}
	}
	
}
