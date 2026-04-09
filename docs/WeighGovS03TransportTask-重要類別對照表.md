# WeighGov S03 TransportTask 重要類別對照表

| 類別 | 角色 | 關鍵欄位 | 狀態/規則重點 |
|---|---|---|---|
| TransportRequest | 載運任務主單 | RequestCode, VehicleId, ScheduledDate, Status, ManualOnly | Scheduled -> InProgress -> Completed；可切換 ManualOnly |
| TransportRequestItem | 任務明細 | LineNumber, MaterialId, WeighingSourceStrategy, UnitPrice, Amount | 第一筆不可引用 PreviousItem*；數量與單價需合法 |
| TransportRecord | 過磅紀錄 | SerialNumber, FirstWeight, SecondWeight, NetWeight, Status | 完成前需雙磅；可 Voided；手動修正需備註 |
| TransportOrder | 結算主單 | RequestCode, OrderDate, TotalAmount, Status | Draft 才可修改；Confirmed 前需有明細且總金額 > 0 |
| TransportOrderItem | 結算明細 | TransportRecordId, NetWeight, UnitPrice, Amount | 金額/單價不可負 |
| TransportContract | 合約主檔 | Code, VendorId, ValidFrom, ValidTo, Status | Draft/Active/Inactive/Voided；有效期檢核 |
| VendorContractSnapshot | 合約快照 | TransportContractId, VendorId, VendorName, ContractCode | 凍結歷史語意 |
| AssistantInfo | 隨車資訊快照 | AssistantNo, AssistantName | 任務與過磅紀錄均可留存 |
| WeighingSourceStrategy | 過磅策略值物件 | FirstWeightSource, SecondWeightSource | 支援 Weighbridge / VehicleTare / PreviousItem* / FixedZero |

## 備註
- 關聯（EFML 顯式）
  - TransportRequest 1..* TransportRequestItem
  - TransportOrder 1..* TransportOrderItem
- 其餘多採 FK 串接，需於 Domain/Application 明確定義讀寫邊界。
