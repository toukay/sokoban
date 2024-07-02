using System.Collections.Generic;

namespace Commands
{
    public class CommandHistoryHandler
    {
        private static CommandHistoryHandler _instance;
        public static CommandHistoryHandler Instance => _instance ??= new CommandHistoryHandler();

        private readonly List<Command> _commands = new List<Command>();
        private int _currentCommandIndex = -1;

        private CommandHistoryHandler() { }

        public void AddCommand(Command command)
        {
            if (_currentCommandIndex < _commands.Count - 1)
                _commands.RemoveRange(_currentCommandIndex + 1, _commands.Count - _currentCommandIndex - 1);

            _commands.Add(command.Clone());
            _currentCommandIndex++;
        }

        public void Undo()
        {
            if (_currentCommandIndex < 0) return;

            _commands[_currentCommandIndex].Undo();
            _currentCommandIndex--;
        }

        public void Redo()
        {
            if (_currentCommandIndex >= _commands.Count - 1) return;

            _currentCommandIndex++;
            _commands[_currentCommandIndex].Redo();
        }
        
        public void Clear()
        {
            _commands.Clear();
            _currentCommandIndex = -1;
        }
    }
}