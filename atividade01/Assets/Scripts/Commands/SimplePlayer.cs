using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    public int moedas = 0;
    
    public CommandManager MyCommandManager;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
           //transform.position += Vector3.up; (erra isso
           MyCommandManager.AddCommand(new MoveUp(transform)); //moveup = encapsulamento
           MyCommandManager.DoCommand();
        }
        

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            //transform.position += Vector3.right;
            MyCommandManager.AddCommand(new MoveUp(transform));
            MyCommandManager.DoCommand();
        }

    }    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Coin"))
            {
                moedas++;
                Destroy(other.gameObject);
            }
        }
}
