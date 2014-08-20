using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public ScoreScript scoreScript;

	public void Damage( int damageCount )
	{
		health -= damageCount;
		if( health <= 0 )
		{
			SoundEffectsHelper.Instance.MakeExplosionSound();
			scoreScript.incrementScore();
			ComponentToggle( false );
			RespawnScript rs = GetComponent< RespawnScript >();
			rs.beginRespawn();
		}
	}

	public void ComponentToggle( bool onoff )
	{
		gameObject.renderer.enabled = onoff;
		GetComponent< PlayerScript >().enabled = onoff;
		GetComponent< CircleCollider2D >().enabled = onoff;
	}

	void OnTriggerEnter2D( Collider2D otherCollider )
	{
		ShotScript shot = otherCollider.gameObject.GetComponent< ShotScript >();
		if( shot != null )
		{
			PlayerScript ps = GetComponent< PlayerScript >();

			if( ! ps.hasShieldUp )
				Damage ( shot.damage );

			Destroy ( shot.gameObject );
		}
	}


	void OnCollisionEnter2D( Collision2D coll )
	{
		SoundEffectsHelper.Instance.MakeShieldHitSound();
	}


}
