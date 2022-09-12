using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedoLogic : MonoBehaviour
{
    [SerializeField] GameManagerScript GameManager;
    GameObject TileToRedo;
    Sprite RedoSprite;
    int PreviousPlayerOccupiedBy;
    private Stack<RedoPacket> RedoStack;
    private Stack<MoveCommand> RedoCommandStack;
    [HideInInspector] public bool UndoInitiatedFlag = false;
    [HideInInspector] public bool RedoStackCorrect = true;
    // Start is called before the first frame update
    struct RedoPacket
    {
        public GameObject TileReference;
        public Sprite RedoSprite;
        public int OccupiedByPlayer;
        
        public RedoPacket(GameObject TileRef, Sprite redoSprite, int PreviousPlayerIndex) 
        { TileReference = TileRef; RedoSprite = redoSprite; OccupiedByPlayer = PreviousPlayerIndex; }
    };
   public void Undo()
    {
        if (!GameManager.IsGameOver) 
        {
            MoveCommand MovetoUndo = GameManager.Moves.Pop();
/*            int x = GameManager.Xmoves.Pop();
            int y = GameManager.Ymoves.Pop();*/
            GameObject TileToUndo = GameObject.Find($"Tile{MovetoUndo.GetX()}{MovetoUndo.GetY()}");
            TileToRedo = TileToUndo;
            CacheTileData(TileToUndo);
            RedoCommandStack.Push(MovetoUndo);
            EraseTileData(TileToUndo);
            GameManager.ChangePlayerTurn();
            UndoInitiatedFlag = true;
        }
    }

    private void CacheTileData(GameObject tileToUndo)
    {
        Tile UndoTile = tileToUndo.GetComponent<Tile>();
        RedoSprite = UndoTile.TileData.GetComponent<SpriteRenderer>().sprite;
        PreviousPlayerOccupiedBy = UndoTile.OccupiedByPlayer;
        UndoTile.PreviouslyOccupiedByPlayer = UndoTile.OccupiedByPlayer;
        RedoPacket NewRedoPacket = new RedoPacket(tileToUndo, RedoSprite, PreviousPlayerOccupiedBy);
        RedoStack.Push(NewRedoPacket);
    }

    private void EraseTileData(GameObject tileToUndo)
    {
        Tile UndoTile = tileToUndo.GetComponent<Tile>();
        UndoTile.IsOccupied = false;
        UndoTile.TileData.SetActive(false);
        //UndoTile.TileData.GetComponent<SpriteRenderer>().sprite = null;
        UndoTile.OccupiedByPlayer = 0;
    }

    public void Redo()
    {
        if (RedoStack.Count != 0 && !GameManager.IsGameOver)
        {
            RedoPacket LatestRedoPacket = RedoStack.Pop();
            MoveCommand MovetoRedo = RedoCommandStack.Pop();
            if (!MovetoRedo.GetTile().IsOccupied)
            {
                RestoreTile(MovetoRedo);
                GameManager.Moves.Push(MovetoRedo);
                GameManager.ChangePlayerTurn();
            }
        }
    }

    private void RestoreTile(MoveCommand MoveToRedo)
    {
        Tile redotile = MoveToRedo.GetTile();
        redotile.TileData.GetComponent<SpriteRenderer>().sprite = MoveToRedo.GetSprite();
        redotile.OccupiedByPlayer = redotile.PreviouslyOccupiedByPlayer;
        redotile.IsOccupied = true;
        redotile.TileData.SetActive(true);
        
/*        GameManager.Xmoves.Push(redotile.Row);
        GameManager.Ymoves.Push(redotile.Column);*/
    }

    public void ClearRedoStack()
    {
        RedoStack.Clear();
    }

    private void Start()
    {
        RedoStack = new Stack<RedoPacket>();
        RedoCommandStack = new Stack<MoveCommand>();
    }
}
