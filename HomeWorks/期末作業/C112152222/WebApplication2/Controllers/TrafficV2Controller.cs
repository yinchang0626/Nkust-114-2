using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class TrafficV2Controller : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TrafficV2Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /TrafficV2 or /TrafficV2/Index
        public async Task<IActionResult> Index(string region = "全部", string eventType = "全部", string keyword = "")
        {
            List<TrafficEvent> events = new List<TrafficEvent>();
            string errorMessage = null;

            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var apiUrl = "https://rtr.pbs.gov.tw/NMP103_PbsWS/resources/roadData/opendata";

            try
            {
                var resp = await client.GetAsync(apiUrl);
                if (!resp.IsSuccessStatusCode)
                {
                    // 連線成功但回傳非 2xx，視為失敗並 fallback
                    errorMessage = $"無法取得路況資料 (HTTP {(int)resp.StatusCode})，已顯示範例資料。";
                    events = TrafficEvent.GetSampleEvents();
                }
                else
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    try
                    {
                        using var doc = JsonDocument.Parse(json);
                        events = ParsePbsTrafficJson(doc);

                        // 若解析結果為空，仍顯示空清單（需求：只有連線失敗時 fallback）
                        if (events == null)
                            events = new List<TrafficEvent>();
                    }
                    catch (JsonException)
                    {
                        // JSON 解析失敗，不 fallback（只有連線例外才 fallback）
                        errorMessage = "路況資料解析失敗，請稍後再試。";
                        events = new List<TrafficEvent>();
                    }
                }
            }
            catch (Exception)
            {
                // 連線或其他例外，才使用範例資料
                errorMessage = "無法連線至路況資料來源，已顯示範例資料。";
                events = TrafficEvent.GetSampleEvents();
            }

            // 篩選（依使用者輸入）
            if (!string.IsNullOrEmpty(region) && region != "全部")
            {
                events = events.Where(e => e.Region == region).ToList();
            }

            if (!string.IsNullOrEmpty(eventType) && eventType != "全部")
            {
                events = events.Where(e => e.EventType == eventType).ToList();
            }

            // 排序：依 OccurredAt 由新到舊（最新在前），無效或未提供時間的放在最後
            events = events
                .OrderByDescending(e => e.OccurredAt == DateTime.MinValue ? DateTime.MinValue : e.OccurredAt)
                .ToList();

            // 搜尋邏輯
            bool isSearchResult = false;
            string searchMessage = "";
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var trimmedKeyword = keyword.Trim();
                var searchResults = events.Where(e =>
                    (e.Location?.Contains(trimmedKeyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.RoadName?.Contains(trimmedKeyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Description?.Contains(trimmedKeyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Direction?.Contains(trimmedKeyword, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (e.Advice?.Contains(trimmedKeyword, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

                if (searchResults.Count == 0)
                {
                    // 搜尋無結果，保留原本的區域事件清單，顯示回退提示
                    var regionDisplay = region == "全部" ? "全台" : region;
                    searchMessage = $"查無「{trimmedKeyword}」相關資料，以下顯示{regionDisplay}最新交通事件。";
                    isSearchResult = false;
                }
                else
                {
                    // 搜尋有結果
                    events = searchResults;
                    isSearchResult = true;
                }
            }

            ViewBag.Regions = TrafficEvent.Regions;
            ViewBag.EventTypes = TrafficEvent.EventTypes;
            ViewBag.SelectedRegion = region ?? "全部";
            ViewBag.SelectedEventType = eventType ?? "全部";
            ViewBag.Keyword = keyword ?? "";
            ViewBag.SearchMessage = searchMessage;
            ViewBag.IsSearchResult = isSearchResult;
            ViewBag.TotalCount = events.Count;
            ViewBag.ErrorMessage = errorMessage;

            return View(events);
        }

        // GET: /Traffic/Map
        public async Task<IActionResult> Map()
        {
            List<TrafficEvent> events = new List<TrafficEvent>();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);
            var apiUrl = "https://rtr.pbs.gov.tw/NMP103_PbsWS/resources/roadData/opendata";

            try
            {
                var resp = await client.GetAsync(apiUrl);
                if (!resp.IsSuccessStatusCode)
                {
                    events = TrafficEvent.GetSampleEvents();
                }
                else
                {
                    var json = await resp.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(json);
                    events = ParsePbsTrafficJson(doc);
                }
            }
            catch
            {
                events = TrafficEvent.GetSampleEvents();
            }

            // 只保留有經緯度的資料用於地圖
            var mapEvents = events.Where(e => e.Latitude.HasValue && e.Longitude.HasValue && e.Latitude.Value != 0 && e.Longitude.Value != 0).ToList();

            return View(mapEvents);
        }

        // 解析警廣（PBS）路況 JSON 到 TrafficEvent 清單
        private List<TrafficEvent> ParsePbsTrafficJson(JsonDocument doc)
        {
            var list = new List<TrafficEvent>();

            JsonElement root = doc.RootElement;
            JsonElement arrayElement = root;

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("data", out var dataProp) && dataProp.ValueKind == JsonValueKind.Array)
                {
                    arrayElement = dataProp;
                }
                else
                {
                    // 找到第一個陣列
                    foreach (var prop in root.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == JsonValueKind.Array)
                        {
                            arrayElement = prop.Value;
                            break;
                        }
                    }
                }
            }

            if (arrayElement.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var item in arrayElement.EnumerateArray())
            {
                try
                {
                    var ev = new TrafficEvent();

                    // UID
                    if (item.TryGetProperty("UID", out var uidProp))
                    {
                        if (uidProp.ValueKind == JsonValueKind.Number && uidProp.TryGetInt32(out var id))
                            ev.Id = id;
                        else if (uidProp.ValueKind == JsonValueKind.String && int.TryParse(uidProp.GetString(), out var id2))
                            ev.Id = id2;
                        else
                            ev.Id = uidProp.GetHashCode();
                    }

                    // roadtype
                    ev.EventType = item.TryGetProperty("roadtype", out var rt) ? (rt.GetString() ?? "") : "";
                    ev.EventType = MapEventType(ev.EventType);

                    // region
                    var regionRaw = item.TryGetProperty("region", out var reg) ? (reg.GetString() ?? "") : "";
                    ev.Region = MapRegionCode(regionRaw);

                    ev.RoadName = item.TryGetProperty("road", out var road) ? (road.GetString() ?? "") : "";
                    ev.Direction = item.TryGetProperty("direction", out var dir) ? (dir.GetString() ?? "") : "";
                    ev.Location = item.TryGetProperty("areaNm", out var area) ? (area.GetString() ?? "") : "";

                    // 若 road 欄位為空，嘗試從 areaNm 或 comment 中擷取道路名稱，否則填入「未提供道路名稱」
                    if (string.IsNullOrWhiteSpace(ev.RoadName))
                    {
                        // 優先使用 areaNm（若包含道路名稱）
                        if (!string.IsNullOrWhiteSpace(ev.Location))
                        {
                            ev.RoadName = ev.Location;
                        }
                        else
                        {
                            // 從 comment 中嘗試以關鍵詞或正規表達式抓出道路名稱
                            var commentText = item.TryGetProperty("comment", out var commForRoad) ? (commForRoad.GetString() ?? "") : "";
                            var roadFromComment = ExtractRoadNameFromText(commentText);
                            if (!string.IsNullOrWhiteSpace(roadFromComment))
                            {
                                ev.RoadName = roadFromComment;
                            }
                            else
                            {
                                ev.RoadName = "未提供道路名稱";
                            }
                        }
                    }

                    // happendate + happentime
                    string dateStr = item.TryGetProperty("happendate", out var hd) ? (hd.GetString() ?? "") : "";
                    string timeStr = item.TryGetProperty("happentime", out var ht) ? (ht.GetString() ?? "") : "";
                    ev.OccurredAt = ParseDateTimeFlexible(dateStr, timeStr);

                    ev.Description = item.TryGetProperty("comment", out var comm) ? (comm.GetString() ?? "") : "";
                    ev.Advice = item.TryGetProperty("srcdetail", out var src) ? (src.GetString() ?? "") : "";

                    // 如果 roadtype 未被判斷為壅塞，則從 comment/description 判斷是否為壅塞
                    // 先用 description (comment) 內容進行關鍵字匹配
                    var descForCheck = ev.Description ?? string.Empty;
                    if (!IsCongestionType(ev.EventType) && ContainsCongestionKeyword(descForCheck))
                    {
                        ev.EventType = "壅塞";
                    }

                    // 若沒有提供生活建議（Advice），依事件類型自動產生建議
                    if (string.IsNullOrWhiteSpace(ev.Advice))
                    {
                        ev.Advice = GenerateAdviceByEventType(ev.EventType);
                    }

                    // 解析座標 x1 (經度), y1 (緯度)
                    if (item.TryGetProperty("x1", out var x1Prop))
                    {
                        var x1Str = x1Prop.ValueKind == JsonValueKind.Number ? x1Prop.GetRawText() : x1Prop.GetString();
                        if (double.TryParse(x1Str, NumberStyles.Any, CultureInfo.InvariantCulture, out var lon))
                        {
                            ev.Longitude = lon;
                        }
                    }
                    if (item.TryGetProperty("y1", out var y1Prop))
                    {
                        var y1Str = y1Prop.ValueKind == JsonValueKind.Number ? y1Prop.GetRawText() : y1Prop.GetString();
                        if (double.TryParse(y1Str, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
                        {
                            ev.Latitude = lat;
                        }
                    }

                    list.Add(ev);
                }
                catch
                {
                    continue;
                }
            }

            return list;
        }

        private DateTime ParseDateTimeFlexible(string dateStr, string timeStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr) && string.IsNullOrWhiteSpace(timeStr))
                return DateTime.MinValue;

            var combined = (dateStr + " " + timeStr).Trim();

            var formats = new[] {
                "yyyyMMdd HHmmss", "yyyyMMdd HHmm", "yyyyMMdd HH:mm:ss", "yyyyMMdd HH:mm",
                "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd HH:mm", "yyyy/MM/dd HH:mm:ss", "yyyy/MM/dd HH:mm",
                "yyyyMMdd", "yyyy-MM-dd", "yyyy/MM/dd"
            };

            if (DateTime.TryParseExact(combined, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                return dt;

            if (DateTime.TryParseExact(dateStr, new[] { "yyyyMMdd", "yyyy-MM-dd", "yyyy/MM/dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
            {
                if (TimeSpan.TryParseExact(timeStr, new[] { "hhmmss", "hhmm", "HHmmss", "HHmm", "HH:mm:ss", "HH:mm" }, CultureInfo.InvariantCulture, out var ts))
                {
                    return d.Date + ts;
                }
                if (TimeSpan.TryParse(timeStr, out var ts2))
                {
                    return d.Date + ts2;
                }
                return d;
            }

            if (DateTime.TryParse(combined, out var dt2))
                return dt2;

            return DateTime.MinValue;
        }

        private string MapEventType(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return "交通障礙";
            raw = raw.Trim();
            if (raw.Contains("事故")) return "事故";
            if (raw.Contains("壅塞") || raw.Contains("塞車")) return "壅塞";
            if (raw.Contains("施工")) return "施工";
            if (raw.Contains("障礙")) return "交通障礙";
            return raw;
        }

        // 判斷是否已經是壅塞型別
        private bool IsCongestionType(string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType)) return false;
            var t = eventType.Trim();
            return t == "壅塞" || t == "塞車";
        }

        // 從描述中判斷是否包含壅塞相關關鍵字
        private bool ContainsCongestionKeyword(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            var keywords = new[] { "壅塞", "車多", "回堵", "車流緩慢", "車速緩慢", "走走停停", "排隊", "時速" };
            foreach (var k in keywords)
            {
                if (text.Contains(k)) return true;
            }
            return false;
        }

        private string MapRegionCode(string code)
        {
            if (string.IsNullOrEmpty(code)) return "其他";
            code = code.Trim().ToUpperInvariant();
            return code switch
            {
                "N" => "北部",
                "M" => "中部",
                "S" => "南部",
                "E" => "東部",
                "A" => "全部",
                _ => code,
            };
        }

        // 嘗試從文字中擷取道路名稱的簡單方法
        private string ExtractRoadNameFromText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            // 常見道路中文字，例如：國道1號、台9線、台17線、中山高速、中山高
            var patterns = new[]
            {
                @"國道\s*\d+號",
                @"台\s*\d+線",
                @"省道\s*\d+線",
                @"中山高|中山高速|中山高速公路",
                @"國道\s*\d+",
                @"\w+路|\w+街|\w+道"
            };

            foreach (var p in patterns)
            {
                var m = Regex.Match(text, p);
                if (m.Success)
                    return m.Value;
            }

            return null;
        }

        private string GenerateAdviceByEventType(string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType)) return "請注意路況並小心駕駛。";
            eventType = eventType.Trim();
            return eventType switch
            {
                "事故" => "建議減速慢行，保持安全距離，必要時改道。",
                "壅塞" => "建議避開尖峰路段，改走替代道路。",
                "施工" => "請依現場標誌行駛，注意施工人員與機具。",
                "交通障礙" => "請提前變換車道，注意路面障礙物。",
                _ => "請注意路況並小心駕駛。",
            };
        }
    }
}
