Devtronを使ったKubernetesの新しい運用方法

Kubernetesはいわゆるコンテナオーケストレーターですが、そのまま素で使っている人はどれだけいるでしょうか。
かくいう私もHelmチャートを使ってKubernetesクラスターに様々なコントローラーを導入したり、ArgoCDを使ってGitOps運用したり、HeadlampやDatadogでモニタリングしたりと、Kubernetesの上にさらに様々なツールを載せて運用しています。

そう、Kubernetesを運用するために多くのツールを組み合わせて、またクラウドとも連携してようやく本格的な運用ができる、運用の難しさこそがKubernetesの難しさたる一面であることは疑いようがないでしょう。普段運用していると気づきにくいですが、ふと振り返ったり、誰かに説明するときに割と本当に難しいと感じます。
そもそもコンテナの知識、それを運用するオーケストレーターの知識、クラウドと連携する方法、DNSやクラウドリースやセキュリティ、監視などなど、Kubernetesを運用するには多くの知識が必要です。
この状況はKubernetesが登場して以来、複雑になる一方で、初心者がKubernetesを使い始めるのは非常にハードル高い状態が続いています。

そんなKubernetesの運用に必要な機能をオールインワンで提供するプラットフォームがいくつか登場しています。今回紹介するDevtronもその1つです。

[:contents]

## Devtronの目指すところ

> Devtron is purpose-built for production Kubernetes teams, unifying app and infrastructure management with an AI teammate that simplifies operations and speeds delivery.

これはDevtronの謳い文句ですが、DevtronはKubernetesのアプリケーションとインフラ管理を統合しAIアシスタントが運用を簡素化し、デリバリーを加速することを目的としています。

実際のところ、触ってみるとこれはなかなか難しいことをやっているなと感じます。DevtronはKubernetesの上に様々な機能を提供しており、アプリケーションのデプロイから監視、セキュリティまで幅広くカバーしています。具体的には、複数のOSSを組み合わせて、またDevtronから展開することで達成しようとしています。

代表的な機能は以下の通りです。

- CI/CD統合 = ArgoCDやFluxをDevtronが制御して、デプロイ履歴やデプロイ問題をAIが検知、自動ロールバック
- セキュリティ統合 = イメージやマニフェストの素スキャン、シークレットがコードやコンフィグに露出していないかチェック、シップ前にライセンススキャン
- Observability = PrometheusやGrafanaをDevtron UIに組み込んで監視を一か所から提供
- コスト管理 = コストを環境単位でモニタリング、AIによるサイジングや異常検出

DevtronはこれらをDevtronから提供、UIも一元化することで、Kubernetesの運用を大幅に簡素化しようとしています。

##



[f:id:guitarrapc_tech:20251118000845p:plain:alt=alt text]<!--image.png-->

[f:id:guitarrapc_tech:20251118000853p:plain:alt=alt text]<!--image-1.png-->

URL: https://devtron.example.com/orchestrator


```yaml
name: cognito
id: cognito
config:
 claimMapping:
  groups: cognito:groups
 clientID: ••••••••
 clientSecret: ••••••••
 issuer: https://cognito-idp.REGION.amazonaws.com/USER-POOL-ID
 redirectURI: https://devtron.example.com/orchestrator/api/dex/callback
 scopes:
  - openid
  - email
  - profile
 userNameKey: cognito:username
```

https://devtron.example/orchestrator/api/dex/.well-known/openid-configuration

```json
{
  "issuer": "https://devtron.example.com/orchestrator/api/dex",
  "authorization_endpoint": "https://devtron.example.com/orchestrator/api/dex/auth",
  "token_endpoint": "https://devtron.example.com/orchestrator/api/dex/token",
  "jwks_uri": "https://devtron.example.com/orchestrator/api/dex/keys",
  "userinfo_endpoint": "https://devtron.example.com/orchestrator/api/dex/userinfo",
  "device_authorization_endpoint": "https://devtron.example.com/orchestrator/api/dex/device/code",
  "grant_types_supported":[
    "authorization_code",
    "refresh_token",
    "urn:ietf:params:OAuth:grant-type:device_code"
],
  "response_types_supported":[
    "code"
],
  "subject_types_supported":[
    "public"
],
  "id_token_signing_alg_values_supported":[
    "RS256"
],
  "code_challenge_methods_supported":[
    "S256",
    "plain"
],
  "scopes_supported":[
    "openid",
    "email",
    "groups",
    "profile",
    "offline_access"
],
  "token_endpoint_auth_methods_supported":[
    "client_secret_basic",
    "client_secret_post"
],
  "claims_supported":[
    "iss",
    "sub",
    "aud",
    "iat",
    "exp",
    "email",
    "email_verified",
    "locale",
    "name",
    "preferred_username",
    "at_hash"
]
}
```


[f:id:guitarrapc_tech:20251118000902p:plain:alt=alt text]<!--image-7.png-->


[f:id:guitarrapc_tech:20251118000908p:plain:alt=alt text]<!--image-2.png-->

[f:id:guitarrapc_tech:20251118000915p:plain:alt=alt text]<!--image-3.png-->


[f:id:guitarrapc_tech:20251118000925p:plain:alt=alt text]<!--image-4.png-->

[f:id:guitarrapc_tech:20251118000934p:plain:alt=alt text]<!--image-5.png-->

[f:id:guitarrapc_tech:20251118000942p:plain:alt=alt text]<!--image-6.png-->


cicd

