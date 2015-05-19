using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player :  MovingObject 
{
	public int WallDamage = 1;
	public int PointsPerFood = 10;
	public int PointsPerSoda = 20;
	public float RestartLevelDelay = 1f;
	public Text FoodText;
	public AudioClip MoveSound1;
	public AudioClip MoveSound2;
	public AudioClip EatSound1;
	public AudioClip EatSound2;
	public AudioClip DrinkSound1;
	public AudioClip DrinkSound2;
	public AudioClip GameOverSound;
	
	
	private Animator animator;
	private int food;
	private Vector2 touchOrigin = -Vector2.one;
	
	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator>();
		
		food = GameManager.Instance.PlayerFoodPoints;
		
		FoodText.text = "Food: " + food;
		
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
		
		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
		
		horizontal = (int) Input.GetAxisRaw("Horizontal");
		vertical = (int) Input.GetAxisRaw("Vertical");
		
		if(horizontal != 0)
		{
			vertical = 0;
		}
		
		#else
		
		if (Input.touchCount > 0)
		{
			Touch myTouch = Input.touches[0];
			
			if(myTouch.phase == TouchPhase.Began)
			{
				touchOrigin = myTouch.position;
			}
			else if(myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
			{
				Vector2 touchEnd = myTouch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin.x = -1;
				
				if(Mathf.Abs(x) > Mathf.Abs(y))
				{
					horizontal = x > 0? 1: -1;
				}
				else
				{
					vertical = y>0? 1: -1;
				}
			}
		}
		
		#endif
		
		if(horizontal != 0 || vertical != 0)
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
			SoundManager.Instance.PlaySingle(GameOverSound);
			SoundManager.Instance.MusicSource.Stop();
			GameManager.Instance.GameOver();
		}
	}
	
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		food --;
		FoodText.text = "Food: " + food;
		
		base.AttemptMove <T> (xDir,yDir);
		
		RaycastHit2D hit;
		
		if(Move(xDir, yDir, out hit))
		{
			SoundManager.Instance.RandomizeSfx(MoveSound1, MoveSound2);
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
	
	public void LoseFood(int loss)
	{
		animator.SetTrigger("playerHit");
		food -= loss;
		FoodText.text = "-" + loss + " Food: " + food;
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
			FoodText.text = "+" + PointsPerFood + " Food: " + food;
			SoundManager.Instance.RandomizeSfx(EatSound1, EatSound2);
			other.gameObject.SetActive(false);
		}
		else if(other.tag == "Soda")
		{
			food += PointsPerSoda;
			FoodText.text = "+" + PointsPerSoda + " Food: " + food;
			SoundManager.Instance.RandomizeSfx(DrinkSound1, DrinkSound2);
			other.gameObject.SetActive(false);
		}
	}
	
}
