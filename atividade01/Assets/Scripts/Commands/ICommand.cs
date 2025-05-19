using UnityEngine;

public interface ICommand
{
    void Do();

    void Undo();
}
