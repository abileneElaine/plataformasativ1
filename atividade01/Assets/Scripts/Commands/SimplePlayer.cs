using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayer : MonoBehaviour
{
    public int moedas = 0;
    
    public CommandManager MyCommandManager;

    private void Start()
    {
        MyCommandManager = new CommandManager();
    }

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
            MyCommandManager.AddCommand(new MoveRight(transform));
            MyCommandManager.DoCommand();
        }

        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            UndoLastCommand();
        }

    }    
    private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Coin"))
            {
                MyCommandManager.AddCommand(new GetCoin(other.gameObject,  this));
                MyCommandManager.DoCommand();
            }
        }

    public void UndoLastCommand()
    {
        MyCommandManager.UndoCommand();
    }
}
