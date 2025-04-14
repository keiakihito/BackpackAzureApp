---
marp: true
theme: default
paginate: true
---

# BackpackAzureApp 技術レビュー

プロジェクトの全体像を振り返り、技術的構成を理解しやすくするためのマルチカテゴリ解説。

---

## 1. データ加工（Python）

- フォルダ: `process/data/proceed_data.csv`
- 使用ライブラリ: `pandas`, `pandas-profiling`
- 処理内容:
  - CSVインポート
  - 欠損値削除
  - 列名の標準化
  - pandas-profilingでEDA
- 出力: 加工済みのCSVをBlazorアプリに取り込む準備

---

## 2. 環境設定（Docker など）

- `Dockerfile`（ASP.NET + Blazor）
- `docker-compose.yml`
  - app + db構成（MySQL or Azure SQL）
- `.env` による接続文字列管理
- ポート設定 / volumeマウント

---

## 3. アーキテクチャ（Blazor MVC構造）

- 使用: **Blazor Web App (.NET 8)**
- `Pages/` → UI層（.razor ファイル）
- `Models/` → DBモデル定義（DataAnnotationsあり）
- `Data/` → `AppDbContext.cs`
- `Services/` 層があればロジック分離

---

## 4. データベースセットアップ（EF Core）

- `AppDbContext.cs` にてDB構成
- Migration (`dotnet ef migrations`) を利用
- Azure SQLと接続
- 初期データシード含む

---

## 5. CRUD機能（Blazor）

- 一覧表示：`QuickGrid` コンポーネント
- 作成：Create.razor
- 編集：Edit.razor
- 削除：Delete.razor
- バリデーション：DataAnnotations

---

## 6. Unit Test / CI

- Unit Test: `xUnit` による `Model` / `DbContext` テスト
- CI: `.github/workflows/dotnet.yml`
  - `dotnet test` 実行
  - 成功/失敗で通知可能

---

## 7. OpenAI API 連携

- サービス: Azure OpenAI (gpt-3.5 or gpt-4)
- 機能: 自然言語 → SQL生成 + 要約生成
- 使用パーツ:
  - `AzureOpenAIService.cs`
  - `prompt` テンプレート分離
  - レスポンスの整形処理

---

## 8. Azureでのデプロイ

- 使用サービス：Azure App Service / Container App
- Dockerイメージをbuildしてpush
- GitHub Actions経由でデプロイ自動化

---

## 9. UI/UXカスタマイズ

- `wwwroot/css/app.css` にカスタムスタイル
- Sidebar ナビゲーションの hover / active ハイライト
- レイアウト分離：`MainLayout.razor`

---

## 10. ログ・エラーハンドリング

- 入力バリデーション（DataAnnotations）
- try-catchでの安全なDB更新
- エラーメッセージのユーザー表示

---

## 11. サンプルデータ・シード処理

- `DbInitializer.cs` で初期データ追加
- アプリ起動時に自動チェック
- `HasData()` によるMigration対応のシードも可

---

## 12. 設計パターン・分離構成

- DI（依存性注入）: `builder.Services.AddScoped<>`
- サービス層とUI層の分離
- Interfaceベース設計も適用可能

---

## まとめ

このプロジェクトは、Azure、OpenAI、Blazor、EF Coreといったモダンな技術スタックをバランス良く組み合わせたフルスタックアプリ。

カテゴリごとの理解を深めることで、面接時の技術説明力を向上させる。

---

## 次のステップ

- 各カテゴリごとのコードベース解説
- 質疑応答に備えた一問一答スライド化
- 成果物PDFとしてポートフォリオ添付