```
$ kubectl get po -A
NAME                                       READY   STATUS      RESTARTS        AGE
app-sync-job-vykie-hj7xv                   1/1     Running     0               4m56s
argocd-dex-server-56b6556c7f-r9xw2         1/1     Running     0               4m57s
dashboard-784b5bbcfb-b8k7d                 1/1     Running     0               4m57s
devtron-584d7b6797-nn6kd                   1/1     Running     2 (118s ago)    2m
devtron-nats-0                             3/3     Running     0               4m56s
devtron-nats-test-request-reply            0/1     Completed   0               4m57s
Git-sensor-0                               1/1     Running     4 (2m29s ago)   4m56s
inception-7b69fb787c-2qj8z                 1/1     Running     0               4m57s
kubelink-f8d5bf985-579gj                   1/1     Running     2 (118s ago)    2m
kubewatch-5c6fd4d4b5-8rlt4                 1/1     Running     4 (2m2s ago)    4m57s
lens-55cdc47f9b-7lmqz                      1/1     Running     4 (2m41s ago)   4m57s
postgresql-migrate-casbin-0b0tv-wldnl      0/1     Completed   0               4m56s
postgresql-migrate-devtron-qqdbt-28hh4     0/1     Completed   2               4m56s
postgresql-migrate-gitsensor-g52lv-pk6gs   0/1     Completed   3               4m56s
postgresql-migrate-lens-rmqgj-skppn        0/1     Completed   3               4m56s
postgresql-postgresql-0                    2/2     Running     0               4m56s
```

add argocd

```
$ kubectl get po -A
NAMESPACE   NAME                                       READY   STATUS      RESTARTS      AGE
argo        workflow-controller-7fbdc9c877-4l8h4       1/1     Running     0             17m
devtroncd   argocd-application-controller-0            1/1     Running     0             9m10s
devtroncd   argocd-dex-server-56b6556c7f-r9xw2         1/1     Running     0             17m
devtroncd   argocd-Redis-86b6b9888b-5lmwg              1/1     Running     0             9m10s
devtroncd   argocd-repo-server-78fcb8745b-9mwvk        1/1     Running     0             9m10s
devtroncd   argocd-server-8474776d6-ssrlq              1/1     Running     0             9m10s
devtroncd   dashboard-784b5bbcfb-b8k7d                 1/1     Running     0             17m
devtroncd   devtron-9f55ffd8f-4nkc9                    1/1     Running     0             9m4s
devtroncd   devtron-nats-0                             3/3     Running     0             17m
devtroncd   devtron-nats-test-request-reply            0/1     Completed   0             17m
devtroncd   Git-sensor-0                               1/1     Running     4 (14m ago)   17m
devtroncd   inception-7b69fb787c-2qj8z                 1/1     Running     0             17m
devtroncd   kubelink-545b8f465c-hw46g                  1/1     Running     0             9m4s
devtroncd   kubewatch-5c6fd4d4b5-8rlt4                 1/1     Running     4 (14m ago)   17m
devtroncd   lens-55cdc47f9b-7lmqz                      1/1     Running     4 (14m ago)   17m
devtroncd   postgresql-migrate-casbin-3ef7n-d56rd      0/1     Completed   0             9m10s
devtroncd   postgresql-migrate-devtron-wcxwp-dhpsb     0/1     Completed   0             9m10s
devtroncd   postgresql-migrate-gitsensor-wgxiq-8cxsf   0/1     Completed   0             9m10s
devtroncd   postgresql-migrate-lens-cdgiz-vk452        0/1     Completed   0             9m10s
devtroncd   postgresql-postgresql-0                    2/2     Running     0             17m
```

add trivy

```
$ k get po -A
NAMESPACE   NAME                                       READY   STATUS      RESTARTS      AGE
argo        workflow-controller-7fbdc9c877-4l8h4       1/1     Running     0             39m
devtroncd   argocd-application-controller-0            1/1     Running     0             31m
devtroncd   argocd-dex-server-56b6556c7f-r9xw2         1/1     Running     0             39m
devtroncd   argocd-Redis-86b6b9888b-5lmwg              1/1     Running     0             31m
devtroncd   argocd-repo-server-78fcb8745b-9mwvk        1/1     Running     0             31m
devtroncd   argocd-server-8474776d6-ssrlq              1/1     Running     0             31m
devtroncd   dashboard-784b5bbcfb-b8k7d                 1/1     Running     0             39m
devtroncd   devtron-7688fbc8d8-ck7qx                   1/1     Running     0             15m
devtroncd   devtron-nats-0                             3/3     Running     0             39m
devtroncd   devtron-nats-test-request-reply            0/1     Completed   0             39m
devtroncd   Git-sensor-0                               1/1     Running     4 (37m ago)   39m
devtroncd   image-scanner-6b5b5b976c-qgfvx             1/1     Running     0             15m
devtroncd   inception-7b69fb787c-2qj8z                 1/1     Running     0             39m
devtroncd   kubelink-668c7ccb7d-qhm6j                  1/1     Running     0             15m
devtroncd   kubewatch-5c6fd4d4b5-8rlt4                 1/1     Running     4 (36m ago)   39m
devtroncd   lens-55cdc47f9b-7lmqz                      1/1     Running     4 (37m ago)   39m
devtroncd   postgresql-migrate-casbin-sspac-zk9×8      0/1     Completed   0             15m
devtroncd   postgresql-migrate-devtron-idjwh-grfwr     0/1     Completed   0             15m
devtroncd   postgresql-migrate-gitsensor-glbv3-8qflx   0/1     Completed   0             15m
devtroncd   postgresql-migrate-lens-jwyyd-b4hzg        0/1     Completed   0             15m
devtroncd   postgresql-postgresql-0                    2/2     Running     0             39m
```

![alt text](image-9.png)<!--image-9.png-->

![alt text](image-8.png)<!--image-8.png-->
