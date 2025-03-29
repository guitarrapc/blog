<Query Kind="Statements" />

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

foreach (var path in targetMonths)
{
	var files = Directory.EnumerateFiles(Path.Combine(basePath, path), "*.md", SearchOption.AllDirectories);
	foreach (var file in files.Where(x => !Path.GetFileNameWithoutExtension(x).EndsWith("_2")))
	{
		var name = file.Replace(basePath, "").Replace("\\", "/").Replace(".md", "");
		Console.WriteLine($"- {name}");
	}
}