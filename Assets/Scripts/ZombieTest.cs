using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Zombies have a box collider at their feet to collide with the ground, and objects at their feet. 
They have seperate colliders for the body, and a capsule collider for the head. 
Headshots do 2.5x damage compared to body.
*/
public class ZombieTest : MonoBehaviour {
	private float hp;
	private GameObject player;
	private GameObject target;
	
	private Animator animator;
	
	UnityEngine.AI.NavMeshAgent agent;
	private Vector3 targetLocation;
	
	private const float timeUntilNewTarget = 10f;
	private float newTargetTimer = 0f;
	
	private bool followPlayer = false;
	private const float followPlayerMaxTime = 20f;
	private float followPlayerTimer = 0f;
	
	Quaternion targetRot;
	
	private bool sleeping = true;
	

	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		newTargetTimer = timeUntilNewTarget;
		
		Vector3 targetLoc = new Vector3(transform.position.x, 0, transform.position.z + 175f);
        agent.SetDestination(targetLoc);
	}
	
	
	// Update is called once per frame
	void Update () {
	
	
	}
	
	
	// movement{
	void FixedUpdate(){
		
	}
	
	
}
