using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	Vector3 mouseMov; //vector that keeps track of mouse movement
	Vector3 smoothnessV;//vector to smooth mouse movement
    public float sensitivity = 3.0f; //mouse sensitivity
    public float smoothness = 4.0f; //smoothness value for camera movement
	private float xRot = 0f;
	private float yRot = 0f;
	
	// keep track of the player so we can rotate it
	private GameObject player;
	private Player playerScript;
	public GameObject playerRightArm;
	public GameObject playerLeftArm;
	public GameObject playerSpine;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerScript = player.transform.GetComponent<Player>();
		playerSpine = GameObject.Find("Player/RigAss/RigSpine1");
		//playerRightArm = GameObject.Find("Player/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmRightCollarbone/RigRightArm1");
		//playerLeftArm = GameObject.Find("Player/RigAss/RigSpine1/RigSpine2/RigSpine3/RigArmLeftCollarbone/RigLeftArm1");
	}
	
	void LateUpdate () {
		CamControls();
	}
	
	void CamControls()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        mx = mx * sensitivity * smoothness; //multiply by smooth and sensitivity value
        my = my * sensitivity * smoothness;
        Vector3 mousePos = new Vector3(mx, my); //get value of position of mouse, so the x and y coord
        //lerp to make movement smooth, so it isn't instant
        smoothnessV.x = Mathf.Lerp(smoothnessV.x, mousePos.x, 1f / smoothness);
        smoothnessV.y = Mathf.Lerp(smoothnessV.y, mousePos.y, 1f / smoothness);
        mouseMov = mouseMov + smoothnessV;
        //clamp to limit movement of y axis
        mouseMov.y = Mathf.Clamp(mouseMov.y, -15f, 20f);
		
		xRot = mouseMov.x;
        yRot = -mouseMov.y; // invert y

		// y rotates body
		transform.eulerAngles = new Vector3(yRot, xRot, 0.0f);
		player.transform.eulerAngles = new Vector3(0, xRot, 0.0f);
		
		// rotate the arms that hold the gun
		// left arm is z
		
		// right arm is y
		
		// need to rotate the body for the gun and arms
		Vector3 oldSpine = playerSpine.transform.eulerAngles;
		//playerSpine.transform.eulerAngles = new Vector3(oldSpine.x, oldSpine.y, -yRot - 100f);
		
		Quaternion newRot = Quaternion.Euler(oldSpine.x, oldSpine.y, -yRot - 100f);
		playerSpine.transform.rotation = Quaternion.Lerp(playerSpine.transform.rotation, newRot, 0.5f);
		
    }
}
