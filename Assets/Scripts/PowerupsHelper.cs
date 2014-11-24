using UnityEngine;
using System.Collections;

public class PowerupsHelper : MonoBehaviour {

	public Transform trishotPrefab;
	public Transform rechargePrefab;
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
			Transform prefab;
			switch( Random.Range( 0, 2 ) )
			{
				case 0: prefab = trishotPrefab; break;
				case 1: prefab = rechargePrefab; break;
				default : prefab = trishotPrefab; break;
			}

			Transform pup1 = Network.Instantiate( prefab, Vector3.zero, Quaternion.identity, 0 ) as Transform;
			Transform pup2 = Network.Instantiate( prefab, Vector3.zero, Quaternion.identity, 0 ) as Transform;

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
