using UnityEngine;
using System.Collections;

public class SoundEffectsHelper : MonoBehaviour {

	public static SoundEffectsHelper Instance;

	public AudioClip laserFireSound;
	public AudioClip shieldHitSound;
	public AudioClip explosionSound;
	public AudioClip countdownTickSound;
	public AudioClip countdownGoSound;

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

	public void MakeExplosionSound()
	{
		MakeSound( explosionSound );
	}

	public void MakeCountdownTickSound()
	{
		MakeSound ( countdownTickSound );
	}

	public void MakeCountdownGoSound()
	{
		MakeSound ( countdownGoSound );
	}

	private void MakeSound( AudioClip originalClip )
	{
		AudioSource.PlayClipAtPoint( originalClip, transform.position );
	}
}
