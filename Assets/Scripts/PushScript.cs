using UnityEngine;
using System.Collections;

public class PushScript : MonoBehaviour {

	public Vector2 force;
	// Use this for initialization
	void Start () {
		rigidbody2D.AddForce( force );
	}

}
