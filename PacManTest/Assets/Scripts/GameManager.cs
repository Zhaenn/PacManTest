using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    private int currentLives;
    private int maxLives = 5;
    private int currentLevel = 1;
    public PlayerMovement pacman;
    public bool levelStarted = false;
    public List<Ghost> allGhosts = new List<Ghost>();

    [Header ("References")]
    public static GameManager instance;
    public GameObject currentScoreValueText;
    public GameObject highScoreValueText;
    public GameObject livesPrefab;
    public GameObject livesContainer;

    [Header("Point System & Dots")]
    private Dot[] allDots;
    private int dotNumber; //used to determine how many dots there was at the start to determine when the players has eaten half of them
    private List<Dot> allDotsInLevel = new List<Dot>();

    private int currentPoints;
    private int currentHighScore;
    public int pointMultiplier = 1;
    private bool bonusLifeAwarded = false;

    public List<Fruits> allFruits = new List<Fruits>();
    public GameObject fruit;
    private bool fruitSpawned = false;
    

    [Header("Fruit Sprite References")]
    public Sprite cherrySprite;
    public Sprite strawberrySprite;
    public Sprite orangeSprite;
    public Sprite appleSprite;
    public Sprite melonSprite;
    public Sprite galaxianSprite;
    public Sprite bellSprite;
    public Sprite keySprite;



    private void Awake()
    {
        //Not needed in this case as I don't load another scene, but generally a good practice to keep singletons throughout the entire game.
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        //Basic setup of lists using temporary arrays. Also contains the Dictionary for fruits that will spawn according to current level.
        //Starts the game by calling StartLevel after 3 seconds.

        instance = this;
        pacman = FindObjectOfType<PlayerMovement>();

        allDots = FindObjectsOfType<Dot>();
        dotNumber = allDots.Length;

        foreach(Dot d in allDots)
        {
            allDotsInLevel.Add(d);
        }

        Ghost[] theGhosts = FindObjectsOfType<Ghost>();
        foreach(Ghost g in theGhosts)
        {
            allGhosts.Add(g);
        }

        Dictionary<int, Fruits> fruitsDic = new Dictionary<int, Fruits>()
        {
             {0, new Fruits(100, "Cherry", cherrySprite)},
             {1, new Fruits(300, "Strawberry", strawberrySprite)},
             {2, new Fruits(500, "Orange", orangeSprite)},
             {3, new Fruits(700, "Apple", appleSprite)},
             {4, new Fruits(1000, "Melon", melonSprite)},
             {5, new Fruits(2000, "Galaxian Flagship", galaxianSprite)},
             {6, new Fruits(3000, "Bell", bellSprite)},
             {7, new Fruits(5000, "Key", keySprite)},
        };

        currentLives = maxLives;
        foreach(Fruits f in fruitsDic.Values)
        {
            allFruits.Add(f);
        }

        UpdateUI();

        Invoke("StartLevel", 3f);
    }

    //---------------------------------------------------- GAMEPLAY LOOP -------------------------------------\\


    //Ghosts and Player can't move if this variable is set to false. 
    //Therefore, after each level and before the game begins, this variable is set to false.
    //Enabling it allows the game to start
    public void StartLevel()
    { 
        levelStarted = true;
    }

    //Basic reset function. Resets the board game by calling the ResetDots function, removes any fruit that the player didn't pick up
    //and increments the current level to make the next fruit appear next time. Also calls ResetLevel which is different than this.
    public void FinishedLevel()
    {  
        currentLevel++;
        pacman.playerDirection = Vector2.right;

        Destroy(transform.Find("FruitSpawnPoint").GetChild(0).gameObject);
        fruitSpawned = false;

        ResetLevel();

        Invoke("ResetDots", 1f); //A delay in the reset is used to prevent the very last dot from not correctly toggling back on.
    }

    //Reset Level is used to reset the ghosts and the player. This is used when the players dies, but has lives remaining.
    //Then, the board is not reset (all dots taken stay taken), but the ghosts and the player all come back to their original position.
    public void ResetLevel()
    {
        levelStarted = false;

        pacman.transform.position = pacman.startPosition;

        foreach (Ghost g in allGhosts)
        {
            g.ResetState();
        }

        Invoke("StartLevel", 3f);
    }

    //Simply toggles the dots back on and adds them back to the list of dots contained in the level for checks later.
    public void ResetDots()
    {        
        foreach (Dot d in allDots)
        {
            d.gameObject.SetActive(true);
            allDotsInLevel.Add(d);
        }
    }

    //When the player collides with a ghost, it checks if there are lives remaining. If so, the game resets except the board.
    //If not, then the game is over and everything resets back to default.
    public void PacmanDeath()
    {
        currentLives--;
        //This removes one of the UI pacman displayed on the screen indicating how many lives are left.
        Destroy(livesContainer.transform.GetChild(livesContainer.transform.childCount - 1).gameObject);

        if (currentLives > 0)
        {
            ResetLevel();
        }
        else
        {
            GameOver();
        }
    }

    //Completely resets the entire game when the player runs out of lives
    public void GameOver()
    {
        //Reset the board state
        ResetLevel();        
        ResetDots();

        //Reset game values
        currentLevel = 1;
        bonusLifeAwarded = false;
        fruitSpawned = false;

        //Reset the UI info
        currentLives = maxLives;
        currentPoints = 0;
        UpdateUI();        
    }

    //This function is used to return the ghosts to normal after they are vulnerable from the player eating an energizer (big dots).
    //It resets their behavior if they're not currently returning to spawn after being eaten and resets their sprite.
    public void ReturnGhostToNormal()
    {
        foreach (Ghost g in allGhosts)
        {
            if (g.currentGhostBehavior != Ghost.behavior.respawn)
            {
                g.GetComponent<SpriteRenderer>().sprite = g.ghostSprite;
                g.currentGhostBehavior = g.mainBehavior;
                g.vulnerable = false;
            }
        }
        pointMultiplier = 1;
    }

    //---------------------------------------------------- DOTS AND POINT SYSTEM -------------------------------------\\

    //This functions is called whenever the player eats a dot. It removes that dot from the list and then checks
    //how many are left. If less than half, it spawns a fruit. It none, it finishes the level.
    public void DotEaten(Dot dot)
    {
        allDotsInLevel.Remove(dot);


        if (allDotsInLevel.Count <= 0)
        {
            FinishedLevel();
        }
        else if (allDotsInLevel.Count < dotNumber / 2 && !fruitSpawned)
        {
            SpawnFruit();
            fruitSpawned = true;
        }
    }

    //The points function used by every object that gives points to the player (dot, energizer, ghost, fruits)
    public void AddPoints(int pointsToAdd)
    {
        currentPoints += pointsToAdd;

        //In the base game, the player is awarded a bonus life only once when reaching 10,000 points.
        if(currentPoints >= 10000 && !bonusLifeAwarded)
        {
            SpawnLife();
            currentLives++;
            bonusLifeAwarded = true;
        }
        currentScoreValueText.GetComponent<TMPro.TextMeshProUGUI>().text = currentPoints.ToString();
       
        //In the base game, the high score is always updated at the same time as the score if the score is higher. While the score remains through death,
        //it doesn't stay when the game is over. The high score stays as long as the game stays open.
        if(currentPoints > currentHighScore)
        {
            currentHighScore = currentPoints;
            highScoreValueText.GetComponent<TMPro.TextMeshProUGUI>().text = currentHighScore.ToString();
        }
    }


    //Spawns a fruit at the designated point. The Transform is located under the Game Manager empty object and is located right under the ghost box.   
    public void SpawnFruit()
    {
        Fruits fruitToSpawn = ChooseFruit();
        Transform spawnPoint = transform.Find("FruitSpawnPoint");
        GameObject newFruit = Instantiate(fruit, spawnPoint.position, transform.rotation, spawnPoint);
        newFruit.name = fruitToSpawn.fruitName;
        newFruit.GetComponent<Interactible>().points = fruitToSpawn.points;
        newFruit.GetComponent<SpriteRenderer>().sprite = fruitToSpawn.sprite;
    }

    //Returns a chosen fruit according to current level. If the level is higher than the number of fruits, always returns the highest fruit.
    public Fruits ChooseFruit()
    {
        if(currentLevel - 1 > allFruits.Count)
        {
            return allFruits[allFruits.Count - 1];
        }
        else
        {
            return allFruits[currentLevel - 1];
        }
    }

    //---------------------------------------------------- UI -------------------------------------\\
    public void UpdateUI()
    {
        currentScoreValueText.GetComponent<TMPro.TextMeshProUGUI>().text = currentPoints.ToString();

        //Spawn lives according to how many we have at the start
        for (int i = 0; i < currentLives; i++)
        {
            SpawnLife();
        }

    }

    //Spawns a Sprite prefab in the bottom left of the UI for each life the player has
    public void SpawnLife()
    {
        Instantiate(livesPrefab, livesContainer.transform.position, livesContainer.transform.rotation, livesContainer.transform);
    }







}
