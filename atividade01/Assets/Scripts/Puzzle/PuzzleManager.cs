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

        // ➕ Esconde painel de vitória se o quebra-cabeça não estiver mais completo
        if (!IsPuzzleComplete())
        {
            winPanel.SetActive(false);
            // Não esconda os botões!
            // Os botões permanecem visíveis após o Undo
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

    IEnumerator ReplaySequence()
    {
        isReplaying = true;

        skipButton.gameObject.SetActive(true);
        winPanel.SetActive(false);
        restartButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        undoButton.gameObject.SetActive(false);

        ShufflePieces();
        yield return new WaitForSeconds(1f);

        foreach (ICommand1 cmd in commandHistory)
        {
            cmd.Execute();
            yield return new WaitForSeconds(1f);
        }

        isReplaying = false;

        winPanel.SetActive(true);
        restartButton.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        undoButton.gameObject.SetActive(true);

        BringButtonsToFront();

        Debug.Log("✅ Replay finalizado!");
    }

    public void SkipReplay()
    {
        StopAllCoroutines();
        isReplaying = true;

        // ✅ Força cada peça a ficar na posição correta com base no índice
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

        // 🧩 Garante que a UI atualize as posições
        LayoutRebuilder.ForceRebuildLayoutImmediate(puzzleGrid.GetComponent<RectTransform>());

        // ❌ Esconde painel de vitória e outros botões
        winPanel.SetActive(false);
        undoButton.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        // ✅ Mostra apenas botão de reinício
        restartButton.gameObject.SetActive(true);
        restartButton.transform.SetAsLastSibling();

        isReplaying = false;

        Debug.Log("⏩ Replay pulado: peças montadas corretamente e painel escondido.");
    }




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

    void BringButtonsToFront()
    {
        restartButton.transform.SetAsLastSibling();
        replayButton.transform.SetAsLastSibling();
        skipButton.transform.SetAsLastSibling();
        undoButton.transform.SetAsLastSibling();
    }
}
