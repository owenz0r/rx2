using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {
	
	public float respawnDelay = 1.0f;
	public PregameScript pregameScript;
	public  bool respawning = false;

	private float delay;
	private Vector3 startPosition;

	void Start()
	{
		delay = respawnDelay;
		startPosition = transform.position;
	}

	void Update()
	{
		if( respawning )
		{
			delay -= Time.deltaTime;
			if( delay <= 0.0f )
			{
				Respawn ();
				delay = respawnDelay;
				respawning = false;
			}
		}
	}

	void Respawn()
	{
		pregameScript.startPregame();
		transform.position = startPosition;
		GetComponent< PlayerScript >().ResetShield ();
		GetComponent< HealthScript >().ComponentToggle( true );
		GetComponentInChildren< WeaponScript >().setWeaponTrishot( false );
	}

	public void beginRespawn()
	{
		respawning = true;
	}
}
