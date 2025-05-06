using UnityEngine;

using System;

public static class StaticEventChannel
{
    // Evento estático que carrega um parâmetro string (o ID da porta)
    public static event Action<string> OnButtonPressed;

    // Método que dispara (notifica) todos os ouvintes desse evento
    public static void RaiseButtonPressed(string doorId)
    {
        // Verifica se existe alguém inscrito no evento antes de chamar
        OnButtonPressed?.Invoke(doorId);
    }
}

