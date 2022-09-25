# push-to-hatenablog
 GitHubとはてなブログの連携用環境。
 - Issueを利用して、記事の同期、作成することができます
 - PullRequestにて変更点を作成し、それがマージされたタイミングではてなブログの記事も更新されます

## セットアップ
### 1. GitHubリポジトリの追加
はてなブログ記事の管理用にGitHubリポジトリを作成してください。

### 2. Secretの追加
GitHubリポジトリに以下の２つのSecretを追加してください。  
GitHubアクションで記事を更新するために使用します。
* `DOMAIN`という名前でSecretを追加してください。値にはブログのドメイン名を設定してください。
* `BSY`という名前で以下のような値のSecretを追加してください。
```yaml
[ブログのドメイン]:\n
  username: [ユーザ名]\n
  password: [AtomPubのAPIキー]\n
```

※ Secretsの追加は`GitHubリポジトリページ/Settings/Secrets`から設定できます。詳細は以下のページを参照してください。

[暗号化されたシークレットの作成と保存 #暗号化されたシークレットの作成](https://help.github.com/ja/actions/configuring-and-managing-workflows/creating-and-storing-encrypted-secrets#about-encrypted-secrets)

### 3.全記事の同期
issueテンプレートから「全記事同期用テンプレート」を選択肢、issueを作成してください。
このissueを閉じると、mainブランチにすべての記事が同期されます。



### 4.ローカル環境のセットアップ (ローカル環境から利用しない場合には不要です)
#### #`blogsync.yaml`の追加
`blogsync.example.yaml`を`blogsync.yaml`に変更して、ドメイン名やユーザ名を書き換えてください。  
ローカル環境から記事を新規追加するために使用します。

`blogsync.yaml`については、以下のページを参照してください。

[x-motemen/blogsync #Configuration](https://github.com/x-motemen/blogsync#configuration)
#### `.env` の追加
.env`ファイルを作成して`DOMAIN=[ブログのドメイン]`を追加してください。


## 使い方
### はてなブログから全記事取得
- ローカル: `npm run pull` で全記事をローカルに取得します。
- Github: issueテンプレート「全記事同期用テンプレート」を使ってissueを作成し、closeしてください。詳しい利用方法はテンプレートを確認してください。


### 新規記事(下書き)の投稿
- ローカル: `npm run push -path='パス名'` で記事を下書き状態で新規作成できます
- Github: issueテンプレート「記事の追加用テンプレート」を使ってissueを作成し、closeすると記事を下書き状態で新規作成できます。詳しい利用方法はテンプレートを確認してください。

いずれの場合でも、空の記事を下書き状態で作成します。記事の編集は、作成されたエントリを編集する方式で管理します(詳しくは「編集した記事の更新」の項を参照してください)。Githubで新規記事を作成した場合は、mainブランチの記事ディレクトリ内に指定したパスで記事が作成されていますので、その記事を編集して公開してください。

注意点として、Githubのissueを使って記事を作成する場合には、記事の著者がsecretで指定したユーザーになります。例えば技術ブログなどで、著者を別のユーザーとしたい場合には、その著者がローカルから記事を作成する必要があります。
[ブログメンバーが AtomPub APIを利用できるようにしました - はてなブログ開発ブログ](https://staff.hatenablog.com/entry/2022/06/17/110608)


### 編集した記事の更新
記事を編集したブランチを作成し、そのpull-requestがmainへマージされると、差分がGithub Actionsにより、はてなブログで更新されます。

GitHubアクションの設定は以下を確認してください。

[.github/workflows/push.yml](.github/workflows/push.yml)

※ mainブランチを直接更新しても、はてなブログは更新されません。必ずブランチを作成しpull-requestを作成してください。


## Slack通知設定
### `.github/workflows/push.yml`の調整
Slackに更新ワークフローの結果を通知する場合は、[.github/workflows/push.yml](.github/workflows/push.yml)内の以下のコメントアウトを解除してください。
```yaml
# - name: notify to slack
#   uses: mm0202/action_slack-notify@master
#   if: always()
#   env:
#     SLACK_WEBHOOK_URL: ${{ secrets.SLACK_WEBHOOK_URL }}
```

### Secretの追加
`GitHubリポジトリページ/Settings/Secrets`から以下のSecretを追加してください。
| key | value
| - | - 
| SLACK_WEBHOOK_URL | Incoming Webhookで指定されたWebhook URL









