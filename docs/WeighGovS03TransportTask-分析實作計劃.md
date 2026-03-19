# WeighGov S03 TransportTask 分析實作計劃

## 1) 目標與範圍
- 目標：先建立可落地的領域理解，再進入程式實作，降低誤解風險。
- 核心來源：
	- src/Further.Weigh.HttpApi.Host/DataModel/WeighGovS03TransportTask.efml
	- src/Further.Weigh.HttpApi.Host/DataModel/WeighGovS03TransportTask.edps
- 本階段不變更資料庫、不產生 migration。

## 2) 情境推測（依目前模型）
1. 載運申請（TransportRequest）
2. 過磅紀錄（TransportRecord）
3. 結算單（TransportOrder）

整體像是「任務派送 → 過磅收斂 → 金額結算」流程，並包含政府/廠商車輛、手動模式、雙磅來源策略。

## 3) 重要類別（優先順序）
### P0（先分析）
- TransportRequest：任務主單與狀態流轉。
- TransportRequestItem：明細與過磅來源策略。
- TransportRecord：雙磅、淨重、來源追蹤與手動修正。
- TransportOrder：結算主單、確認與作廢。
- TransportOrderItem：結算明細與金額。

### P1（支撐）
- TransportContract：合約邊界與有效期。
- VendorContractSnapshot：快照，避免歷史資料漂移。
- AssistantInfo：隨車人員快照。
- WeighingSourceStrategy：第一磅/第二磅來源規則。

## 4) 已辨識關係
- One-to-Many
	- TransportRequest -> TransportRequestItems
	- TransportOrder -> TransportOrderItems
- 其餘多由 FK 欄位串接（如 TransportRequestId、TransportRecordId），需在程式層補足語意。

## 5) 第一輪交付（本週）
1. 產出「欄位/狀態/不變條件」對照表。
2. 產出「流程事件與狀態轉移圖」草稿。
3. 產出「風險與未知清單」（含缺文件項目）。

## 6) 實作步驟（開始執行）
- Step A：建立類別責任矩陣（已開始）
	- 定義每個 AggregateRoot 的建立、修改、完成、作廢入口。
- Step B：建立狀態轉移清單
	- Request: Scheduled -> InProgress -> Completed
	- Record: InProgress -> Completed / Voided
	- Order: Draft -> Confirmed / Voided
- Step C：建立資料一致性檢核清單
	- 雙磅資料完整性
	- 手動模式限制
	- 金額重算規則

## 7) 風險與待確認
- docs 目前無對應流程文件，商業語意多來自命名推測。
- S03 與 S04 邊界尚未有正式文件證實。
- .edps 列出大量預期產物，需核對實際程式是否已生成且一致。

## 8) 下一步（可直接接續）
1. 建立「實體欄位對照表」文件（Request/Record/Order）。
2. 建立「狀態轉移與例外代碼」文件。
3. 建立「從 EFML 到 ABP 層（Domain/Application/EFCore）對應清單」。

