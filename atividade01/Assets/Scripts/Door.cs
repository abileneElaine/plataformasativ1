using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string doorID;  // ID da porta, usado para saber se deve reagir ao botão

    private void OnEnable()
    {
        StaticEventChannel.OnButtonPressed += OnButtonPressed; 
    }

    private void OnButtonPressed(string id)
    {
        throw new NotImplementedException();
    }
}
