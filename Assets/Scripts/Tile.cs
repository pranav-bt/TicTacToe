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
    [SerializeField] public GameObject TileData;
    [SerializeField] private Sprite Player1Sprite;
    [SerializeField] private Sprite Player2Sprite;
    public bool IsOccupied = false;
    [HideInInspector]public int PreviouslyOccupiedByPlayer;
    [HideInInspector]public int OccupiedByPlayer;
    [HideInInspector]private Sprite PreviousSprite;
    
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
            MoveCommand NewMove = new MoveCommand(this, Row, Column);
            NewMove.ExecuteMove();
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
        PreviousSprite = GetMoveSprite();
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

        GameManager.Moves.Push(move);
        GameManager.WinConditionCheck(Row, Column, OccupiedByPlayer);
        GameManager.ChangePlayerTurn();
        GameManager.PlaySound(ClickSoundEffect);
        if (GameManager.UndoInitiatedFlag == true)
        {
            GameManager.ClearRedoStack();
        }
    }

    public void UndoTileMove()
    {
        if (!GameManager.IsGameOver)
        {
            PreviouslyOccupiedByPlayer = OccupiedByPlayer;
            EraseTileData(this.gameObject);
            GameManager.ChangePlayerTurn();
            GameManager.UndoInitiatedFlag = true;
        }
    }

    private void EraseTileData(GameObject tileToUndo)
    {
        IsOccupied = false;
        TileData.SetActive(false);
        OccupiedByPlayer = 0;
    }

    public void RedoTileMove()
    {
        if (!IsOccupied)
        {
            RestoreTile();
            GameManager.ChangePlayerTurn();
        }
    }

    private void RestoreTile()
    {
        TileData.GetComponent<SpriteRenderer>().sprite = PreviousSprite;
        OccupiedByPlayer = PreviouslyOccupiedByPlayer;
        IsOccupied = true;
        TileData.SetActive(true);
    }

    void Start()
    {
        GameManager = FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
