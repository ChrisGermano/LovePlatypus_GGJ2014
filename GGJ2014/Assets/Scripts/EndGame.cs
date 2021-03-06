﻿using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public bool doomsday;

	public GameObject meteor;
	private Vector3 meteorStart;
	private Vector3 meteorScale;
	private float meteorFlameScale;
	public AudioClip doom;
	public AudioClip saved;

	public Camera mainC;
	private float noise;
	private float blur;

	private Color skyboxC;
	private Color badColor = new Color(1f,0f,0f,1f);
	private Color goodColor = new Color(0.059f,0.725f,1f,1f);

	public float startT = 0;
	private float dist;
	private float speed = 40f;

	bool LevelEnding = false;

	// Use this for initialization
	void Start () {
		RenderSettings.skybox.SetColor("_Tint", new Color(0f, 0f, 1f));
		skyboxC = RenderSettings.skybox.GetColor("_Tint");
		meteorStart = meteor.transform.position;
		meteorScale = meteor.transform.localScale;
		meteorFlameScale = meteor.GetComponent<ParticleSystem>().startLifetime;
		dist = Vector3.Distance (meteorStart, new Vector3(0f,0f,0f));
	}

	public void EndUpdate () {
		if(!LevelEnding){
			float movedDist = (Time.time - startT) * speed;
			float perDist = movedDist / dist;
			meteor.transform.position = Vector3.Lerp(meteorStart, new Vector3(0f,0f,0f), perDist);
			if (!doomsday) {
				meteor.transform.localScale = Vector3.Lerp (meteorScale, new Vector3(0f,0f,0f), perDist);
				meteor.GetComponent<ParticleSystem>().startLifetime = Mathf.Lerp (meteorFlameScale, 0f, perDist);
				RenderSettings.skybox.SetColor("_Tint", Color.Lerp(skyboxC, goodColor, perDist));
				if (mainC.GetComponent<AudioSource>().isPlaying) {
					mainC.GetComponent<AudioSource>().volume = Mathf.Lerp (0f, 1f, perDist);
				} else {
					mainC.GetComponent<AudioSource>().clip = saved;
					mainC.GetComponent<AudioSource>().Play();
				}
			} else {
				mainC.GetComponent<NoiseEffect>().grainIntensityMin = Mathf.Lerp (0f, 0.5f, perDist);
				mainC.GetComponent<NoiseEffect>().grainIntensityMax = Mathf.Lerp (0f, 3f, perDist);
				mainC.GetComponent<NoiseEffect>().grainSize = Mathf.Lerp (0f, 5f, perDist);
				if (mainC.GetComponent<AudioSource>().isPlaying) {
					mainC.GetComponent<AudioSource>().volume = Mathf.Lerp (0f, 0.35f, perDist);
				} else {
					mainC.GetComponent<AudioSource>().clip = doom;
					mainC.GetComponent<AudioSource>().Play();
				}
				RenderSettings.skybox.SetColor("_Tint", Color.Lerp(skyboxC, badColor, perDist));
			}

			if (perDist >= 1) {
				this.StartCoroutine(EndLevel());
				LevelEnding = true;
			} else if (perDist > 0.8f) {

				if (!RenderSettings.fog) {
					RenderSettings.fog = true;
				}

				RenderSettings.fogDensity = (0.2f - (1 - perDist))/2;
			}
		}
		
	}

	private IEnumerator EndLevel()
	{
		LoadLevel lvl = GetComponent<LoadLevel>();
		bool waiting = true;
		lvl.FadeOut();
		while(waiting)
		{
			if(lvl.IsFading())
			{
				yield return 0;
			}
			else 
			{
				waiting = false;
				Application.LoadLevel("CreditScene");
			}
		}
	}
}
