using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour 
{
	public GameObject GameManagerPrefab;
	
	// Use this for initialization
	void Awake () 
	{
		if(GameManager.Instance == null)
		{
			Instantiate(GameManagerPrefab);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
