namespace AdventureKit.Kernel.Tasks
{
    public class TaskAutoSave : Task.Task
    {
        protected override void DoStart()
        {
            base.DoStart();

            //K.GameManager.AutoSave();

            Complete("TaskAutoSave::Complete");
        }
    }
}
