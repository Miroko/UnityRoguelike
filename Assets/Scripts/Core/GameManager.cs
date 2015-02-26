using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public int mapWidth;
	public int mapHeight;

	public int seed;
	public int startLevel;

	public int rooms;
	public int roomSize;

	public int corridors;
	public int corridorLength;

	public float enemyChance;
	public float itemChance;
	
	public Player player;

	public GameObject playerTemplate;
	public GameObject exitTemplate;
	public GameObject breakableWallTemplate;
	public GameObject[] floorTemplates;
	public GameObject[] wallTemplates;
	public GameObject[] enemyTemplates;
	public GameObject[] itemTemplates;

	public UIScript uiScript;

	public static PlayerHandler playerHandler;
	public static MapGenerator mapGenerator;
	public static GameMap gameMap;
	public static Instantiator instantiator;
	public static TurnHandler turnHandler;
	public static InputHandler inputHandler;
	public static Ai ai;

	void Awake(){
		instance = this;
		DontDestroyOnLoad (gameObject);

		instantiator = new Instantiator ();		
		playerHandler = new PlayerHandler ();		
		mapGenerator = new MapGenerator (mapWidth, mapHeight, seed, rooms, corridors,
		                                 roomSize, corridorLength, enemyChance, itemChance, startLevel,
		                                 exitTemplate, breakableWallTemplate, floorTemplates, wallTemplates,
		                                 enemyTemplates, itemTemplates);		
		turnHandler = new TurnHandler ();		
		inputHandler = new InputHandler ();		
		ai = new Ai ();

		GameManager.playerHandler.SetPlayerCharacter (player);
	}

	public void NewGame(){
		GameManager.playerHandler.playerCharacter.isAlive = true;
		GameManager.mapGenerator.currentLevel = startLevel;
		GameManager.playerHandler.playerCharacter.collectedFood = GameManager.playerHandler.playerCharacter.initialCollectedFood;
		LoadNextLevel ();
	}

	void OnLevelWasLoaded(int level){
		gameMap = mapGenerator.NewGameMap (mapGenerator.currentLevel);
		turnHandler.playerTurn = true;
	}

	public void LoadNextLevel(){
		Application.LoadLevel ("Game");
	}

	void Update () {
		if (GameManager.playerHandler.playerCharacter.isAlive) {
			turnHandler.PlayTurn ();
		}
	}

}
