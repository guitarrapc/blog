<Query Kind="Statements" />

// マージ後に記事から以下のセクションを抜く処理
/*
<!--
Date: 2012-12-11T23:12:50+09:00
URL: https://tech.guitarrapc.com/entry/2012/12/11/231250
-->
*/
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

foreach (var targetMonth in targetMonths)
{
	var searchBase = Path.Combine(basePath, targetMonth);
	var files = Directory.EnumerateFiles(searchBase, "*.md", SearchOption.AllDirectories);
	foreach (var file in files)
	{
		// _2.md は除外
		if (Path.GetFileNameWithoutExtension(file).EndsWith("_2"))
			continue;

		var lines = File.ReadAllLines(file);
		var contentBuilder = new StringBuilder();
		var inSkip = false;
		foreach (var line in lines)
		{
			if (line.StartsWith("<!--"))
			{
				inSkip = true;
				continue;
			}
			if (inSkip && line.StartsWith("-->"))
			{
				inSkip = false;
				continue;
			}
			if (inSkip && line.StartsWith("Date: "))
				continue;
			if (inSkip && line.StartsWith("URL: "))
				continue;
				
			contentBuilder.AppendLine(line);
		}
		
		var content = contentBuilder.ToString();
		File.WriteAllText(file, content);
	}
}
