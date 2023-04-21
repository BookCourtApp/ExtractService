namespace BusinessLogin.ExtTask.Queue;

public interface ITaskQueue
{
    public bool IsEnd();

    public ExtractorTask Dequeue();

    public void Enqueue(ExtractorTask extractorTask);
}