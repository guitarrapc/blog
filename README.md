# blog

はてなブログ記事とGitHub-はてなブログ連携用環境です。

- Issueを利用して、記事の同期、作成できます
- PullRequestにて変更点を作成し、それがマージされたタイミングではてなブログの記事も更新されます

# セットアップ

## 1. GitHubリポジトリの追加

はてなブログ記事の管理用にGitHubリポジトリを作成してください。

## 2. Secretの追加

GitHubリポジトリに以下の２つのSecretを追加してください。GitHubアクションで記事を更新するために使用します。

| key | value |
| - | - |
| DOMAIN | ブログのドメイン名 |
| BSY | ↓のYAML |

```yaml
[ブログのドメイン]:
  username: [ユーザ名]
  password: [AtomPubのAPIキー]
```

## 3.全記事の同期

issueテンプレートから「全記事同期用テンプレート」を選択肢、issueを作成してください。このissueを閉じると、mainブランチにすべての記事が同期されます。

> [!TIP]
> github actions の`記事同期`で workflow_dispatch で実行しても同期できます。

## 4.ローカル環境のセットアップ

> [!NOTE]
> ローカル環境から利用しないなら不要です。

### `blogsync.yaml`の追加

ローカル環境から記事を新規追加するために使用します。

`blogsync.example.yaml`を`blogsync.yaml`に変更して、ドメイン名やユーザ名を書き換えてください。`blogsync.yaml`については、以下のページを参照してください。

[x-motemen/blogsync #Configuration](https://github.com/x-motemen/blogsync#configuration)

### `.env`の追加

.env`ファイルを作成して`DOMAIN=[ブログのドメイン]`を追加してください。

## 5.VS Codeで記事を書く準備

textlintと各種ルールをセットアップします。

```shell
npm install --save-dev textlint textlint-rule-no-mixed-zenkaku-and-hankaku-alphabet textlint-rule-period-in-list-item textlint-rule-preset-ja-spacing textlint-rule-preset-ja-technical-writing textlint-rule-spellcheck-tech-word
```

### VSCodeの拡張機能をインストール

[vscode-textlint](https://marketplace.visualstudio.com/items?itemName=taichi.vscode-textlint) をインストールして、VSCodeでtextlintを実行できるようにします。

# 使い方

## はてなブログから全記事取得

- ローカル: `npm run pull`で全記事をローカルに取得します
- Github: issueテンプレート「全記事同期用テンプレート」を使ってissueを作成し、closeしてください。詳しい利用方法はテンプレートを確認してください

> [!NOTE]
> GitHubアクションの設定は[.github/workflows/pull_articles.yaml](.github/workflows/pull_articles.yaml)を確認してください。

## 新規記事(下書き)の投稿

次の方法を用いて、空の記事を下書き状態で作成します。

- ローカル: `npm run push -path='パス名'`で記事を下書き状態で新規作成します
- Github: issueテンプレート「記事の追加用テンプレート」を使ってissueを作成し、closeすると記事を下書き状態で新規作成します

> [!NOTE]
> GitHubアクションの設定は[.github/workflows/post_draft.yaml](.github/workflows/post_draft.yaml)を確認してください。

> [!TIP]
> Githubで新規記事を作成した場合は、mainブランチの記事ディレクトリ内に指定したパスで記事が作成されています。

## 編集した記事の更新

作成した記事を編集したブランチを作成し、そのpull-requestがmainへマージされると、Github Actionsで差分記事をはてなブログに更新します。

> [!NOTE]
> GitHubアクションの設定は[.github/workflows/push_article.yaml](.github/workflows/push_article.yaml)を確認してください。

> [!WARNING]
> mainブランチを直接更新しても、はてなブログは更新されません。必ずブランチを作成しpull-requestを作成してください。

# Slack通知設定

## `.github/workflows/push_article.yaml`の調整

Slackに更新ワークフローの結果を通知する場合は、[.github/workflows/push_article.yaml](.github/workflows/push_article.yaml)内の以下のコメントアウトを解除します。

```yaml
# - name: notify to slack
#   uses: mm0202/action_slack-notify@master
#   if: always()
#   env:
#     SLACK_WEBHOOK_URL: ${{ secrets.SLACK_WEBHOOK_URL }}
```

## Secretの追加

`GitHubリポジトリページ/Settings/Secrets`から以下のSecretを追加してください。

| key | value |
| - | - |
| SLACK_WebHOOK_URL | Incoming Webhookで指定されたWebhook URL |
