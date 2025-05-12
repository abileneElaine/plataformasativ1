using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] public string doorId; // ID da porta que escutará os eventos

    private void OnEnable()
    {
        // Se inscreve para escutar o evento sempre que o objeto estiver ativo
        StaticEventChannel.OnButtonPressed += OnButtonPressed;
    }

    private void OnDisable()
    {
        // Remove a inscrição no evento quando o objeto for desativado/destruído
        StaticEventChannel.OnButtonPressed -= OnButtonPressed;
    }

    private void OnButtonPressed(string id)
    {
        Debug.Log("Recebi o evento para ID: " + id + " | Minha porta é: " + doorId);

        // Verifica se o ID do evento é igual ao da porta
        if (id == doorId)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Desativa completamente o GameObject da porta (desaparece e deixa de colidir)
        gameObject.SetActive(false);
    }
}