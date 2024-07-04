using System.Collections.Generic;
using UnityEngine;

namespace Commands
{
    public class CommandHistoryHandler
    {
        private static CommandHistoryHandler _instance;
        public static CommandHistoryHandler Instance => _instance ??= new CommandHistoryHandler();

        private readonly LinkedList<Command> _commands = new LinkedList<Command>();
        private LinkedListNode<Command> _currentCommandNode;
        private const int MaxHistorySize = 100;
        private int _currentIndex = -1;

        private CommandHistoryHandler()
        {
            _currentCommandNode = null;
        }

        public void AddCommand(Command command)
        {
            // Remove commands after the current node if we're in the middle of the history
            while (_currentCommandNode != null && _currentCommandNode.Next != null)
            {
                _commands.RemoveLast();
            }

            // Add the new command
            _commands.AddLast(command);
            _currentCommandNode = _commands.Last;
            _currentIndex++;

            // Enforce the maximum history size limit by removing the oldest command
            if (_commands.Count > MaxHistorySize)
            {
                _commands.RemoveFirst();
                _currentIndex--;
            }
        }

        public void Undo()
        {
            if (_currentCommandNode == null || _currentIndex < 0) return;

            _currentCommandNode.Value.Undo();
            _currentCommandNode = _currentCommandNode.Previous;
            _currentIndex--;
        }

        public void Redo()
        {
            if ((_currentCommandNode == null || _currentIndex == -1) && _commands.First == null) return;

            if (_currentCommandNode == null)
                _currentCommandNode = _commands.First;
            else if (_currentCommandNode.Next != null)
                _currentCommandNode = _currentCommandNode.Next;
            else
                return;

            _currentCommandNode.Value.Redo();
            _currentIndex++;
        }

        public void Clear()
        {
            _commands.Clear();
            _currentCommandNode = null;
            _currentIndex = -1;
        }
    }
}
