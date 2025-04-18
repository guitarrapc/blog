## レビューに関して

レビューする際には、以下のprefix(接頭辞)を付けましょう。

<!-- for GitHub Copilot review rule -->
[must]→ かならず変更してね
[imo]→ 自分の意見だとこうだけど修正必須ではないよ(in my opinion)
[nits]→ ささいな指摘(nitpick)
[ask]→ 質問
[fyi]→ 参考情報
<!-- for GitHub Copilot review rule-->

<!-- for GitHub Copilot review keypoint -->

レビューは、以下のルールに従ってください。

| パス | レビュー対象 | 概要 |
| --- | --- | --- |
| draft_entries/**.md | レビュー対象です | 下書きのブログ記事です |
| entries/**.md | レビュー対象です | すでに公開されているブログ記事です |

* レビューは日本語で行ってください
* 下書きブログ記事が追加された場合、その記事のタイトルは内容を要約したものにしてください。タイトルはメタセクションの`Title`に記載されています
* 内容に著しい誤りや誤解がある場合、指摘してください
* すでに公開されているブログ記事のPRは、レビュー対象外です
* 文章の改定を提示する場合、textlintのルールに従ってください。利用しているtextlintはpackage.json及び.textlintrc.jsonに記載されています

<!-- for GitHub Copilot review keypoint -->
