using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Camera cameraMain;

	public GameObject playerTemplate;

	public int mapWidth;
	public int mapHeight;

	public int seed;

	public int rooms;
	public int roomSize;

	public int corridors;
	public int corridorLength;

	public float enemyChance;
	public float itemChance;

	public GameObject[] floorTemplates;
	public GameObject[] wallTemplates;
	public GameObject[] enemyTemplates;
	public GameObject[] itemTemplates;

	public static PlayerHandler playerHandler;
	public static MapGenerator mapGenerator;
	public static GameMap gameMap;
	public static Instantiator instantiator;
	public static TurnHandler turnHandler;
	public static InputHandler inputHandler;
	public static Ai ai;

	void Awake () {
		instantiator = new Instantiator ();

		playerHandler = new PlayerHandler ();	

		mapGenerator = new MapGenerator (playerTemplate, floorTemplates, wallTemplates,
		                                 enemyTemplates, itemTemplates);
		gameMap = mapGenerator.NewGameMap (mapWidth, mapHeight, seed, rooms, corridors,
		                                   roomSize, corridorLength, enemyChance, itemChance);

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
