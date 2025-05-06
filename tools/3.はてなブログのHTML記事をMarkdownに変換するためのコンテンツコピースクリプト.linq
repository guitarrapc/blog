<Query Kind="Statements" />

// 既存コンテンツをベースにDraftにコンテンツを持ってくる処理。
// 既存コンテンツがHTMLなのをマークダウンに変換する機能をはてなブログはもっていないので、同一URLで記事を新規で作り直すために行う。
// Steps to use:
// 1. 既存の記事にCustomPath: yyyy/MM/dd を設定
// 2. create-draft-bulk ワークフローでyyyy/MM/ddの下書きをまとめて作成
// 3. ドラフトのブランチを1ブランチにまとめて、複数日まとめて処理する
// 4. このスクリプトを実行して、既存コンテンツをベースに下書きを更新
var draftBasePath = @"D:\github\guitarrapc\blog\draft_entries\";
var basePath = @"D:\github\guitarrapc\blog\entries\guitarrapc-tech.hatenablog.com\entry\";
var targetMonths = new[] {
	"2012/11",
	"2012/12",
	"2013/01",
	"2013/02",
	"2013/03",
	"2013/04",
	"2013/05",
	"2013/06",
	"2013/07",
	"2013/08",
	"2013/09/02",
	"2013/09/03",
	"2013/09/06",
	"2013/09/07",
	"2013/09/08",
	"2013/09/10",
};
var titlePattern = new Regex("201[2,3]/[0-9]{2}/[0-9]{2}/[0-9]{6}", RegexOptions.Compiled);

var drafts = Directory.EnumerateFiles(draftBasePath, "*.md");
foreach (var draft in drafts)
{
	var title = File.ReadLines(draft)
		.Where(x => x.StartsWith("Title:"))
		.Select(x => x.Split(":")[1].Trim())
		.Single();

	if (!titlePattern.IsMatch(title))
		continue;

	var draftContent = File.ReadAllLines(draft);
	var draftEditUrlLine = draftContent.Where(x => x.StartsWith("EditURL: ")).Single();
	var draftPreviewURL = draftContent.Where(x => x.StartsWith("PreviewURL: ")).Single();

	foreach (var targetMonth in targetMonths)
	{
		var searchBase = Path.Combine(basePath, targetMonth);
		var files = Directory.EnumerateFiles(searchBase, "*.md", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			// 既存コンテンツか判定
			var lines = File.ReadAllLines(file);
			var isTargetFile = lines.Any(x => x.Contains($"CustomPath: {title}"));
			if (!isTargetFile)
				continue;

			// _2.md は除外
			if (Path.GetFileNameWithoutExtension(file).EndsWith("_2"))
			{
				File.Delete(file);
				continue;
			}

			// 既存コンテンツをベースにdraftにコンテンツを持ってくる
			var sectionLines = GetHeaderSectionLines(lines);
			var titleLine = sectionLines.Where(x => x.StartsWith("Title: ")).Single();
			var categoryLine = GetCategories(sectionLines);
			var dateLine = sectionLines.Where(x => x.StartsWith("Date: ")).Single();
			var urlLine = sectionLines.Where(x => x.StartsWith("URL: ")).Single();
			var customPathLine = sectionLines.Where(x => x.StartsWith("CustomPath: ")).Single();

			var contentBuilder = new StringBuilder();
			// ヘッダー
			contentBuilder.AppendLine("---");
			contentBuilder.AppendLine(titleLine);
			contentBuilder.AppendLine(categoryLine);
			contentBuilder.AppendLine(dateLine);
			contentBuilder.AppendLine(draftEditUrlLine);
			contentBuilder.AppendLine(draftPreviewURL);
			contentBuilder.AppendLine(customPathLine);
			contentBuilder.AppendLine("---");

			// コンテンツ
			var contentsLines = lines.Skip(sectionLines.Length);
			contentBuilder.AppendLine();
			contentBuilder.AppendLine("<!--");
			contentBuilder.AppendLine($"{dateLine}");
			contentBuilder.AppendLine($"{urlLine}");
			contentBuilder.AppendLine("-->");
			foreach (var line in contentsLines)
			{
				contentBuilder.AppendLine(line);
			}
			var content = contentBuilder.ToString();

			// 書き込み
			if (content != "")
			{
				File.WriteAllText(draft, content);
			}

			// 既存コンテンツを削除する
			File.Delete(file);
		}
	}
}

static string[] GetHeaderSectionLines(string[] lines)
{
	int number = 0;
	var inSection = false;
	foreach (var line in lines)
	{
		// enter
		if (!inSection && line == "---")
		{
			inSection = true;
			number++;
			continue;
		}
		// exit
		if (inSection && line == "---")
		{
			inSection = false;
			number++;
			break;
		}
		// inside
		if (inSection && line != "---")
		{
			number++;
			continue;
		}
	}

	return lines.Take(number).ToArray();
}

static string GetCategories(string[] lines)
{
	var categoryLines = new StringBuilder();
	var inCategory = false;
	foreach (var line in lines)
	{
		if (line.StartsWith("Category:"))
		{
			inCategory = true;
			categoryLines.AppendLine(line);
			continue;
		}
		if (inCategory)
		{
			if (!line.StartsWith("- "))
			{
				inCategory = false;
				break;
			}

			categoryLines.AppendLine(line);
			continue;
		}
	}

	// 最終行の空行はトリム除去
	return categoryLines.ToString().TrimEnd();
}
