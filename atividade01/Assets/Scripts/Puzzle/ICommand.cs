using UnityEngine;

public interface ICommand //a interface ICommand será usada pra encapsular comandos
{
    void Execute(); //metodo que executa o comando( por exemplo, trocar duas peças)

    void Undo; //desfaz o comando ( npor exemplo, desfaz a troca)
}
