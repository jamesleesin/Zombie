using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private float hp = 100f;
	private Animator animator;
	private PlayerController controller;
	private Vector3 movementVector;
	
	private bool aiming = false;
	private bool attacking = false;
	private int currentWeapon = 0;
	private int weaponDamage = 4;
	private GameObject currentGun;
	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		controller = transform.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		// need to keep setting aiming because it keeps going back to false
		if (aiming){
			animator.SetBool("Aiming", true);
		}
		if (attacking){
			if (aiming){
				animator.ResetTrigger("Attack");
				animator.SetTrigger ("Attack");
			}
		}

		// Attack
		if (Input.GetMouseButtonDown(0)){
			// fire or punch
			aiming = true;
			attacking = true;
		}
		if (Input.GetMouseButtonDown(1)){
			// aim
			aiming = true;
		}
		if (Input.GetMouseButtonUp(0)){
			attacking = false;
			animator.ResetTrigger("Attack");
			// if user is holding right click then dont set aiming to false
			if (!Input.GetMouseButton(1)){
				aiming = false;
				animator.SetBool("Aiming", false);
			}
		}
		if (Input.GetMouseButtonUp(1)){
			aiming = false;
			attacking = false;
			animator.SetBool("Aiming", false);
		}
		
		
		// switch weapons
		if (Input.GetKey(KeyCode.Alpha1)){
			controller.SetArsenal("Empty");
			currentWeapon = 0;
			// fists punch twice so lower damage for each
			weaponDamage = 4;
			currentGun = controller.GetGun();
		}
		if (Input.GetKey(KeyCode.Alpha2)){
			controller.SetArsenal("Rifle");
			currentWeapon = 1;
			weaponDamage = 8;
			currentGun = controller.GetGun();
		}
		if (Input.GetKey(KeyCode.Alpha3)){
			controller.SetArsenal("Pistol");
			currentWeapon = 2;
			weaponDamage = 20;
			currentGun = controller.GetGun();
		}
		if (Input.GetKey(KeyCode.Alpha4)){
			controller.SetArsenal("AK47");
			currentWeapon = 3;
			currentGun = controller.GetGun();
		}
	}
	
	// 0 = fists, 1 = rifle, 2 = pistol
	public void FireGun(int gunType){
		if (gunType == 0){
			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			
			RaycastHit hitInfo;
			if (Physics.Raycast(transform.position + new Vector3(0, 4, 0), fwd, out hitInfo, 5f)){
				if (hitInfo.collider.tag == "ZombieBody" || hitInfo.collider.tag == "ZombieHead"){
					Debug.Log("Hit");
					// find the zombie that player hit and subtract damage
					Zombie zombieHit = hitInfo.collider.gameObject.transform.parent.gameObject.GetComponent<Zombie>();
					zombieHit.TakeDamage(weaponDamage);
				}
				else{
					Debug.Log("Miss");
				}
			}
		}
		// hitscan through raycasting
		else if (gunType > 0){
			Vector3 fwd = currentGun.transform.TransformDirection(Vector3.forward);
			Debug.DrawLine(currentGun.transform.position, currentGun.transform.position + fwd * 30f);

			RaycastHit hitInfo;
			if (Physics.Raycast(currentGun.transform.position, fwd, out hitInfo, 30f)){
				if (hitInfo.collider.tag == "ZombieHead")
				{
					Debug.Log("Hit head");
					// find the zombie that player hit and subtract damage
					Zombie zombieHit = hitInfo.collider.gameObject.transform.parent.gameObject.GetComponent<Zombie>();
					zombieHit.TakeDamage(weaponDamage * 2.5f);
				}
				else if (hitInfo.collider.tag == "ZombieBody"){
					Debug.Log("Hit body");
					// find the zombie that player hit and subtract damage
					Zombie zombieHit = hitInfo.collider.gameObject.transform.parent.gameObject.GetComponent<Zombie>();
					zombieHit.TakeDamage(weaponDamage);
				}
				else{
					Debug.Log("Miss");
				}
			}
		}
	}
	
	// movement
	void FixedUpdate(){
		// move character. Dampen movement because it is too high
		movementVector = animator.GetFloat("Speed") >= 1f ? movementVector * 0.2f : movementVector * 0.1f;
		transform.Translate(movementVector);
		movementVector = new Vector3(0, 0, 0);
		
		// walking
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) ||Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.S)){
			// hold shift to run
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
				animator.SetFloat ("Speed", 1f);
			}
			else{
				animator.SetFloat ("Speed", 0.5f);
			}
		}
		// no movement
		if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false && Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false){
			animator.SetFloat ("Speed", 0f);
		}
		
		if (Input.GetKey(KeyCode.A)){
			// left
			movementVector = new Vector3(-1, 0, movementVector.z);
		}
		if (Input.GetKey(KeyCode.D)){
			// right
			movementVector = new Vector3(1, 0, movementVector.z);
		}
		if (Input.GetKey(KeyCode.W)){
			// forward
			movementVector = new Vector3(movementVector.x, 0, 1);
		}
		if (Input.GetKey(KeyCode.S)){
			// back
			movementVector = new Vector3(movementVector.x, 0, -1);
		}
		
		if (Input.GetKey(KeyCode.Space)){
			// jump
			animator.SetBool ("Squat", false);
			animator.SetFloat ("Speed", 0f);
			aiming = false;
			animator.SetTrigger ("Jump");
		}
		
	}
	
	public int GetCurrentWeapon(){
		return currentWeapon;
	}
	public bool GetAimingState(){
		return aiming;
	}
	public bool GetRunningState(){
		return animator.GetFloat("Speed") > 0.5f ? true : false;
	}
}
