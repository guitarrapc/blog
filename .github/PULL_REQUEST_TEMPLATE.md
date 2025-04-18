## レビューに関して

レビューする際には、以下のprefix(接頭辞)を付けましょう。

<!-- for GitHub Copilot review rule -->
[must]→ かならず変更してね
[imo]→ 自分の意見だとこうだけど修正必須ではないよ(in my opinion)
[nits]→ ささいな指摘(nitpick)
[ask]→ 質問
[fyi]→ 参考情報
<!-- for GitHub Copilot review rule-->

<!-- for GitHub Copilot review structure -->

ブログ記事は次の構造です。

| パス | レビュー対象 | 概要 |
| --- | --- | --- |
| draft_entries/**.md | レビュー対象です | 下書きのブログ記事です |
| entries/**.md | レビュー対象です | すでに公開されているブログ記事です |

<!-- for GitHub Copilot review structure -->

<!-- for GitHub Copilot review keypoint -->

レビューは以下のルールに従ってください。

- レビューは日本語で行ってください
- 記事タイトルはメタセクションの`Title`を参照してください
- 下書きブログ記事は記事のタイトルは内容を要約したものかチェックしてください
- 公開されているブログ記事のタイトルは変更しないでください
- 内容に著しい誤りや誤解が見受けられる場合は指摘してください

文章のLintは以下のルールに従ってください。

- 文章の改定を提示する場合、textlintのルールに従ってください
- 利用しているtextlintはpackage.json及び.textlintrc.jsonを参照してください
- 例えば英語や製品名と「日本語の助詞」の間にスペースは含めません

<!-- for GitHub Copilot review keypoint -->
