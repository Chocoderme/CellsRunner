using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	[SerializeField] private AudioClip impact;
	[SerializeField] private AudioClip health;
	[SerializeField] private AudioClip coffee;
	[SerializeField] private AudioClip stun;

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void onVirusTransmitted() {
		audioSource.PlayOneShot (impact);
	}

	public void onHealthTaken () {
		audioSource.PlayOneShot (health);
	}

	public void onCoffeeTaken() {
		audioSource.PlayOneShot (coffee);
	}

	public void onWhiteGlobuleTouched() {
		audioSource.PlayOneShot (stun);
	}
}
