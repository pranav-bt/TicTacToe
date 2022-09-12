using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public bool IsAlternate = true;
    public Color Color1;
    public Color Color2;
    [SerializeField] private GameObject MouseHighlight;
    public int Row = 0;
    public int Column = 0;
    [SerializeField] public AudioClip ClickSoundEffect;
    [SerializeField] private GameManagerScript GameManager;
    [SerializeField] private UndoRedoLogic UndoRedoMethods;
    [SerializeField] public GameObject TileData;
    [SerializeField] private Sprite Player1Sprite;
    [SerializeField] private Sprite Player2Sprite;
    public bool IsOccupied = false;
    [HideInInspector]public int PreviouslyOccupiedByPlayer;
    [HideInInspector] public int OccupiedByPlayer;
    
    public void SetColor()
    {
        if (IsAlternate)
        {           
            this.GetComponent<SpriteRenderer>().color = new Color(Color1.r, Color1.g, Color1.b);

        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(Color2.r, Color2.g, Color2.b); ;
        }
    }


    private void OnMouseEnter()
    {
        MouseHighlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        MouseHighlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (!IsOccupied && !GameManager.IsGameOver)
        {
            MoveCommand NewMove = new MoveCommand(this, Row, Column, GetMoveSprite());
            NewMove.ExecuteMove();
            //ExecuteTileMove();
        }
    }

    private Sprite GetMoveSprite()
    {
        if (GameManager.CurrentplayerIndex == 1)
        {
            return Player1Sprite; 
        }
        else
        {
            return Player2Sprite;
        }
    }

    public void ExecuteTileMove(MoveCommand move)
    {
        if (GameManager.CurrentplayerIndex == 1)
        {
            TileData.SetActive(true);
            TileData.GetComponent<SpriteRenderer>().sprite = Player1Sprite;
            IsOccupied = true;
            OccupiedByPlayer = 1;
        }
        else
        {
            TileData.SetActive(true);
            TileData.GetComponent<SpriteRenderer>().sprite = Player2Sprite;
            IsOccupied = true;
            OccupiedByPlayer = 2;
        }

        GameManager.Xmoves.Push(Row);
        GameManager.Ymoves.Push(Column);
        GameManager.Moves.Push(move);
        GameManager.WinConditionCheck(Row, Column, OccupiedByPlayer);
        GameManager.ChangePlayerTurn();
        GameManager.PlaySound(ClickSoundEffect);
        if (UndoRedoMethods.UndoInitiatedFlag == true)
        {
            UndoRedoMethods.ClearRedoStack();
        }
    }

    public void UndoTileMove()
    {

    }

    public void RedoTileMove()
    {
        
    }

    void Start()
    {
        GameManager = FindObjectOfType<GameManagerScript>();
        UndoRedoMethods = FindObjectOfType<UndoRedoLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
