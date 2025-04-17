---
marp: true
theme: default
class: lead
paginate: true
---

# 🎒バックパックAIアシスタント  
## 在庫管理 × 自然言語によるデータ検索

---

## 📌 アプリの要点

- Azure上のSQLデータベースに登録されたバックパック製品データ
- Blazor + EF Core + Azure OpenAI を活用
- 自然言語をSQLに変換してデータ操作（クエリ）が可能
- ユーザーは「SQLを知らなくても」データを扱える！

---

## 📦 データの概要

- 元データ：CSVからインポート　(Kaggle Backpack challenge)
- 主なカラム：
  - ブランド名（brand）
  - 価格（price）
  - サイズ・スタイル・色
  - 防水・容量など
- Azure SQL にホストされたリレーショナルデータ

---

## 🛠️ 基本機能：CRUD操作

- **Create**: 新しい製品を追加
- **Read**: 一覧表示・ページネーション
- **Update**: 既存データの編集
- **Delete**: レコードの削除（確認付き）

💡 開発者・非開発者どちらでも直感的に使えるUI

---

## 🧠 AI Assistant 機能

- 自然言語での質問に対応（例：「Nikeのバック何個あるかを教えて」）
- 裏で以下を自動実行：
  - 自然言語 → SQL変換（OpenAI）
  - Azure SQL にクエリ実行
  - 結果を自然言語に要約（OpenAI 2回目呼び出し）
  - チャートによる可視化（JS連携）//開発途中

---

## 🔒 安全設計：Prompt Engineering

- `DELETE`, `UPDATE` 禁止（AIが勝手に消したり変更しない）
- `SELECT` 文のみ生成するよう制約を設定
- SQL文の検証ロジック付き（明示的に `SELECT` でなければUIに返す）

---

## 🔄 開発フロー：CI/CD構成

- GitHub Actionsによる自動ビルド・テスト（CI）
- DockerイメージをACRにPush
- Azure App Serviceに自動デプロイ（CD）
- ステージング → 本番への安全な移行が可能

---

## 🚀 今後の拡張可能性

- 🔍 データ参照のフィルター・ソート機能の強化
- 📊 チャート種類の追加（比較、可視化）
- 🗣️ Chat UI化（対話型インターフェース）

