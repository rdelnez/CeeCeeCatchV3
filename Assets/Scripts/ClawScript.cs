﻿using UnityEngine;
using System.Collections;

public class ClawScript : MonoBehaviour {
	
	
	public Transform origin; 
	public Vector3 retractOrigin;
	public Vector3 originalClawPos;
	public float speed =5f; 
	public GunScript gunScript;

	
	
	
	public ScoreManagerScript SM_Script;
	public GM_1 GM_Script;
	public BeeMScript BeeM_Script;

	public Vector3 target; 
	public int ballValue = 100;
	public GameObject childObject; 
	public LineRenderer lineRenderer;
	public Material lineMaterial;
	public bool hitBall;
	public bool hitCollectibles;
	public bool retracting; 
	public GameObject fishingRod;

	public float retractingSpeed;

	void Awake () 
	{
		retractOrigin = new Vector3 (0,0.73f,-2.77f);
		originalClawPos = transform.localPosition;
		hitCollectibles = false;
		
		lineRenderer = GetComponent<LineRenderer> ();
	}

	void Start(){

	if (SVM_Script.gameDifficulty == "easy") {
		retractingSpeed = 2.5f;
		
	}
	else if (SVM_Script.gameDifficulty == "advance") {
		retractingSpeed = 3.5f;
		
	}
	else if (SVM_Script.gameDifficulty == "expert") {
		retractingSpeed = 5f;
		
		}
	}
	
	void Update () 
	{
		float step = speed * Time.deltaTime; 
		float stoop = retractingSpeed*Time.deltaTime; 

		if (gunScript.isShooting && !retracting) {
			transform.position = Vector3.MoveTowards(transform.position, target, step);
			transform.localEulerAngles += new Vector3(0,10.0f,0);


			lineRenderer.material = lineMaterial;
			lineRenderer.SetPosition (0, origin.position);
			lineRenderer.SetPosition (1, transform.position);
		}
		else if(gunScript.isShooting && retracting){
			transform.position = Vector3.MoveTowards(transform.position, origin.position, stoop);

			if(!hitCollectibles){
				transform.localEulerAngles += new Vector3(0,10.0f,0);
			}
		}
			
		{
			lineRenderer.material = lineMaterial;
			lineRenderer.SetPosition (0, origin.position);
			lineRenderer.SetPosition (1, transform.position);


		}


		//if (transform.position == retractrigin && retracting) 
		if (transform.position == origin.position && retracting) 
		{
			gunScript.CollectedObject ();

			if (hitBall) // this if is for when the claw hits a ball that needs to be destroyed
			{
				Debug.Log ("collectedOBJ");
				//	scoreManager.AddPoints (ballValue);
				hitBall = false;
				Debug.Log ("booo");

				Debug.Log ("booo2");
				if(SM_Script.CheckScore(childObject.GetComponent<BallScript>().scoreValue)){ 	//to instantiate particle for win 
					childObject.GetComponent<BallScript>().InstantiateParticleWin();

					///////////////////////////////////////////////

					GM_Script.ResetQuestion();
					GM_Script.DestroyInstatiatedBalls("balls");
					GM_Script.SpawnBalls();
					BeeM_Script.ClearBees();
				}
				else {																  			//to instantiate particle for lose
					childObject.GetComponent<BallScript>().InstantiateParticleLose();
				}

			}

			else if(hitCollectibles){
				hitCollectibles = false;


				childObject.GetComponent<CollectiblesScript>().DestroyCollectible();

				SM_Script.GainLife ();
				childObject.GetComponent<CollectiblesScript>().InstantiateStars();


			}


			this.transform.localPosition = new Vector3 (0,-1.888f,-2.77f);
			Debug.Log ("retracingSpeed");

			//this.transform.localRotation = Quaternion.Euler (270,0,0); 
			//this.transform.localEulerAngles = new Vector3 (270,0,0);

			retracting = false;
			this.gameObject.SetActive (false);


			Debug.Log ("dead");

			//Reposition();


			



		}
	}


	//Re-position the fishing Rod
	//public void Reposition(){
//		fishingRod.transform.localEulerAngles = new Vector3 (270, fishingRod.transform.localEulerAngles.y, fishingRod.transform.localEulerAngles.z);
	//}
	//End reposition fishingRod

	public void ClawTarget (Vector3 pos)
		
	{
		target = pos;
	}

	public void GetOrigin(){
		target = origin.position;
	}

	//public void ResetClaw(){
		//this.transform.localPosition = originalClawPos;
	//}

	void OnTriggerEnter (Collider other)
	{
		if(!retracting){
			retracting = true;
			
			GetOrigin ();
			//target = origin.position;
			//target = retractOrigin;
			gunScript.CallRotateBackRod ();
			
			if (other.gameObject.CompareTag ("balls")) {
				Debug.Log ("Hit");
				
				hitBall = true;
				childObject = other.gameObject;
				SM_Script.playerAnswerInSM = childObject.GetComponent<BallScript> ().points;
				if(SM_Script.VerifyAnswer())
				{
					BeeM_Script.SpawnBees(other.gameObject);
				}
			
				other.transform.SetParent (this.transform);
			} else if (other.gameObject.CompareTag ("collectibles")) {
				hitCollectibles = true;
				childObject = other.gameObject;
				other.gameObject.GetComponent<CollectiblesScript>().isCollected = true;
				other.transform.SetParent (this.transform);

			}




		
		}
	}


	//}
}
