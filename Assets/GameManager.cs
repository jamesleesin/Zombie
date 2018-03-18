using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private float gameTime = 0f;
	private int wave;
	
	public Zombie zombiePrefab;
	private List<GameObject> zombies = new List<GameObject>();

	float deltaTime = 0.0f;
	
	GameObject fpsDisplay;
	
	// Use this for initialization
	void Start () {
		fpsDisplay = GameObject.Find("FPS");
		
		wave = 0;
		
		for (int i = 0; i < 5; i++){
			float newX = Random.Range(-20f, 20f);
			float newZ = Random.Range(-20f, 20f);
			float rotY = Random.Range(-180f, 180f);
			float scale = Random.Range(2.7f, 3.2f);
			
			Zombie newZombie = Instantiate(zombiePrefab) as Zombie;
			newZombie.Initialize(wave);
			newZombie.transform.localRotation = Quaternion.Euler(0, rotY, 0);
			newZombie.transform.localScale = new Vector3(scale, scale, scale);
			newZombie.transform.localPosition = new Vector3(newX, 4f, newZ);
			zombies.Add(newZombie.gameObject);
			
		}	
	}
	
	// Update is called once per frame
	void Update () {
		// show fps 
		float msec = Time.unscaledDeltaTime * 1000.0f;
		float fps = 1.0f / Time.unscaledDeltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		fpsDisplay.GetComponent<Text>().text = text;
	}
	
	


}
