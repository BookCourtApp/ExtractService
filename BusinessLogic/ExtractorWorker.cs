using Core;
using Core.Models;
using Core.Object;
using InfrastructureProject;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogin;

public class ExtractorWorker : IExtractorWorker
{
    public ExtractorWorker(IExtractorFactory factory,ApplicationContext context)
    {
        _factory = factory;
        _context = context;
    }
    
    private IExtractorFactory _factory;
    private readonly ApplicationContext _context;

    /// <summary>
    /// Метод который запускает экстракцию
    /// </summary>
    /// <param name="settings">Настройки парсера</param>
    /// <typeparam name="T">тип данных которые парсятся</typeparam>
    public async Task ExtractDataAsync<T>(ExtractorSettings settings)
    {
        ExtractorResult result = new ExtractorResult();
        result.Status = ExtractorStatus.Created;
        
        result = (await _context.ExtractorResults.AddAsync(result)).Entity;     // промежуточное сохранение
        await _context.SaveChangesAsync();
        
        // получаем тип спаршенных данных и получаем сервис-репозиторий через фабрику
        IExtractor<T> extractor = _factory.CreateExtractor<T>(settings);
        while (!extractor.IsEndData())
        {
            try
            {

                result.Status = ExtractorStatus.Parsing;
                
                // получаем батч с экстрактора 
                ExtractBatchResult<T> extractorBatch = await extractor.ExtractNextBatch();
                
                // сохраняем батч и позволяем EF самому понять в какую таблицу записать спаршенную инфу 
                await _context.AddRangeAsync(extractorBatch.Buffer);
                
                result.Errors.AddRange(extractorBatch.Errors);
                result.ExtractorDataCount += extractorBatch.ExtratctorDataCount;
                
                _context.ExtractorResults.Update(result);
                
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Error error = new Error()
                {
                    Reason = $"Exception: {ex.Message}",
                    Type = PlaceType.Saving
                };
                result.Errors.Add(error);
                Console.WriteLine(ex.Message);
            }

        }
        
        
    }

}