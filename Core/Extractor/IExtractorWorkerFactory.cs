namespace Core.Extractor;

public interface IExtractorWorkerFactory
{
    IExtractorWorker CreateWorker();
}