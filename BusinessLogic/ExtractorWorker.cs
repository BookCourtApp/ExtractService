namespace Core;

public class ExtractorWorker : IExtractorWorker
{
    private IExtractorFactory _factory;

    public void ExtractData(string setings)
    {
        
        IExtractor extractor = _factory.CreateExtractor(setings);
        bool examExtract = extractor.IsEndData(); //todo: Записываем данные о массиве в переменную examExtract 
        while (examExtract == true)
        {
            Guid extractorBatch = extractor.ExtractNextBatch();
            //todo: Данный while должен принимать значение Batch и записывать его в репозиторий (ExtractorResultRepository)
            // Если данные в Batch отсутствуют, меняем значение переменной examExtract == false
            if (extractorBatch == null)
            {
                break;
            }
        }
        
    }

    public ExtractorWorker(IExtractorFactory factory)
    {
        _factory = factory;
    }

}