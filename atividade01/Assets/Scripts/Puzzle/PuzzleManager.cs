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

    public GameObject winPanel;
    public Button restartButton;

    private List<ICommand1> commandHistory = new List<ICommand1>();
    private Stack<ICommand1> undoStack = new Stack<ICommand1>();

    private List<Piece> pieces = new List<Piece>();
    private Piece firstSelected = null;

    private bool isReplaying = false;

    private List<Transform> initialPieceOrder = new List<Transform>();

    void Start()
    {
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        winPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);

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

        // 🔧 2. Salva o estado inicial ao clicar pela primeira vez
        if (commandHistory.Count == 0)
        {
            SaveInitialSiblingOrder();
        }

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

    void SaveInitialSiblingOrder()
    {
        initialPieceOrder.Clear();
        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            initialPieceOrder.Add(puzzleGrid.GetChild(i));
        }
    }


    void RestoreInitialSiblingOrder()
    {
        for (int i = 0; i < initialPieceOrder.Count; i++)
        {
            initialPieceOrder[i].SetSiblingIndex(i);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleGrid.GetComponent<RectTransform>());
    }


    void CheckWin()
    {
        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return;
        }

        Debug.Log("🎉 Quebra-cabeça completo!");
        ShowVictoryScreen();
    }

    void ShowVictoryScreen() // 🔧 5. Exibe painel de vitória
    {
        winPanel.SetActive(true);
        restartButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(true);
        BringButtonsToFront();
    }

    public void Undo()
    {
        if (isReplaying || undoStack.Count == 0 || firstSelected != null) return;

        ICommand1 lastCommand = undoStack.Pop();
        lastCommand.Undo();

        // 🔧 Remover da lista de histórico para o replay ficar fiel ao estado real
        if (commandHistory.Count > 0)
        {
            commandHistory.RemoveAt(commandHistory.Count - 1);
        }

        if (!IsPuzzleComplete())
        {
            winPanel.SetActive(false);
        }
    }


    bool IsPuzzleComplete()
    {
        Piece[] currentPieces = puzzleGrid.GetComponentsInChildren<Piece>();
        for (int i = 0; i < currentPieces.Length; i++)
        {
            if (currentPieces[i].correctIndex != i)
                return false;
        }
        return true;
    }

    public void StartReplay()
    {
        if (isReplaying) return;
        StartCoroutine(ReplaySequence());
        winPanel.SetActive(false);
    }

    IEnumerator ReplaySequence() // 🔧 4. Agora restaura a ordem antes do replay
    {
        isReplaying = true;

        winPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(true);

        RestoreInitialSiblingOrder(); // ⚠️ ponto crucial
        yield return new WaitForSeconds(1f);

        foreach (ICommand1 cmd in commandHistory)
        {
            if (!isReplaying) yield break;

            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;
        ShowVictoryScreen();

        Debug.Log("✅ Replay concluído com sucesso.");
    }

    public void SkipReplay()
    {
        StopAllCoroutines();
        isReplaying = true;

        for (int i = 0; i < puzzleGrid.childCount; i++)
        {
            for (int j = 0; j < puzzleGrid.childCount; j++)
            {
                Piece piece = puzzleGrid.GetChild(j).GetComponent<Piece>();
                if (piece.correctIndex == i)
                {
                    piece.transform.SetSiblingIndex(i);
                    break;
                }
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleGrid.GetComponent<RectTransform>());

        isReplaying = false;
        ShowVictoryScreen();

        Debug.Log("⏩ Replay pulado: peças montadas corretamente.");
    }

    public void RestartGame()
    {
        ShufflePieces();
        commandHistory.Clear();
        undoStack.Clear();
        initialPieceOrder.Clear();

        winPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(true);

        Debug.Log("🔁 Jogo reiniciado.");
    }

    void BringButtonsToFront()
    {
        restartButton.transform.SetAsLastSibling();
        replayButton.transform.SetAsLastSibling();
        skipButton.transform.SetAsLastSibling();
        undoButton.transform.SetAsLastSibling();
    }
}
