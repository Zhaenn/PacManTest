using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("References")]
    public static GameManager instance;

    [Header("Point System & Dots")]
    private int currentPoints;
    public List<Dot> allDotsInLevel = new List<Dot>(); 


    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        instance = this;
        Dot[] allDots = FindObjectsOfType<Dot>();
        foreach(Dot d in allDots)
        {
            allDotsInLevel.Add(d);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPoints(int pointsToAdd)
    {
        currentPoints += pointsToAdd;
    }

    public void DotEaten(Dot dot)
    {
        allDotsInLevel.Remove(dot);

        if(allDotsInLevel.Count <= 0)
        {
            Debug.Log("Game Over");
        }
    }
}
