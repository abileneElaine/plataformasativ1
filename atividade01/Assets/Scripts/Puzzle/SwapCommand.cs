using UnityEngine;

// Esta classe implementa a interface ICommand1 e representa o comando de trocar duas peças
public class SwapCommand : ICommand1
{
    private Transform pieceA;
    private Transform pieceB;

    private int indexA;
    private int indexB;

    public SwapCommand(Transform a, Transform b, int aIndex, int bIndex)
    {
        pieceA = a;
        pieceB = b;
        indexA = aIndex;
        indexB = bIndex;
    }

    public void Execute()
    {
        // Cria objeto temporário para armazenar a posição original da peçaA
        GameObject temp = new GameObject("TempHolder");
        temp.transform.SetParent(pieceA.parent);
        temp.transform.SetSiblingIndex(pieceA.GetSiblingIndex());

        pieceA.SetSiblingIndex(pieceB.GetSiblingIndex());
        pieceB.SetSiblingIndex(temp.transform.GetSiblingIndex());

        GameObject.Destroy(temp);
        Debug.Log("executou");
    }

    public void Undo()
    {
        GameObject temp = new GameObject("TempHolderUndo");
        temp.transform.SetParent(pieceA.parent);
        temp.transform.SetSiblingIndex(pieceA.GetSiblingIndex());

        pieceA.SetSiblingIndex(pieceB.GetSiblingIndex());
        pieceB.SetSiblingIndex(temp.transform.GetSiblingIndex());

        GameObject.Destroy(temp);
    }
}