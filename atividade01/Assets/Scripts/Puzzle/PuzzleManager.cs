using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public Transform puzzleGrid;
    public Button undoButton;
    public Button replayButton;
    public Button skipButton;

    public GameObject winPanel;         // ➕ Painel de vitória
    public Button restartButton;        // ➕ Botão de jogar novamente

    private List<ICommand1> commandHistory = new List<ICommand1>();
    private Stack<ICommand1> undoStack = new Stack<ICommand1>();

    private List<Piece> pieces = new List<Piece>();
    private Piece firstSelected = null;

    private bool isReplaying = false;

    void Start()
    {
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        winPanel.SetActive(false); // ➕ Oculta painel de vitória no início
        restartButton.gameObject.SetActive(false); // ➕ Oculta botão de reinício no início

        SetupPieces();
        ShufflePieces();
    }

    void SetupPieces()
    {
        pieces.Clear();

        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            Transform t = puzzleGrid.GetChild(i);
            Piece p = t.GetComponent<Piece>();
            p.manager = this;
            p.SetIndex(i);
            pieces.Add(p);
        }
    }

    void ShufflePieces()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int randomIndex = Random.Range(0, pieces.Count);
            Transform a = pieces[i].transform;
            Transform b = pieces[randomIndex].transform;

            int indexA = a.GetSiblingIndex();
            int indexB = b.GetSiblingIndex();
            a.SetSiblingIndex(indexB);
            b.SetSiblingIndex(indexA);
        }
    }

    public void OnPieceClicked(Piece clicked)
    {
        if (isReplaying) return;

        if (firstSelected == null)
        {
            firstSelected = clicked;
        }
        else
        {
            Transform a = firstSelected.transform;
            Transform b = clicked.transform;

            int indexA = a.GetSiblingIndex();
            int indexB = b.GetSiblingIndex();

            SwapCommand cmd = new SwapCommand(a, b, indexA, indexB);
            cmd.Execute();

            commandHistory.Add(cmd);
            undoStack.Push(cmd);

            firstSelected = null;

            CheckWin();
        }
    }

    void CheckWin()
    {
        Debug.Log(puzzleGrid.childCount);

        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return;
        }

        Debug.Log("🎉 Quebra-cabeça completo!");

        // ➕ Mostrar vitória e botões
        winPanel.SetActive(true);
        restartButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
    }

    public void Undo()
    {
        if (isReplaying || undoStack.Count == 0 || firstSelected != null) return;

        ICommand1 lastCommand = undoStack.Pop();
        lastCommand.Undo();
    }

    public void StartReplay()
    {
        if (isReplaying) return;
        StartCoroutine(ReplaySequence());
    }

    IEnumerator ReplaySequence()
    {
        isReplaying = true;
        skipButton.gameObject.SetActive(true);
        winPanel.SetActive(false); // ➕ Oculta painel de vitória no replay
        restartButton.gameObject.SetActive(false); // ➕ Oculta botão reiniciar no começo do replay

        ShufflePieces();

        yield return new WaitForSeconds(1f);

        foreach (ICommand1 cmd in commandHistory)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
        skipButton.gameObject.SetActive(false);

        Debug.Log("✅ Replay finalizado!");
    }

    public void SkipReplay()
    {
        StopAllCoroutines();
        isReplaying = true;

        foreach (ICommand1 cmd in commandHistory)
        {
            cmd.Execute();
        }

        isReplaying = false;

        // ➕ Esconde tudo, exceto o botão de jogar novamente
        winPanel.SetActive(false);
        undoButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);

        Debug.Log("⏩ Replay pulado!");
    }

    // ➕ Método chamado pelo botão de jogar novamente
    public void RestartGame()
    {
        ShufflePieces();
        commandHistory.Clear();
        undoStack.Clear();

        winPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(true);

        Debug.Log("🔁 Jogo reiniciado.");
    }
}
