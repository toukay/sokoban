namespace Commands
{
    public abstract class Command: ICommand
    {
        public void Execute()
        {
            ExecuteCommand();
            CommandHistoryHandler.Instance.AddCommand(this);
        }
        
        protected abstract void ExecuteCommand();

        public abstract void Undo();

        public abstract void Redo();

        public abstract Command Clone();
    }
}