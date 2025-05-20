using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour //gerencia o jogo tdinho
{
    public Transform puzzleGrid; // Referência ao painel onde os botões (peças) estão
    public Button undiButton; // Botão de desfazer
    public Button replayButton; // Botão de ver replay
    public Button skipButton;  // Botão de pular replay
    
    private List<ICommand> commandHistory = new List<ICommand>();  // Histórico de comandos executados
    private Stack<ICommand> undoStack = new Stack<ICommand>();     // Pilha para desfazer
    private List<ICommand> replayCommands = new List<ICommand>(); // Lista de comandos do replay

    private Piece firstSelected = null; //gurada a primeira peça clicada
    private bool isReplaying = false;  // Flag para saber se está executando replay

    
    
    void Start()
    {
        //desativa os botões de replay e pular no inicio do jogo
        replayButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        
        //organiza e configura as peças
        SetupPieces()
        {
            pieces.Clear();
            
        }

        // Embaralha as peças ao iniciar
        ShufflePieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
