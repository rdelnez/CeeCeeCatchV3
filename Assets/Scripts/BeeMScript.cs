﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BeeMScript : MonoBehaviour {
	private List<GameObject> beeList;
	private int numberOfBEES;
	private GameObject tempBall;
	private float beeSpeed;
	private int beeValue;
	public Object BeePrefab;
	// Use this for initialization
	void Start () {
		beeList = new List<GameObject>();

		//Checking Difficulty and setting amount of bees to be spawned
		if (SVM_Script.gameDifficulty == "easy") {
			numberOfBEES = 2;
			beeSpeed = 10f;
		}
		else if (SVM_Script.gameDifficulty == "advance") {
			numberOfBEES = 3;
			beeSpeed = 12.5f;
		}
		else if (SVM_Script.gameDifficulty == "expert") {
			numberOfBEES = 5;
			beeSpeed = 15f;
		}
	}

	/*
	public void SetNumberOfBees(int howManyBees)
	{
		numberOfBEES = howManyBees;
	}
*/

	/// <summary>
	/// Starts spawning the bees and sends them after the correct answer.
	/// </summary>
	/// <param name="targetBall">The Ball with the correct answer.</param>
	public void SpawnBees(GameObject targetBall)
	{
		tempBall = targetBall;
		StartCoroutine(ReleaseTheBees());
	}

	// Update is called once per frame
	/*void Update () {
	
	}*/

	/// <summary>
	/// Spawns the bees with a delay between each, number spawned based off of member variable.
	/// </summary>
	IEnumerator ReleaseTheBees()
	{
		Debug.Log ("Spawning " + numberOfBEES + " Bees");
		for(int x = 0; x < numberOfBEES; x++)
		{
			Vector3 shiftPos = new Vector3(0f, Random.Range (-2.5f,2.5f), 0f);
			GameObject tempBee = GameObject.Instantiate(BeePrefab, this.gameObject.transform.position + shiftPos, Quaternion.identity) as GameObject;
			tempBee.GetComponent<Bee_Script>().InitialiseVariables(tempBall, beeSpeed);
			//tempBee.GetComponent<Bee_Script>().value = tempValue;
			beeList.Add(tempBee);
			yield return new WaitForSeconds(0.03f);
		}
	}

	/// <summary>
	/// Clears the bees.
	/// </summary>
	public void ClearBees()
	{
		for(int x = 0; x < beeList.Count; x++)
		{
			if(beeList[x])
				beeList[x].GetComponent<Bee_Script>().Kill();
		}
		beeList.Clear();
	}
}
