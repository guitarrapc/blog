DexによるCognitoとのOIDC連携

今回は、Dexを使ってAWS CognitoとOIDC連携する方法を紹介します。
OIDC先はEKS上の[Devtron](https://devtron.ai/)ですが、Devを使ったOIDC連携として他でも使えるでしょう。

[:contents]

## Dexとは

[Dex](https://github.com/dexidp/dex)は、他のアプリケーションに対してOpenID Connect (OIDC)やSAML 2.0を提供するオープンソースのアイデンティティプロバイダーです。KubernetesコントローラーのOIDC認証としてよく使われ、Kubernetesクラスター内のアプリケーションに対してシングルサインオン(SSO)を提供しているのを見かけることが多いでしょう。

## Dexの仕組み

とはいえ、実のところDexを使ってOIDC認証を提供していたツールのいくつかは独自OIDC認証を提供するようになってきているように感じます。例えばArgoCDは当初Dexを使ってOIDC認証を提供していましたが、現在はArgoCD自体がOIDCプロバイダーとして動作できるようになっています。個人的には、Dexを使うとアプリケーションへの認証の途中でDex Podとの通信が発生することからグローバルなエンドポイントが必要となることが気になっており、可能であればDexを用いないOIDC認証を使いたいというのが正直な気持ちです。そのあたりは後ほど紹介します。

## DevtronとOIDC認証

DevtronのOIDC認証はDexを使って提供しており、様々なIDプロバイダーと連携できます。今回はその中でAWS Cognitoと連携する方法を紹介します。

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

https://devtron.example.com/orchestrator/api/dex/.well-known/openid-configuration

```json
{
  "issuer": "https://devtron.example.com/orchestrator/api/dex",
  "authorization_endpoint": "https://devtron.example.com/orchestrator/api/dex/auth",
  "token_endpoint": "https://devtron.example.com/orchestrator/api/dex/token",
  "jwks_uri": "https://devtron.example.com/orchestrator/api/dex/keys",
  "userinfo_endpoint": "https://devtron.example.com/orchestrator/api/dex/userinfo",
  "device_authorization_endpoint": "https://devtron.example.com/orchestrator/api/dex/device/code",
  "grant_types_supported": [
    "authorization_code",
    "refresh_token",
    "urn:ietf:params:oauth:grant-type:device_code"
  ],
  "response_types_supported": [
    "code"
  ],
  "subject_types_supported": [
    "public"
  ],
  "id_token_signing_alg_values_supported": [
    "RS256"
  ],
  "code_challenge_methods_supported": [
    "S256",
    "plain"
  ],
  "scopes_supported": [
    "openid",
    "email",
    "groups",
    "profile",
    "offline_access"
  ],
  "token_endpoint_auth_methods_supported": [
    "client_secret_basic",
    "client_secret_post"
  ],
  "claims_supported": [
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


---

[f:id:guitarrapc_tech:20251118000951p:plain:alt=alt text]


[f:id:guitarrapc_tech:20251118000957p:plain:alt=alt text]

[f:id:guitarrapc_tech:20251118001005p:plain:alt=alt text]

---

[f:id:guitarrapc_tech:20251118001015p:plain:alt=alt text]

[f:id:guitarrapc_tech:20251118001023p:plain:alt=alt text]

[f:id:guitarrapc_tech:20251118001033p:plain:alt=alt text]
