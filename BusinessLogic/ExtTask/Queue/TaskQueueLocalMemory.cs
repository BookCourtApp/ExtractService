using Core.Extractor;

namespace BusinessLogin.ExtTask.Queue;

public class TaskQueueLocalMemory : ITaskQueue         //todo: async
{
    private readonly Queue<ExtractorTask> _queue;

    public TaskQueueLocalMemory()
    {
        _queue = new Queue<ExtractorTask>();
    }


    public bool IsEnd()
    {
        return _queue.Count == 0;
    }

    public ExtractorTask Dequeue()
    {
        return _queue.Dequeue();
    }

    public void Enqueue(ExtractorTask extractorTask)
    {
        _queue.Enqueue(extractorTask);
    }
}