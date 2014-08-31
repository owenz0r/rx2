using UnityEngine;
using System.Collections;

public class PowerupsHelper : MonoBehaviour {

	public Transform trishotPrefab;
	public float powerupInterval;

	private float counter;	
	// Use this for initialization
	void Start () {
		counter = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if( counter > powerupInterval )
		{
			Transform pup1 = Instantiate( trishotPrefab ) as Transform;
			Transform pup2 = Instantiate( trishotPrefab ) as Transform;

			MoveScript ms1 = pup1.GetComponent< MoveScript >();
			MoveScript ms2 = pup2.GetComponent< MoveScript >();

			Vector2 random = RandomPositivePointOnUnitCircle();
			ms1.direction.x = random.x;
			ms1.direction.y = random.y;
			ms2.direction.x = random.x * -1.0f;
			ms2.direction.y = random.y * -1.0f;

			counter = 0.0f;
		}
	}

	Vector2 RandomPositivePointOnUnitCircle()
	{
		float angle = Random.Range( -90.0f, 90.0f );
		float radians = angle * Mathf.PI / 180.0f;

		float x = Mathf.Cos ( radians );
		float y = Mathf.Sin ( radians );

		return new Vector2( x, y );
	}

}
