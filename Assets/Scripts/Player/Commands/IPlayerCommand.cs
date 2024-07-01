namespace Player.Commands
{
    public interface IPlayerCommand
    {
        void Execute();
        void Undo();
        void Redo();
    }
}