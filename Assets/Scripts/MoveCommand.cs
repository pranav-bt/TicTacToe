using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand
{
    private
    Tile tileunit;
    Sprite PreviousSprite;
    int previousx;
    int previousy;
    public
    MoveCommand(Tile currenttile, int prevX, int prevY, Sprite prevsprite)
    {
        PreviousSprite = prevsprite;
        tileunit = currenttile;
        previousx = prevX;
        previousy = prevY;
    }

    public int GetX()
    {
        return previousx;
    }

    public int GetY()
    {
        return previousy;
    }
    public Tile GetTile()
    {
        return tileunit;
    }
    public Sprite GetSprite()
    {
        return PreviousSprite;
    }
    public void ExecuteMove()
    {
        tileunit.ExecuteTileMove(this);
    }

    public void Undo()
    {

    }
}
