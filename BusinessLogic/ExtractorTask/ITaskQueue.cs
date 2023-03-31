namespace BusinessLogin.ExtractorTask;

public interface ITaskQueue
{
    public bool IsEnd();

    public TaskInfo Dequeue();

    public void Enqueue(TaskInfo taskInfo);
}