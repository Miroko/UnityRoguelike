using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Camera cameraMain;

	public int mapWidth;
	public int mapHeight;

	public int seed;
	public int level;

	public int rooms;
	public int roomSize;

	public int corridors;
	public int corridorLength;

	public float enemyChance;
	public float itemChance;

	public GameObject playerTemplate;
	public GameObject exitTemplate;
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

		mapGenerator = new MapGenerator (mapWidth, mapHeight, seed, rooms, corridors,
		                                 roomSize, corridorLength, enemyChance, itemChance, level,
		                                 playerTemplate, exitTemplate, floorTemplates, wallTemplates,
		                                 enemyTemplates, itemTemplates);

		NextLevel ();

		turnHandler = new TurnHandler ();
		turnHandler.playerTurn = true;

		inputHandler = new InputHandler ();

		ai = new Ai ();
	}

	public static void NextLevel(){
		gameMap = mapGenerator.NewGameMap (mapGenerator.currentLevel++);
	}

	void Update () {
		turnHandler.PlayTurn ();

		// TODO: Fix lag
		Vector3 cameraPos = new Vector3(playerHandler.playerCharacter.transform.position.x, playerHandler.playerCharacter.transform.position.y, cameraMain.transform.position.z);
		cameraMain.transform.position = cameraPos;
	}

}
