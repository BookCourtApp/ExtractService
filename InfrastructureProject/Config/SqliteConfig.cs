namespace InfrastructureProject.Config;

public class SqliteConfig
{
    /// <summary>
    /// Абсолютный путь до .bd файла, будет создан новый в случае если файла не существует
    /// </summary>
    public string DbPath { get; set; }
}