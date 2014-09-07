using UnityEngine;
using System.Collections;

public class ConsumeScript : MonoBehaviour {

	void OnTriggerEnter2D( Collider2D otherCollider )
	{
		Destroy ( otherCollider.gameObject );
	}
}
