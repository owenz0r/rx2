using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject bluePrefab;
	public Transform blueSpawnObject;
	public GameObject redPrefab;
	public Transform redSpawnObject;

	private float startServerButtonX;
	private float startServerButtonY;
	private float startServerButtonW;
	private float startServerButtonH;

	private string gameName = "OMcN_RX2";
	private bool refreshing = false;
	private HostData[] hostData;

	void Start()
	{
		startServerButtonW = 0.1f * Screen.width;
		startServerButtonH = 0.05f * Screen.height;
		startServerButtonX = (0.3f * Screen.width);
		startServerButtonY = (0.3f * Screen.height) - (startServerButtonH / 2);
	}

	void Update()
	{
		if( refreshing )
		{
			print ( "..." );
			if( MasterServer.PollHostList().Length > 0 )
			{
				refreshing = false;
				print ( MasterServer.PollHostList().Length );
				hostData = MasterServer.PollHostList();
			}
		}
	}

	void refreshHostList()
	{
		MasterServer.RequestHostList( gameName );
		refreshing = true;
	}
	
	
	void OnMasterServerEvent( MasterServerEvent msEvent )
	{
		if( msEvent == MasterServerEvent.RegistrationSucceeded )
			print( "Server registered!" );
	}

	void OnPlayerConnected()
	{
		networkView.RPC( "startGame", RPCMode.AllBuffered );
	}

	[RPC]
	void startGame()
	{
		GameObject red = GameObject.FindGameObjectWithTag( "red" );
		GameObject blue = GameObject.FindGameObjectWithTag( "blue" );
		WeaponScript wp = red.GetComponentInChildren< WeaponScript >();
		wp.respawnScript = blue.GetComponent< RespawnScript >();
		blue.GetComponentInChildren< WeaponScript >().respawnScript = red.GetComponent< RespawnScript >();
		GetComponent< PregameScript >().startCountdown();
	}

	void OnServerInitialized()
	{
		print( "Server Initialized!" );
		GameObject bluePlayer = (GameObject)Network.Instantiate( bluePrefab, blueSpawnObject.position, Quaternion.identity, 0 );
		GameObject scripts = GameObject.Find ( "scripts"  );
		GameObject blueScripts = GameObject.Find ( "scripts/blueScripts"  );
		GameObject redScripts = GameObject.Find ( "scripts/redScripts"  );
		bluePlayer.GetComponent< PlayerScript >().shieldBar = blueScripts.transform;
		bluePlayer.GetComponent< HealthScript >().scoreScript = redScripts.GetComponent< ScoreScript >();
		bluePlayer.GetComponent< RespawnScript >().pregameScript = scripts.GetComponent< PregameScript >();
		WeaponScript wp = bluePlayer.GetComponentInChildren< WeaponScript >();
		wp.pregameScript = scripts.GetComponent< PregameScript >();
		wp.wellManager = scripts.GetComponent< WellManager >();
	}

	void OnConnectedToServer()
	{
		print( "Connected!" );
		GameObject redPlayer = (GameObject)Network.Instantiate( redPrefab, redSpawnObject.position, Quaternion.identity, 0 );
		GameObject scripts = GameObject.Find ( "scripts"  );
		GameObject blueScripts = GameObject.Find ( "scripts/blueScripts"  );
		GameObject redScripts = GameObject.Find ( "scripts/redScripts"  );
		redPlayer.GetComponent< PlayerScript >().shieldBar = redScripts.transform;
		redPlayer.GetComponent< HealthScript >().scoreScript = blueScripts.GetComponent< ScoreScript >();
		redPlayer.GetComponent< RespawnScript >().pregameScript = scripts.GetComponent< PregameScript >();
		WeaponScript wp = redPlayer.GetComponentInChildren< WeaponScript >();
		wp.pregameScript = scripts.GetComponent< PregameScript >();
		wp.wellManager = scripts.GetComponent< WellManager >();
	}

	void startServer()
	{
		Network.InitializeServer( 2, 25001, !Network.HavePublicAddress() );
		MasterServer.RegisterHost( gameName, "Owen's Awesome Game", "Isn't is awesome?" );
	}

	void OnGUI()
	{
		if( !Network.isClient && !Network.isServer)
		{
			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY, 
			                         startServerButtonW, 
			                         startServerButtonH ), "Start Server" ) )
			{
				print( "Start server" );
				startServer();
			}
			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY + startServerButtonH + (0.02f * Screen.height), 
			                         startServerButtonW, 
			                         startServerButtonH ), "Refresh" ) )
			{
				print( "Refreshing..." );
				refreshHostList();
			}
			
			if( hostData != null )
			{
				for( int i=0; i < hostData.Length; i++ )
				{
					if( GUI.Button( new Rect( startServerButtonX * 1.5f,
					                         startServerButtonY + (i * (startServerButtonH  + (0.02f * Screen.height) ) ),
					                         startServerButtonW,
					                         startServerButtonH ),
					               hostData[i].gameName ) )
					{
						Network.Connect( hostData[i] );
					}
				}
			}
		}
	}
}
