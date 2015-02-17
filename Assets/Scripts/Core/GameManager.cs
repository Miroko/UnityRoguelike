using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Camera cameraMain;

	public GameObject playerTemplate;

	public int mapWidth;
	public int mapHeight;

	public int rooms;
	public int roomSize;

	public int corridorsPerRoom;
	public int corridorLength;


	public GameObject[] floorTemplates;
	public GameObject[] wallTemplates;
	public GameObject[] enemyTemplates;
	public GameObject[] itemTemplates;

	public GameObject[] roomTemplates;

	public static PlayerHandler playerHandler;
	public static GameMap gameMap;
	public static Instantiator instantiator;
	public static TurnHandler turnHandler;
	public static InputHandler inputHandler;
	public static Ai ai;

	void Awake () {
		instantiator = new Instantiator ();

		gameMap = new GameMap(floorTemplates, wallTemplates, enemyTemplates, itemTemplates, roomTemplates);
		gameMap.GenerateMap (mapWidth, mapHeight, rooms, roomSize, corridorsPerRoom, corridorLength);

		playerHandler = new PlayerHandler ();		
		playerHandler.playerEntity = (Character)gameMap.SpawnEntity (gameMap.bounds.center, playerTemplate).gameObject.GetComponent<Entity>();

		turnHandler = new TurnHandler ();
		turnHandler.playerTurn = true;

		inputHandler = new InputHandler ();

		ai = new Ai ();
	}

	void Update () {
		turnHandler.PlayTurn ();

		// TODO: Fix lag
		Vector3 cameraPos = new Vector3(playerHandler.playerEntity.transform.position.x, playerHandler.playerEntity.transform.position.y, cameraMain.transform.position.z);
		cameraMain.transform.position = cameraPos;
	}

}
