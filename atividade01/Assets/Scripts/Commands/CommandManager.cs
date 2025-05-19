using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    public List<ICommand> commands;


     public CommandManager()
     {
        commands = new List<ICommand>();
     }

     public void AddCommand(ICommand command)//recebe um comando e add ele na lista
     {
        commands.Add(command);
     }

      public void DoCommand()
     {
        commands[^1].Do();
     }

     public void UndoCommand() //tira o elemento mais recente e o remove da lista
     {
         ICommand command = commands[^1]; //^1 tira o ultimo da lista
         commands.RemoveAt(commands.Count - 1); //remove o ultimo elemento da lista
         command.Undo(); // executa essw elemento
     }
     
}