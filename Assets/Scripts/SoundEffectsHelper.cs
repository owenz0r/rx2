using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour {

	public static SoundEffectsHelper Instance;

	public AudioClip laserFireSound;
	public AudioClip shieldHitSound;

	void Awake()
	{
		if( Instance != null )
		{
			Debug.LogError ( "Multiple instances of SoundEffectsHelper!" );
		}
		Instance = this;
	}

	public void MakeLaserFireSound()
	{
		MakeSound( laserFireSound );
	}

	public void MakeShieldHitSound()
	{
		MakeSound( shieldHitSound );
	}

	private void MakeSound( AudioClip originalClip )
	{
		AudioSource.PlayClipAtPoint( originalClip, transform.position );
	}
}
