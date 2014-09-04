using UnityEngine;
using System.Collections;

public class AttractorScript : MonoBehaviour {

	void FixedUpdate()
	{
		float wellMass = 10.0f;
		float myMass = rigidbody2D.mass;
		float g = 9.8f;
	
		float num = wellMass * myMass;
		Vector2 myPos = new Vector2( transform.position.x, transform.position.y );
		Vector3 wellPos = new Vector3( 0, 0, 0 );
		Vector2 wellPos2D = new Vector2( wellPos.x, wellPos.y );
		Vector2 dir = wellPos2D - myPos;
		float mag = dir.sqrMagnitude;

		if( mag > 0.0f )
		{
			float fract = num / mag;
			float f = g * fract;
			rigidbody2D.AddForce( f * dir );
		}
	}
}
