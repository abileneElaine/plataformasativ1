using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    [SerializeField] private string doorIDOpen; //ID da porta pra o botão abrir

    private void OnTriggerEnter2D(Collider2D other)
    {
        //verifica se o que entrou no botão tem a tag "Player"
        object staticEventChannel;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Botão pressionado, enviando ID: " + doorIDOpen);

            //emite um evento dizendo que a porta com esse ID deve abrir
            StaticEventChannel.RaiseButtonPressed(doorIDOpen);
        }
    }
}
