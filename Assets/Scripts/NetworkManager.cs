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

	private GameObject redPlayer;
	private GameObject bluePlayer;

	private string gameName = "OMcN_RX2";
	private bool refreshing = false;
	private HostData[] hostData;
	
	private PlayerScript.ControlStyle selectedControlStyle;

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

	void OnPlayerConnected( NetworkPlayer player )
	{
		networkView.RPC ( "doPlayerSetup", player );
	}

	[RPC]
	void startGame()
	{

		GameObject red = GameObject.FindWithTag( "red" );
		GameObject blue = GameObject.FindWithTag ( "blue" );
		if( red ){
			WeaponScript wp = blue.GetComponentInChildren< WeaponScript >();
			RespawnScript rs = red.GetComponent< RespawnScript >();
			wp.respawnScript = rs;
			print( "CONNECTED RESPAWN SCRIPT!" );
		} else {
			print ("DIDNT FIND RED :(" );
		}


		red.GetComponent< HealthScript >().scoreScript = GameObject.Find( "scripts/blueScripts" ).GetComponent< ScoreScript >();
		red.GetComponent< RespawnScript >().pregameScript = GameObject.Find( "scripts" ).GetComponent< PregameScript >();
		red.GetComponent< PlayerScript >().shieldBar = GameObject.Find ( "scripts/redScripts" ).transform;
		red.GetComponentInChildren< WeaponScript >().wellManager = GameObject.Find( "scripts" ).GetComponent< WellManager >();
		GetComponent< PregameScript >().startCountdown();
	}

	void OnServerInitialized()
	{
		print( "Server Initialized!" );
		bluePlayer = Network.Instantiate( bluePrefab, blueSpawnObject.position, Quaternion.identity, 0 ) as GameObject;
		GameObject scripts = GameObject.Find ( "scripts"  );
		GameObject blueScripts = GameObject.Find ( "scripts/blueScripts"  );
		GameObject redScripts = GameObject.Find ( "scripts/redScripts"  );
		bluePlayer.GetComponent< PlayerScript >().shieldBar = blueScripts.transform;
		bluePlayer.GetComponent< PlayerScript >().setControls( selectedControlStyle );
		bluePlayer.GetComponent< HealthScript >().scoreScript = redScripts.GetComponent< ScoreScript >();
		bluePlayer.GetComponent< HealthScript >().gamestateManager = scripts.GetComponent< GamestateManager >();
		bluePlayer.GetComponent< RespawnScript >().pregameScript = scripts.GetComponent< PregameScript >();
		bluePlayer.GetComponent< RespawnScript >().gamestateManager = scripts.GetComponent< GamestateManager >();
		WeaponScript wp = bluePlayer.GetComponentInChildren< WeaponScript >();
		wp.pregameScript = scripts.GetComponent< PregameScript >();
		wp.wellManager = scripts.GetComponent< WellManager >();
		scripts.GetComponent<PowerupsHelper>().enabled = true;
		//wp.gamestateManager = scripts.GetComponent< GamestateManager >();
	}

	void OnConnectedToServer()
	{

	}

	[RPC]
	void doPlayerSetup()
	{
		print( "Connected!" );
		redPlayer = Network.Instantiate( redPrefab, redSpawnObject.position, Quaternion.identity, 0 ) as GameObject;

		GameObject scripts = GameObject.Find ( "scripts"  );
		GameObject blueScripts = GameObject.Find ( "scripts/blueScripts"  );
		GameObject redScripts = GameObject.Find ( "scripts/redScripts"  );
		redPlayer.GetComponent< PlayerScript >().shieldBar = redScripts.transform;
		redPlayer.GetComponent< PlayerScript >().setControls( selectedControlStyle );
		redPlayer.GetComponent< HealthScript >().scoreScript = blueScripts.GetComponent< ScoreScript >();
		redPlayer.GetComponent< HealthScript >().gamestateManager = scripts.GetComponent< GamestateManager >();
		redPlayer.GetComponent< RespawnScript >().pregameScript = scripts.GetComponent< PregameScript >();
		redPlayer.GetComponent< RespawnScript >().gamestateManager = scripts.GetComponent< GamestateManager >();
		WeaponScript wp = redPlayer.GetComponentInChildren< WeaponScript >();
		wp.pregameScript = scripts.GetComponent< PregameScript >();
		wp.wellManager = scripts.GetComponent< WellManager >();
		//wp.gamestateManager = scripts.GetComponent< GamestateManager >();

		GameObject blue = GameObject.FindWithTag ( "blue" );
		if( blue == null )
			print( "Couldn't find blue" );
		RespawnScript rs = blue.GetComponent< RespawnScript >();
		if( rs == null )
			print( "Couldn't find respawnscript" );
		wp.respawnScript = rs;

		blue.GetComponent< HealthScript >().scoreScript = redScripts.GetComponent< ScoreScript >();
		blue.GetComponent< RespawnScript >().pregameScript = scripts.GetComponent< PregameScript >();
		blue.GetComponent< PlayerScript >().shieldBar = blueScripts.transform;
		blue.GetComponentInChildren< WeaponScript >().wellManager = scripts.GetComponent< WellManager >();
		networkView.RPC( "startGame", RPCMode.AllBuffered );
	}

	void startServer()
	{
		Network.InitializeServer( 2, 25001, !Network.HavePublicAddress() );
		MasterServer.RegisterHost( gameName, "Owen's Awesome Game", "Isn't is awesome?" );
	}

	void setKeyboardMouseControls()
	{
		selectedControlStyle = PlayerScript.ControlStyle.KeyboardMouse;
	}

	void setXBoxControls()
	{
		selectedControlStyle = PlayerScript.ControlStyle.Controller;
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

			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY + (startServerButtonH + (0.02f * Screen.height)) * 2 , 
			                         startServerButtonW, 
			                         startServerButtonH ), "Keyboard+Mouse" ) )
			{
				print( "Setting Keyboard+Mouse..." );
				setKeyboardMouseControls();
			}

			if( GUI.Button( new Rect( startServerButtonX, 
			                         startServerButtonY + (startServerButtonH + (0.02f * Screen.height)) * 3 , 
			                         startServerButtonW, 
			                         startServerButtonH ), "XBox Controller" ) )
			{
				print( "Setting XBox Controller..." );
				setXBoxControls();
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
