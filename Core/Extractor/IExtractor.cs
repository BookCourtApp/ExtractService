using Core.Object;

namespace Core
{
    /// <summary>
    /// Интерфейс, описывающий реализацию парсеров 
    /// </summary>
    public interface IExtractor <RawDataT, OutputDataT, ResourceIterator> 
    {
        public RawDataT GetRawData(ResourceIterator resourceIterator);

        public OutputDataT Handle(RawDataT data);
        
    }
}