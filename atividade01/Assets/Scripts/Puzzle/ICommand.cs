using UnityEngine;
using System.Collections.Generic;



public interface ICommand1//a interface ICommand será usada pra encapsular comandos
{
    void Execute(); //metodo que executa o comando( por exemplo, trocar duas peças)

    void Undo(); //desfaz o comando ( npor exemplo, desfaz a troca)
}
