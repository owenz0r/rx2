using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public ScoreScript scoreScript;

	private List<bool> toggleEnable;

	void Start()
	{
		toggleEnable = new List<bool>();
	}

	public void Damage( int damageCount )
	{
		health -= damageCount;
		if( health <= 0 )
		{
			SoundEffectsHelper.Instance.MakeExplosionSound();
			SpecialEffectsHelper.Instance.Explosion( transform.position );
			scoreScript.incrementScore();
			ComponentToggle( false );
			RespawnScript rs = GetComponent< RespawnScript >();
			rs.beginRespawn();
		}
	}

	public void ComponentToggle( bool onoff )
	{
		if(toggleEnable.Count > 0 )
		{
			if( toggleEnable[0] )
				GetComponentInChildren< SpriteRenderer >().enabled = onoff;
			if( toggleEnable[1] )
				GetComponent< PlayerScript >().enabled = onoff;
			if( toggleEnable[2] )
				GetComponent< CircleCollider2D >().enabled = onoff;
		} else {
			toggleEnable.Add( GetComponentInChildren< SpriteRenderer >().enabled );
			toggleEnable.Add( GetComponent< PlayerScript >().enabled );
			toggleEnable.Add( GetComponent< CircleCollider2D >().enabled );

			GetComponentInChildren< SpriteRenderer >().enabled = onoff;
			GetComponent< PlayerScript >().enabled = onoff;
			GetComponent< CircleCollider2D >().enabled = onoff;
		}
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
		PowerupScript pup = otherCollider.gameObject.GetComponent< PowerupScript >();
		if( pup != null )
		{
			pup.triggerPowerup( this.transform );
			Destroy ( pup.gameObject );
		}
	}


	void OnCollisionEnter2D( Collision2D coll )
	{
		SoundEffectsHelper.Instance.MakeShieldHitSound();
		ContactPoint2D c = coll.contacts[0];
		Vector3 newPos = new Vector3( c.point[0], c.point[1], -5 );
		//newPos.z = -5.0f;
		SpecialEffectsHelper.Instance.ShieldBounce( newPos );
	}


}
