HeadlampではじめるKubernetesダッシュボード

Kubernetesのダッシュボードは無数にありますが、Headlampというオープンソースのダッシュボードを触ってみて手触りよかったので紹介します。

EKSで使っているならCognito連携でOIDC認証もできて、Automodeで利用されているKarpneterの状態もプラグインで詳細に追いかけることができます。
また、イベントはPending Podが最優先で表示され、マップビューでクラスター全体のリソース展開状況を見たり、特定のノード、namespaceに絞り込めたり... これこれという感じなのが気に入りました。

私は、[Kubernetes公式ブログ](https://kubernetes.io/blog/2025/10/06/introducing-headlamp-plugin-for-karpenter/)でKarpenterプラグインの紹介記事を読んで興味を持ち、試してみました。

## 誰向けのダッシュボード?

ダッシュボードと一口にいっても、対象ユーザーによって求められる機能は異なります。HeadlampはKubernetesのリソース状態を詳細に追いかけたい開発者や運用担当者向けのダッシュボードです。
特に以下のようなニーズを持つユーザーに適していると感じます。

- Kubernetesのリソースを視覚的に管理したい開発者
- クラスターの状態をリアルタイムで監視したい運用担当者
- Podのイベントやログを迅速に確認したいエンジニア
- Karpenterなどのオートスケーリングツールの状態を詳細に把握したいユーザー

逆に、初心者向けのシンプルなダッシュボードを求めている場合や、特定のアプリケーションの監視に特化したダッシュボードを探している場合には、他の選択肢を検討する方が良いかもしれません。
例えば、ArgoCDはアプリケーション単位でまとめてくれ、リソースの正常状態が確認でき、Deploymentのリスタートもできたりネットワーク的な図も見られるため、アプリケーション管理に特化したい場合にはこっちが適しています。

## Headlampの特徴

Headlampは以下のような特徴を持っています。

- Kubernetes-sigが数年来管理しているオープンソースプロジェクト
- デスクトップアプリケーションとしての利用
- Kubernetesのインクラスターでダッシュボードサーバーとして利用
- プラグインによる拡張性（Karpenterプラグインなど）
- リソースの詳細な表示と管理
- マップビューによるクラスター全体のリソース展開状況の可視化
- OIDC認証のサポート（EKSのCognito連携も可能）

特にさくっとルックアンドフィールで触りたいならデスクトップアプリケーション版がおすすめです。
公式サイトからダウンロードしてインストールするだけで、すぐにKubernetesクラスターに接続して利用できます。

## デスクトップアプリケーション版のHeadlamp

もし自分がKubernetesの閲覧可能なら、デスクトップ版Headlampを使うと何ができるかすぐにためすことができます。

### プラグインを利用する

https://github.com/headlamp-k8s/plugins/tree/main/karpenter

## インクラスター版のHeadlamp

ここではEKSでCognito連携している場合のOIDC認証の設定方法を紹介します。

要点を先にあげておきます。

- Cognitoユーザープールを作成する
- Cognitoアプリクライアントを作成し、リダイレクトURIを設定する
- Cognitoドメインを設定する
- EKS OIDC用のIAMロールにOIDCプロバイダーを信頼するように設定する
- EKSクラスターのOIDCプロバイダーにCognitoユーザープールのURLとIAMロールを紐づけ、Cognitoからemail、groups属性を取得できるようにする
- Headlamp Helmのvalues.yamlでoidc.configにてOIDC設定を入力
- HelmでHeadlampをデプロイする、ClusterRoleBindingでCognitoグループあるいはユーザーのクラスター権限を付ける

EKSのOIDC設定は公式ドキュメントや[Introducing OIDC identity provider authentication for Amazon EKS | AWS Blog](https://aws.amazon.com/jp/blogs/containers/introducing-oidc-identity-provider-authentication-amazon-eks/)が詳しく、大いに参考となります。

### Pulumiで構成する例

以下はPulumiでEKSクラスターとCognito連携のOIDC認証を構成する例です。
結局使うときは認証付きにせざるを得ないのでクラスター認証しておくとかせず、使う想定ができる程度の状態で試しましょう。IaCで構成する例を示すので参考にしてください。

[公式をベース](https://headlamp.dev/docs/latest/installation/in-cluster/eks/)に、足りない箇所を補っています。

#### Cognitoユーザープールとアプリクライアントを作成する

```csharp
```


#### EKSクラスターのOIDCプロバイダー用IAMロールを作成する

```csharp
```

### EKSクラスターにOIDCプロバイダーを紐づける

```csharp
```


### HeadlampをHelmでデプロイする

https://artifacthub.io/packages/helm/headlamp/headlamp

```yaml
```
