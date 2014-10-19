using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthScript : MonoBehaviour {

	public int health = 1;
	public ScoreScript scoreScript;
	public GamestateManager gamestateManager;

	private List<bool> toggleEnable;

	void OnNetworkInstantiate( NetworkMessageInfo info )
	{
		print( "New object instanited by " + info.sender );
	}

	void Start()
	{
		toggleEnable = new List<bool>();
	}

	public void Damage( int damageCount )
	{
		health -= damageCount;
		if( health <= 0 )
		{
			networkView.RPC ( "kill", RPCMode.AllBuffered );
		}
	}

	[RPC]
	void kill()
	{
		SoundEffectsHelper.Instance.MakeExplosionSound();
		SpecialEffectsHelper.Instance.Explosion( transform.position );
		scoreScript.incrementScore();
		ComponentToggle( false );
		RespawnScript rs = GetComponent< RespawnScript >();
		rs.beginRespawn();
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
		if( networkView.isMine )
		{
			ShotScript shot = otherCollider.gameObject.GetComponent< ShotScript >();
			if( shot != null )
			{
				PlayerScript ps = GetComponent< PlayerScript >();
	
				if( ! ps.hasShieldUp )
					Damage ( shot.damage );
	
				if( shot.networkView.isMine )
				{
					Network.Destroy( shot.gameObject );
				} else {
					networkView.RPC( "destroyBullet", RPCMode.OthersBuffered, shot.gameObject.networkView.viewID );
				}
			}
			PowerupScript pup = otherCollider.gameObject.GetComponent< PowerupScript >();
			if( pup != null )
			{
				pup.triggerPowerup( this.transform );
				Destroy ( pup.gameObject );
			}
		}
	}

	[RPC]
	void destroyBullet( NetworkViewID viewId )
	{
		NetworkView bulletView = NetworkView.Find( viewId );
		Network.Destroy( bulletView.gameObject );
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
