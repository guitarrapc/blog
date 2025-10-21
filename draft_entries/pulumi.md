PulumiでIgnoreChangesを使う際の注意点


Pulumiで入れ子のキーをIgnoreChangesで無視する方法についてドキュメントがないようなのでメモしておきます。

例えば以下のようなコードがあったとします。

```csharp
new Aws.S3.Bucket("my-bucket", new Aws.S3.BucketArgs
{
    Tags = new InputMap<string>
    {
        { "Environment", "Dev" },
        { "Owner", "Alice" },
    }
});
```

このとき、Tagsのうち"Owner"キーだけをIgnoreChangesで無視したい場合、以下のように`入れ子の親プロパティ[\"入れ子の子プロパティキー\"]`と記述します。

```csharp
new Aws.S3.Bucket("my-bucket", new Aws.S3.BucketArgs
{
    Tags = new InputMap<string>
    {
        { "Environment", "Dev" },
        { "Owner", "Alice" },
    }
}, new CustomResourceOptions
{
    IgnoreChanges = { "tags[\"Owner\"]" }
});
```

これにより、Tagsの"Owner"キーの変更はPulumiの差分検出から無視されるようになります。

## 入れ子のキーがなかったのに後からクラウド側が追加してきてそれを無視したい場合

注意点として、もしリソース作成時に入れ子のキーが存在しなかった場合、PulumiはそのキーをIgnoreChangesで無視できません。
例えば、上記の例で"verified"キーが最初は存在せず、後からクラウド側が追加してきた場合、そのキーをIgnoreChangesで指定しても効果がありません。

```csharp
new Aws.S3.Bucket("my-bucket", new Aws.S3.BucketArgs
{
    Tags = new InputMap<string>
    {
        { "Environment", "Dev" },
        { "Owner", "Alice" },
        // クラウドが後から"verified"キーを追加してきた
    }
}, new CustomResourceOptions
{
    IgnoreChanges = { "tags[\"verified\"]" } // 意味がない
});
```

この場合、Terraformなら存在しなかったキーを定義上で追加すればIgnoreChangesで無視できますが、Pulumiではできません。
リソースにもよりますが、最初から作っておけば無視されるようです。

```csharp
new Aws.S3.Bucket("my-bucket", new Aws.S3.BucketArgs
{
    Tags = new InputMap<string>
    {
        { "Environment", "Dev" },
        { "Owner", "Alice" },
        { "verified", "true" }, // 存在しなかったキーを定義上で追加
    }
}, new CustomResourceOptions
{
    IgnoreChanges = { "tags[\"verified\"]" } // 無視される
});
```

ということで、そういう時は2つ手があります。リソースの再作成は検証環境とかならいいですが、そうじゃないと無理ゲー感がすごいですね。

- 一度消してもいいリソースなら、ignoreChangesを設定してからリソースを再作成する
- Pulumi stackをexportしてから、当該リソースに手動で`verified`キーを追加してからimportし直す力技
