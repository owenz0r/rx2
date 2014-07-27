using UnityEngine;
using System.Collections;

public class RespawnScript : MonoBehaviour {
	
	public float respawnDelay = 1.0f;

	private bool respawning = false;
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
		transform.position = startPosition;
		GetComponent< PlayerScript >().ResetShield ();
		GetComponent< HealthScript >().ComponentToggle( true );
	}

	public void beginRespawn()
	{
		respawning = true;
	}
}
