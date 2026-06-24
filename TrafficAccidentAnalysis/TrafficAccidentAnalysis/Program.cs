using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

// 1. 定義資料結構 (對應 JSON 格式)
public class AccidentRoot
{
    [JsonPropertyName("result")]
    public AccidentResult Result { get; set; }
}

public class AccidentResult
{
    [JsonPropertyName("records")]
    public List<AccidentRecord> Records { get; set; }
}

public class AccidentRecord
{
    [JsonPropertyName("發生地點")]
    public string Location { get; set; }

    [JsonPropertyName("肇因研判子類別名稱-主要")]
    public string MainCause { get; set; }

    [JsonPropertyName("當事者區分-類別-大類別名稱-車種")]
    public string VehicleType { get; set; }

    [JsonPropertyName("天候名稱")]
    public string Weather { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        string filePath = "NPA_TMA1_JSON.json";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("找不到數據檔案！");
            return;
        }

        // 2. 讀取並解析 JSON
        string jsonString = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<AccidentRoot>(jsonString, options);

        if (data?.Result?.Records != null)
        {
            var records = data.Result.Records;
            Console.WriteLine($"--- 成功載入 {records.Count} 筆 A1 類事故數據 ---\n");

            // 3. 簡單分析：統計前三大肇事原因
            var topCauses = records
                .GroupBy(r => r.MainCause)
                .OrderByDescending(g => g.Count())
                .Take(3);

            Console.WriteLine("【前三大肇事原因統計】");
            foreach (var cause in topCauses)
            {
                Console.WriteLine($"{cause.Key}: {cause.Count()} 件");
            }

            // 4. 簡單分析：統計主要事故車種
            var topVehicles = records
                .GroupBy(r => r.VehicleType)
                .OrderByDescending(g => g.Count())
                .Take(3);

            Console.WriteLine("\n【前三大涉案車種】");
            foreach (var v in topVehicles)
            {
                Console.WriteLine($"{v.Key}: {v.Count()} 件");
            }
        }

        Console.WriteLine("\n按任意鍵結束...");
        Console.ReadKey();
    }
}