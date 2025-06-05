using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class GameData
{
    public int bestScore;

    public int coins;
    public List<int> unlockedCharacters = new List<int>();
    public int selectedCharacter;

    public GameData()
    {
        bestScore = 0;
        coins = 0;
        selectedCharacter = 0;

        if (!unlockedCharacters.Contains(0))
            unlockedCharacters.Add(0);
    }
}
