using UnityEngine;
using System.Collections;

public class OutOfBoundsScript : MonoBehaviour {

	void OnTriggerEnter2D( Collider2D otherCollider )
	{
		print( "OUT OF BOUNDS" );
		if( otherCollider.gameObject.GetComponent< NetworkView >().isMine )
		{
			Network.Destroy( otherCollider.gameObject );
			print( "OBJECT DELETED" );
		}
	}
}
