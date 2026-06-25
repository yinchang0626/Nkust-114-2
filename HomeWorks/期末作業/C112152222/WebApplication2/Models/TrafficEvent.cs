using System;
using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class TrafficEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; } = "";     // 事故、壅塞、施工、交通障礙
        public string Region { get; set; } = "";        // 北部、中部、南部、東部
        public string RoadName { get; set; } = "";
        public string Direction { get; set; } = "";     // 例如：北上、南下
        public string Location { get; set; } = "";      // 路段或公里牌
        public DateTime OccurredAt { get; set; }
        // 經緯度，供地圖標記使用（x1 為經度、y1 為緯度）
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string Description { get; set; } = "";
        public string Advice { get; set; } = "";

        // 供 Controller 與 View 使用的篩選選項（含全部）
        public static readonly List<string> Regions = new List<string>
        {
            "全部","北部","中部","南部","東部"
        };

        public static readonly List<string> EventTypes = new List<string>
        {
            "全部","事故","壅塞","施工","交通障礙"
        };

        // 假資料
        public static List<TrafficEvent> GetSampleEvents()
        {
            var now = DateTime.Now;
            return new List<TrafficEvent>
            {
                new TrafficEvent
                {
                    Id = 1,
                    EventType = "事故",
                    Region = "北部",
                    RoadName = "國道1號",
                    Direction = "南下",
                    Location = "台北系統交流道至新竹系統交流道 48-52km",
                    OccurredAt = now.AddMinutes(-40),
                    Description = "小型車追撞，佔用內側車道，影響通行。",
                    Advice = "請改走中/外側車道並預留行車距離，可能延誤約20分鐘。"
                },
                new TrafficEvent
                {
                    Id = 2,
                    EventType = "壅塞",
                    Region = "北部",
                    RoadName = "台北市忠孝東路",
                    Direction = "雙向",
                    Location = "復興北路口",
                    OccurredAt = now.AddMinutes(-15),
                    Description = "上下班尖峰造成路段壅塞，車速緩慢。",
                    Advice = "建議改道走環東大道或使用大眾運輸。"
                },
                new TrafficEvent
                {
                    Id = 3,
                    EventType = "施工",
                    Region = "中部",
                    RoadName = "中山高（國道1號）",
                    Direction = "北上",
                    Location = "雲林路段 120km",
                    OccurredAt = now.AddHours(-2),
                    Description = "路面維修，全天段封閉1小時。",
                    Advice = "配合改道標誌，注意施工人員與機具。"
                },
                new TrafficEvent
                {
                    Id = 4,
                    EventType = "交通障礙",
                    Region = "南部",
                    RoadName = "台17線",
                    Direction = "南下",
                    Location = "台南市安南區 30.2km",
                    OccurredAt = now.AddMinutes(-5),
                    Description = "落石造成單向通行管制。",
                    Advice = "請減速慢行並遵照交通指揮。"
                },
                new TrafficEvent
                {
                    Id = 5,
                    EventType = "事故",
                    Region = "東部",
                    RoadName = "台9線",
                    Direction = "北上",
                    Location = "花蓮縣秀林鄉 10km",
                    OccurredAt = now.AddHours(-1).AddMinutes(-10),
                    Description = "摩托車與自小客擦撞，現場已等待拖吊。",
                    Advice = "前往花蓮路段請小心慢行，可能延誤。"
                },
                new TrafficEvent
                {
                    Id = 6,
                    EventType = "壅塞",
                    Region = "中部",
                    RoadName = "台中市中清路",
                    Direction = "北上",
                    Location = "中清交流道附近",
                    OccurredAt = now.AddMinutes(-25),
                    Description = "大型展覽期間周邊路段車流量大。",
                    Advice = "建議避開展覽時段，使用替代道路或大眾運輸。"
                },
                new TrafficEvent
                {
                    Id = 7,
                    EventType = "施工",
                    Region = "南部",
                    RoadName = "高雄市中山一路",
                    Direction = "雙向",
                    Location = "民生路口-中正路口",
                    OccurredAt = now.AddHours(-3),
                    Description = "下水道施工，單線交替通行。",
                    Advice = "請耐心等候並依現場指揮。"
                },
                new TrafficEvent
                {
                    Id = 8,
                    EventType = "交通障礙",
                    Region = "北部",
                    RoadName = "台2線",
                    Direction = "東向",
                    Location = "基隆市七堵段",
                    OccurredAt = now.AddMinutes(-55),
                    Description = "路樹倒塌占道，需清除。",
                    Advice = "避免靠近路緣，改走替代道路或慢行。"
                }
            };
        }
    }
}
