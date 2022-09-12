using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public (int,int) WinConditions;
    [SerializeField] private GameObject CurrentPlayerText;
    [SerializeField] private AudioClip GameStartClip;
    [SerializeField] private TextMeshProUGUI WinText;
    [HideInInspector] public bool IsGameOver = false;
    [HideInInspector] public int CurrentplayerIndex = 1;
    [HideInInspector]public int MaxGridX;
    [HideInInspector]public int MaxGridY;
    public Stack<MoveCommand> Moves;
    [HideInInspector] public bool UndoInitiatedFlag = false;
    public Stack<MoveCommand> RedoCommandStack;
    void Start()
    {
        PlaySound(GameStartClip);
        var GridMan = FindObjectOfType<GridManager>();
        MaxGridX = GridMan.Width ;
        MaxGridY = GridMan.Length ;
        Moves = new Stack<MoveCommand>();
        RedoCommandStack = new Stack<MoveCommand>();
    }

    // Update is called once per frame
    public void ClearRedoStack()
    {
        RedoCommandStack.Clear();
    }

    public void ChangePlayerTurn()
    {
        if (CurrentplayerIndex == 1)
        { 
            CurrentplayerIndex = 2; 
        }
        else 
        { 
            CurrentplayerIndex = 1; 
        }
        CurrentPlayerText.GetComponent<TextMeshProUGUI>().text = $"Current Player \n     Player - {CurrentplayerIndex}";
    }
    public void WinConditionCheck(int RowOftheLatestInputTile, int ColumnOftheLatestInputTile, int CurrentTileOccupiedByPlayer)
    {
        if (Moves.Count == MaxGridX * MaxGridY)
        {
            GameOver(0);
        }
        int WinCounter = 0;
        // Check Vertical Win Condition
        for(int i = 0; i < MaxGridX; i++)
        {
            GameObject currenttile = GameObject.Find($"Tile{RowOftheLatestInputTile}{i}");
            var TileComp = currenttile.GetComponent<Tile>();
            if (!TileComp.IsOccupied || CurrentTileOccupiedByPlayer != TileComp.OccupiedByPlayer)
            {
                //Break Early If condition not met early
                WinCounter = 0;
                break;
            }
            else 
            {
                WinCounter++;
            }
            if(WinCounter >= 3)
            {
                GameOver(CurrentTileOccupiedByPlayer);
            }
        }
        // Check Horizontal Win Condition
        for (int i = 0; i < MaxGridY; i++)
        {
            GameObject currenttile = GameObject.Find($"Tile{i}{ColumnOftheLatestInputTile}");
            var TileComp = currenttile.GetComponent<Tile>();
            if (!TileComp.IsOccupied || CurrentTileOccupiedByPlayer != TileComp.OccupiedByPlayer)
            {
                //Break Early If condition not met early
                WinCounter = 0;
                break;
            }
            else
            {
                WinCounter++;
            }
            if (WinCounter >= 3)
            {
                GameOver(CurrentTileOccupiedByPlayer);
            }
        }
        // Check in 4 directions
/*        for(int i = 1; i <=4; i++)
        {
            for(int j = 1; j < )
        }*/

    }

    private void GameOver(int CurrentTileOccupiedByPlayer)
    {
        IsGameOver = true;
        StartCoroutine("RestartGameAfterSeconds");
        if (CurrentTileOccupiedByPlayer == 0)
        {
            WinText.text = "DRAW!";
        }
        else 
        { 
            WinText.text = $"Player{CurrentTileOccupiedByPlayer} Wins"; 
        }
        Debug.Log($"Player{CurrentTileOccupiedByPlayer} Wins");
    }

    public void PlaySound(AudioClip AudioToPlay)
    {
        GetComponent<AudioSource>().PlayOneShot(AudioToPlay);
    }

    public void RestartGame()
    {
        if (IsGameOver)
        { SceneManager.LoadScene(0); }
        IsGameOver = false;
    }

    IEnumerator RestartGameAfterSeconds()
    {
        yield return new WaitForSeconds(5);
        RestartGame();
    }
}
