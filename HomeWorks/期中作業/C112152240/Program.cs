using System.Text.Json;

namespace ConsoleApp1;

public class CertificationRecord
{
    public int ym { get; set; }
    public string kinds { get; set; }
    public string kind2 { get; set; }
    public string orgid { get; set; }
    public int num1 { get; set; }
}

internal class Program
{
    static void Main(string[] args)
    {
        
        string inputPath = @"C:\Users\User\source\repos\new\ConsoleApp1\ConsoleApp1\電信設備手機輻射值審驗合格型號數量統計.json";
        string outputPath = "output.json";

        string jsonString = File.ReadAllText(inputPath);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = null
        };

        var records = JsonSerializer.Deserialize<List<CertificationRecord>>(jsonString, options);
        string outputJson = JsonSerializer.Serialize(records, options);
        File.WriteAllText(outputPath, outputJson);

        Console.WriteLine($"完成！共處理 {records.Count} 筆記錄");
        Console.WriteLine($"輸出檔：{Path.GetFullPath(outputPath)}");
    }
}