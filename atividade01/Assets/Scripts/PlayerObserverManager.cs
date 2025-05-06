using System;
using UnityEngine;

//equivale ao nosso youtube
public static class PlayerObserverManager
{
    //criar canal de moedas do player
    public static event Action<int> OnMoedasChanged;
    
    //posta um video novo no canal (notifica os inscrintos)
    public static void ChangeMoedas(int valor)
    {
        OnMoedasChanged?.Invoke(valor);
    }
}

