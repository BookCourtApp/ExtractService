using Core.Object;

namespace Core.Extractor;

public interface IExtractorQueue
{ 
     void Enqueue(ExtractorSettings settings);
     
     ExtractorSettings Dequeue();

     bool IsEnd();
}