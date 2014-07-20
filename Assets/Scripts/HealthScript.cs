using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {

	public int health = 1;

	public void Damage( int damageCount )
	{
		health -= damageCount;
		if( health <= 0 )
			Destroy( gameObject );
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

}
