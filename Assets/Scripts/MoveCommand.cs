using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class Command
{
    public abstract void ExecuteMove();
}

public class MoveCommand : Command
{
    private
    Tile tileunit;
    int previousx;
    int previousy;
    public
    MoveCommand(Tile currenttile, int prevX, int prevY)
    {
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
    public override void ExecuteMove()
    {
        tileunit.ExecuteTileMove(this);
    }
   public void Undo()
    {
        tileunit.UndoTileMove();
    }
    public void Redo()
    {
        tileunit.RedoTileMove();
    }
}
