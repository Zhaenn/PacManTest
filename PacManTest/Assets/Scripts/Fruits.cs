using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fruits
{
    //Fruits is a class used to determine which fruit spawns in the center of the level when half the dots are eaten
    //Currently, the fruit is chosen according to level with a Dictionary in the Game Manager class.

    public int points;
    public Sprite sprite;
    public string fruitName;   

    public Fruits(int newPoints, string newFruitName, Sprite newSprite)
    {
        points = newPoints;
        fruitName = newFruitName;
        sprite = newSprite;       
    }
}


