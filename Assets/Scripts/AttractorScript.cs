using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractorScript : MonoBehaviour {

	public WellManager wellManager;
	public float g = 9.8f;
	public float maxDistSqr = 100.0f;

	void Start()
	{
		GameObject scriptManager = GameObject.Find( "scripts" );
		wellManager = scriptManager.GetComponent< WellManager >();
	}

	void FixedUpdate()
	{
		float myMass = rigidbody2D.mass;

		List< Transform > wellList = wellManager.getWellList();
		foreach( Transform well in wellList )
		{
			float wellMass = well.rigidbody2D.mass;
	
			float num = wellMass * myMass;
			Vector2 myPos = new Vector2( transform.position.x, transform.position.y );
			Vector3 wellPos = well.position;
			Vector2 wellPos2D = new Vector2( wellPos.x, wellPos.y );
			Vector2 dir = wellPos2D - myPos;
			float mag = dir.sqrMagnitude;

			if( mag > 0.0f && mag < maxDistSqr )
			{
				float fract = num / mag;
				float f = g * fract;
				rigidbody2D.AddForce( f * dir );
			}
		}
	}
}
