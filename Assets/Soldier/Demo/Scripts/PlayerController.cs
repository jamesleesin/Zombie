using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
public class PlayerController : MonoBehaviour {

	public Transform rightGunBone;
	public Transform leftGunBone;
	public Arsenal[] arsenal;
	
	private GameObject currentGun;

	private Animator animator;

	void Awake() {
		animator = GetComponent<Animator> ();
		if (arsenal.Length > 0){
			SetArsenal (arsenal[0].name);
		}
	}

	public GameObject GetGun(){
		return currentGun;
	}
		
	public void SetArsenal(string name) {
		foreach (Arsenal hand in arsenal) {
			if (hand.name == name) {
				if (rightGunBone.childCount > 0)
					Destroy(rightGunBone.GetChild(0).gameObject);
				if (leftGunBone.childCount > 0)
					Destroy(leftGunBone.GetChild(0).gameObject);
				if (hand.rightGun != null) {
					GameObject newRightGun = (GameObject) Instantiate(hand.rightGun);
					currentGun = newRightGun;
					newRightGun.transform.parent = rightGunBone;

					if (hand.name == "Rifle"){
						newRightGun.transform.localPosition = Vector3.zero;
						newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
						// sci fi rifle needs to be scaled to match body
						newRightGun.transform.localScale = new Vector3(0.4f, 0.35f, 0.35f);
					}
					if (hand.name == "Pistol"){
						newRightGun.transform.localPosition = new Vector3(0, -0.08f, 0.05f);
						newRightGun.transform.localRotation = Quaternion.Euler(100, 0, 0);
						newRightGun.transform.localScale = new Vector3(0.011f, 0.009f, 0.009f);
					}
					if (hand.name == "AK47"){
						newRightGun.transform.localPosition = new Vector3(0, -0.1f, 0.05f);
						newRightGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
						newRightGun.transform.localScale = new Vector3(0.03f, 0.022f, 0.022f);
					}

				}
				if (hand.leftGun != null) {
					GameObject newLeftGun = (GameObject) Instantiate(hand.leftGun);
					newLeftGun.transform.parent = leftGunBone;
					newLeftGun.transform.localPosition = Vector3.zero;
					newLeftGun.transform.localRotation = Quaternion.Euler(90, 0, 0);
				}
				animator.runtimeAnimatorController = hand.controller;
				return;
			}
		}
	}

	[System.Serializable]
	public struct Arsenal {
		public string name;
		public GameObject rightGun;
		public GameObject leftGun;
		public RuntimeAnimatorController controller;
	}
}
