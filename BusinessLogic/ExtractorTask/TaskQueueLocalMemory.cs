namespace BusinessLogin.ExtractorTask;

public class TaskQueueLocalMemory : ITaskQueue         //todo: multyThreading
{
    private readonly Queue<TaskInfo> _queue;

    public TaskQueueLocalMemory()
    {
        _queue = new Queue<TaskInfo>();
    }


    public bool IsEnd()
    {
        return _queue.Count == 0;
    }

    public TaskInfo Dequeue()
    {
        return _queue.Dequeue();
    }

    public void Enqueue(TaskInfo taskInfo)
    {
        _queue.Enqueue(taskInfo);
    }
}