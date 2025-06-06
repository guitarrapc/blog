# blog

はてなブログ記事とGitHub-はてなブログ連携用環境です。

[hatena/Hatena-Blog-Workflows-Boilerplate](https://github.com/hatena/Hatena-Blog-Workflows-Boilerplate)をベースに、はてなブログの記事をGitHubで管理するための環境を構築します。

# VS Codeで記事を書く

Nodeをインストールしておきます。

```shell
nvm install lts
nvm use lts
```

textlintと各種ルールをセットアップします。

```shell
npm install --save-dev textlint textlint-rule-no-mixed-zenkaku-and-hankaku-alphabet textlint-rule-period-in-list-item textlint-rule-preset-ja-spacing textlint-rule-preset-ja-technical-writing textlint-rule-spellcheck-tech-word textlint-filter-rule-comments
```

VS Codeの拡張機能をインストールします。

[vscode-textlint](https://marketplace.visualstudio.com/items?itemName=taichi.vscode-textlint)をインストールして、VS Codeでtextlintを実行できるようにします。

## textlintを実行する

VS Codeで記事を書いているとリアルタイムで校正されます。PROBLEMSタブを開いてエラーを確認できます。

ローカルでtextlintを実行するには、以下のコマンドを実行します。

```shell
npm run lint
```

<!-- textlint-disable -->

# HatenaBlog Workflows Boilerplate(β)

- このBoilerplateは、企業がはてなブログで技術ブログを運営する際のレビューや公開作業など、運営ワークフローを支援する目的で作成しています
- GitHub上で、はてなブログとの記事の同期、下書きの作成・編集・公開、公開記事の編集などを行うことができます。下書きの作成時にプルリクエストが作成されるため、記事のレビューなどの業務のワークフローに組み込むことが容易になります
- 本機能はベータ版です。正常に動作しない可能性がありますが、予めご了承下さい


## セットアップ

1. 本リポジトリトップに表示されている、「Use this templateボタンクリック > Create a new repository」から、新規にリポジトリを作成する
    - ![Use this templateボタンの位置](https://cdn-ak.f.st-hatena.com/images/fotolife/h/hatenablog/20231107/20231107164142.png)
2. `blogsync.yaml`の各種項目を記述し、変更を`main`ブランチにコミットしてください
    - `<BLOG DOMAIN>`にはブログのブログ取得時に設定したドメインを指定してください(独自ドメインではありません)
    - `<BLOG OWNER HATENA ID>`にはブログのオーナー(ブログ作成者)のはてなIDを指定してください
    - 上記のどちらの項目もブログの「詳細設定 > AtomPub > ルートエンドポイント」から確認できます。ルートエンドポイントは以下のように構成されています
        - `https://blog.hatena.ne.jp/<BLOG OWNER HATENA ID>/<BLOG DOMAIN>/atom`
```yaml
<BLOG DOMAIN>:
  username: <BLOG OWNER HATENA ID>
default:
  local_root: entries
```
3. GitHubリポジトリの設定「`Secrets and variables` > `actions` > `Repository variables`」から以下のVariableを登録する
    - Name: `BLOG_DOMAIN`
    - Value: ブログのドメインを指定してください (例: staff.hatenablog.com)
4. GitHubリポジトリの設定「`Secrets and variables` > `actions` > `Repository Secrets`」から以下のSecretを登録する
    - Name: `OWNER_API_KEY`
    - Secret: ブログのオーナーはてなアカウントのAPIキーを指定してください
        - APIキーは、ブログオーナーアカウントでログイン後、[アカウント設定](https://blog.hatena.ne.jp/-/config)よりご確認いただけます
5. GitHubリポジトリの設定「`Actions` > `General`」の`Workflow permissions`の設定を以下の通り変更する
    - `Read and write permissions`を選択する
    - `Allow GitHub Actions to create and approve pull requests`にチェックを入れる
6. GitHubリポジトリの設定「`Branches`」の`Add branch protection rule`ボタンから、ルールを作成する
    - `Branch name pattern`に`main`を指定する
7. GiHubリポジトリの設定「`General`」の`Pull Requests`項の`Allow auto merge`にチェックを入れる
8. リポジトリにはてなブログの記事を同期させる
    - Actionsタブを開き`initialize` workflowを選択する
    - Run workflowをクリック
    - `Branch: main`が指定されていることを確認し、`Run workflow`ボタンをクリック
    - 「全記事が含まれたプルリクエスト」が作成されます。これをマージしてはてなブログとリポジトリの状況を同期させてください
    - ![Actionsタブ、workflowリスト、Run workflowボタン](https://cdn-ak.f.st-hatena.com/images/fotolife/h/hatenablog/20231107/20231107163433.png)
9. はてなブログの「[設定 > 編集モード](https://blog.hatena.ne.jp/my/config#blog-config-syntax)」設定を「Markdownモード」に設定する

## オプション
- 下書きの作成時のプルリクエストをドラフトプルリクエストとして作成するかどうかのオプション
  - ドラフトプルリクエストは[利用できるプランに制限](https://docs.github.com/ja/pull-requests/collaborating-with-pull-requests/proposing-changes-to-your-work-with-pull-requests/changing-the-stage-of-a-pull-request)があります。対象外のプランを利用している場合、以下のファイルの該当行を`draft: false`に変更してください
  - `/.github/workflows/pull-draft.yaml#L15`
  - `/.github/workflows/create-draft.yaml#L15`

## 想定ワークフロー

このツールで想定している下書き作成から記事公開までのワークフローは以下のとおりです。

1. 下書きを作成する
2. 下書き記事をプルリクエスト上で編集する
3. 適宜レビューなどを行い、通れば次の公開手順に進む
4. 下書き記事を公開する

## 手順の詳細

### 下書きの作成

下書きの作成方法は以下の2通りの方法があります。

- ブログメンバーが個人のアカウントで投稿する(記事の署名は個人のアカウントになります)
- ブログオーナーのアカウントで投稿する(記事の署名はブログオーナーアカウントになります)

それぞれ、下書き作成の手順が異なります。ブログの運営方針や記事の内容に沿った方法を選択してください。

### ブログメンバーが個人のアカウントで投稿する場合

1. 投稿したいブログの編集画面を開く
2. 下書き記事の記事タイトルを`{{username}}-{{日付}}`等、ユニークな記事タイトルに設定し、クリップボードにコピーしておく
3. 下書きを投稿する
4. Actionsから`pull draft from hatenablog`を選択し、`Draft Entry Title`に先程コピーしたタイトルを設定、`Branch: main`に対して実行する
5. 投稿した下書きを含むプルリクエストが作成される

### ブログオーナーのアカウントで投稿する場合

1. Actionsから  `create draft`を選択し、`Title`に記事タイトルを設定、`Branch: main`に対して実行する
2. 作成した下書きを含むプルリクエストが作成される

### 下書き記事の編集

- 手順「下書きの作成」にて作成したプルリクエスト上で記事を編集してください
- はてなブログでプレビューできるようにするため、下書きに限りプッシュされた時点ではてなブログに同期されます
- 記事の編集画面のURLは、プルリクエストに記載されています。編集画面に遷移した後、下書きプレビューのURLを発行し、プルリクエストの概要に記載しておくとプレビューが容易になります

### 下書き記事の公開

- 下書き記事の`Draft: true`行を削除し、プルリクエストをmainブランチにマージすると記事がはてなブログで公開されます
- 記事を公開すると、下書き記事は下書き記事用ディレクトリ`draft_entries`から公開記事用`entries`ディレクトリに移管されます

### 既存記事を修正する場合

- 修正ブランチを作成し、mainブランチにマージすると修正がはてなブログに反映されます

## Boilerplateに新しく追加されたWorkflowを取得する

- workflowの変更は原則[Reusable workflows](https://github.com/hatena/hatenablog-workflows)を変更するため基本的には更新は不要です
- ただし、新しくworkflowが追加されたりした場合は、Boilerplateを元に作成されたリポジトリで新しいファイルを追加したり既存のファイルを更新する必要があります
- 新しいファイルを取得するには`scripts/download_boilerplate_workflows.sh`を実行してください

```bash
bash scripts/download_boilerplate_workflows.sh
```

### Scriptが見つからない場合

- 手元のリポジトリに上記のファイルがない場合、[こちらのファイル](https://github.com/hatena/Hatena-Blog-Workflows-Boilerplate/blob/main/scripts/download_boilerplate_workflows.sh)を自身のリポジトリに追加してください

## トラブルシューティング

### はてなブログ側のデータとリポジトリのデータとで差分が発生した場合

はてなブログのWebの編集画面から記事を更新するなど、はてなブログ側のデータとリポジトリのデータに差異が発生してしまう場合があります。
この場合、 Actionsの`pull from hatenablog`を選択、`Branch: main`に対して実行してください。
実行すると、リポジトリの更新日時以降に更新された公開記事のデータを更新するプルリクエストが作成されます。
これをマージすることで、最新のデータに更新できます。

## workflow に関する詳細

- 各workflowでは下記で提供されているReusable workflowsを利用しています
  - https://github.com/hatena/hatenablog-workflows

<!-- textlint-enable -->
