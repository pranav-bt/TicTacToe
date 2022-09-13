using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameManagerScript GameManager;
    [SerializeField] AudioClip UndoRedoSound;
    // Start is called before the first frame update
    public void UndoButton()
    {
        if (GameManager.Moves.Count != 0 && !GameManager.IsGameOver)
        {
            MoveCommand MovetoUndo = GameManager.Moves.Pop();
            MovetoUndo.Undo();
            GameManager.PlaySound(UndoRedoSound);
            GameManager.RedoCommandStack.Push(MovetoUndo);
        }
    }
    public void RedoButton()
    {
        if (GameManager.RedoCommandStack.Count != 0 && !GameManager.IsGameOver)
        {
            MoveCommand MovetoRedo = GameManager.RedoCommandStack.Pop();
            MovetoRedo.Redo();
            GameManager.PlaySound(UndoRedoSound);
            GameManager.Moves.Push(MovetoRedo);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
