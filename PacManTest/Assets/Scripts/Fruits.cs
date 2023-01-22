using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fruits
{
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


