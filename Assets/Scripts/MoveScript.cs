using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public Vector2 speed = new Vector2( 10, 10 );
	public int topBorder = 4;
	public int bottomBorder = -4;

	private Vector2 movement;

	void Update()
	{
		float inputY = Input.GetAxis ( "Vertical" );
		movement = new Vector2( 0, speed.y * inputY );

		if( transform.position.y > topBorder )
			transform.position = new Vector3( transform.position.x, topBorder, transform.position.z );
		if( transform.position.y < bottomBorder )
			transform.position = new Vector3( transform.position.x, bottomBorder, transform.position.z );
	}

	void FixedUpdate()
	{
		rigidbody2D.AddForce( movement );
	}
}
