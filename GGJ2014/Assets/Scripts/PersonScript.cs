﻿using UnityEngine;
using System.Collections;

public class PersonScript : MonoBehaviour {

	public int[] thoughts = new int[3];

	// Use this for initialization
	void Start () {
		int thought = Random.Range (1,50);
		for (int i = 0; i < thoughts.Length; i++) {
			thoughts[i] = thought % 3 + 1;
			thought = Random.Range (1,50);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
