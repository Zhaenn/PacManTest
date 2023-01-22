using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Data")]
    private int currentLives = 5;
    private int currentLevel = 1;
    private PlayerMovement pacman;

    [Header ("References")]
    public static GameManager instance;
    public GameObject Pacman;
    public GameObject currentScoreValueText;
    public GameObject highScoreValueText;
    public GameObject livesPrefab;
    public GameObject livesContainer;

    [Header("Point System & Dots")]
    private Dot[] allDots;
    private int currentPoints;
    private int currentHighScore;
    public List<Dot> allDotsInLevel = new List<Dot>();
    private int dotNumber;
    public List<Fruits> allFruits = new List<Fruits>();
    public GameObject fruit;
    private bool fruitSpawned = false;

    [Header("Sprite References")]
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
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        instance = this;
        pacman = FindObjectOfType<PlayerMovement>();

        allDots = FindObjectsOfType<Dot>();
        dotNumber = allDots.Length;

        foreach(Dot d in allDots)
        {
            allDotsInLevel.Add(d);
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

        foreach(Fruits f in fruitsDic.Values)
        {
            allFruits.Add(f);
        }

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PacmanDeath();
        }
    }

    public void AddPoints(int pointsToAdd)
    {
        currentPoints += pointsToAdd;
        currentScoreValueText.GetComponent<TMPro.TextMeshProUGUI>().text = currentPoints.ToString();
        
        if(currentPoints > currentHighScore)
        {
            currentHighScore = currentPoints;
            highScoreValueText.GetComponent<TMPro.TextMeshProUGUI>().text = currentHighScore.ToString();
        }
    }

    public void DotEaten(Dot dot)
    {
        allDotsInLevel.Remove(dot);


        if(allDotsInLevel.Count <= 0)
        {
            Debug.Log("Finished Level");
        } else if(allDotsInLevel.Count < dotNumber / 2 && !fruitSpawned)
        {
            SpawnFruit();
            fruitSpawned = true;
        }
    }

    public void SpawnFruit()
    {
        Fruits fruitToSpawn = ChooseFruit();
        Transform spawnPoint = transform.Find("FruitSpawnPoint");
        GameObject newFruit = Instantiate(fruit, spawnPoint.position, transform.rotation, spawnPoint);
        newFruit.name = fruitToSpawn.fruitName;
        newFruit.GetComponent<Interactible>().points = fruitToSpawn.points;
        newFruit.GetComponent<SpriteRenderer>().sprite = fruitToSpawn.sprite;
    }

    public Fruits ChooseFruit()
    {
        if(currentLevel - 1 > allFruits.Count)
        {
            return allFruits[allFruits.Count];
        }
        else
        {
            return allFruits[currentLevel - 1];
        }
    }

    public void UpdateUI()
    {
        //Spawn lives according to how many we have at the start
        for(int i = 0; i < currentLives; i++)
        {
            Instantiate(livesPrefab, livesContainer.transform.position, livesContainer.transform.rotation, livesContainer.transform);
        }

    }

    public void PacmanDeath()
    {
        currentLives--;

        if(currentLives > 0)
        {
            ResetGame();
        }
        else
        {
            GameOver();
        }

        
    }

    public void ResetGame()
    {
        //Reset player position and all ghosts
        pacman.transform.position = pacman.startPosition;
       
    }

    public void GameOver()
    {
        currentPoints = 0;
        foreach (Dot d in allDots)
        {
            d.gameObject.SetActive(true);
        }
    }
}
