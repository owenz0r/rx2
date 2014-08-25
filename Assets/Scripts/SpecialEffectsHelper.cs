using UnityEngine;
using System.Collections;

public class SpecialEffectsHelper : MonoBehaviour {

	public static SpecialEffectsHelper Instance;

	public ParticleSystem explosionEffect;

	void Awake()
	{
		if( Instance != null )
			Debug.LogError ( "Multiple instances of SpecialEffectsHelper!" );
		Instance = this;
	}

	public void Explosion( Vector3 position )
	{
		instantiate ( explosionEffect, position );
	}

	private ParticleSystem instantiate( ParticleSystem prefab, Vector3 position )
	{
		ParticleSystem newParticleSystem = Instantiate (
												prefab,
												position,
												Quaternion.identity ) as ParticleSystem;

		Destroy ( newParticleSystem.gameObject, newParticleSystem.startLifetime );
		return newParticleSystem;
	}
}
