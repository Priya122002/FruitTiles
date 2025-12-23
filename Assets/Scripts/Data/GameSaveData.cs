using System;
using System.Collections.Generic;

[Serializable]
public class GameSaveData
{
    public int rows;
    public int columns;

    public int score;
    public int turns;
    public int matches;

    public List<int> cardIds;       
    public List<bool> cardRemoved;     
}
