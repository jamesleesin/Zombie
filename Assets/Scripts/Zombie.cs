using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Zombies have a box collider at their feet to collide with the ground, and objects at their feet. 
They have seperate colliders for the body, and a capsule collider for the head. 
Headshots do 2.5x damage compared to body.
*/
public class Zombie : MonoBehaviour {
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
	
	private bool sleeping = false;
	
	private bool dead = false;
	private float deathTimer = 10f;
	

	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		animator = GetComponent<Animator>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		newTargetTimer = timeUntilNewTarget;
		animator.SetBool("Sleeping", false);		
	}
	
	// initialize with more health based on wave
	public void Initialize(int wave){
		hp = 75f + wave * 20f;
	}
	
	// Update is called once per frame
	void Update () {
		if (sleeping == false){
			if (Input.GetKey(KeyCode.Q)){
				ZombieMovement();
			}
			
			if (newTargetTimer > 0f && target == null){
				newTargetTimer -= Time.deltaTime;
			}
			if (newTargetTimer <= 0f){
				newTargetTimer = timeUntilNewTarget;
				ZombieMovement();
			}
			
			// if the player gets too close then zombie targets them
			if (Vector3.Distance(transform.position, player.transform.position) < 25f){
				if (followPlayer == false){
					followPlayer = true;
					target = player;
					followPlayerTimer = followPlayerMaxTime;
				}

				if (Vector3.Distance(transform.position, player.transform.position) < 6f){
					animator.ResetTrigger("Attack");
					animator.SetTrigger("Attack");
				}
			}
			else{
				followPlayer = false;
				target = null;
			}
		}
		
		if (dead){
			transform.GetComponent<Rigidbody>().detectCollisions = false;
			transform.GetComponent<Rigidbody>().isKinematic = true;
			transform.GetComponent<Rigidbody>().useGravity = false;
			deathTimer -= Time.deltaTime;
			transform.GetComponent<Rigidbody>().position = new Vector3(transform.position.x, transform.position.y - Time.deltaTime/9f, transform.position.z);
			if (deathTimer <= 0f){
				Destroy(this.gameObject);
			}
		}
		else{
			if (followPlayer && target != null){
				agent.SetDestination(target.transform.position);
				if (followPlayerTimer > 0f){
					followPlayerTimer -= Time.deltaTime;
					if (followPlayerTimer <= 0f){
						followPlayerTimer = followPlayerMaxTime;
						followPlayer = false;
					}
				}
			}
		}
	}
	
	// Headshots = 2.5x damage
	public void TakeDamage(float damage){
		Debug.Log("take " + damage + " damage");
		agent.enabled = true;
		animator.SetTrigger("Hit");
		animator.SetBool("Sleeping", false);
		sleeping = false;
		hp -= damage;
		target = player;
		followPlayer = true;
		followPlayerTimer = followPlayerMaxTime;		
		if (hp <= 0f){
			Death();
		}
	}
	
	void Death(){
		animator.SetTrigger("Death");
		transform.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		dead = true;
	}
	
	// calculate where the zombie will move
	Vector3 ZombieMovement(){
		// map size is from -100 to 100 for x and z
		Vector3 targetLoc = new Vector3(Random.Range(-100f, 100f), 0.5f, Random.Range(-100f, 100f));
        agent.SetDestination(targetLoc);
		Debug.Log("Set destination to" + targetLoc);
		return targetLoc;
	}
	
	
	
	// movement{
	void FixedUpdate(){
		
		if (sleeping == false && !dead){
			// look at player if close enough and targetted, otherwise look forward
			if (target != null)
			{
				Vector3 pos = target.transform.position - transform.position;
				targetRot = Quaternion.LookRotation(pos);
			}
			else
			{
				targetRot = Quaternion.LookRotation(transform.forward);
			}
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.05f);
			
		}
	
	}
	
	
}
