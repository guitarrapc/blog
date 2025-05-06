<Query Kind="Statements" />

// 既存コンテンツをバックアップする処理。
// 既存コンテンツを退避して、マークダウン変換前の作業ファイルと元ファイルをそれぞれ用意す。る
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

// Rename(basePath, targetMonths, "_2");
// Copy(basePath, targetMonths, 2);
// RenameCustomPath(basePath, targetMonths, "_2");
// Remove(basePath, targetMonths, "_2");

static void Rename(string basePath, string[] targetMonths, string suffix)
{
	foreach (var path in targetMonths)
	{
		var files = Directory.EnumerateFiles(Path.Combine(basePath, path), "*.md", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var dst = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + suffix + Path.GetExtension(file));
			dst.Dump(file);
			if (File.Exists(dst))
				continue;
			File.Move(file, dst);
		}
	}
}

static void Copy(string basePath, string[] targetMonths, int removeSuffixLetters)
{
	foreach (var path in targetMonths)
	{
		var files = Directory.EnumerateFiles(Path.Combine(basePath, path), "*.md", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var name = Path.GetFileNameWithoutExtension(file);
			var dst = Path.Combine(Path.GetDirectoryName(file), name.Substring(0, name.Length - removeSuffixLetters)  + Path.GetExtension(file));
			dst.Dump(file);
			if (File.Exists(dst))
				continue;
			File.Copy(file, dst);
		}
	}
}

static void RenameCustomPath(string basePath, string[] targetMonths, string excludeSuffix)
{
	foreach (var path in targetMonths)
	{
		var files = Directory.EnumerateFiles(Path.Combine(basePath, path), "*.md", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var name = Path.GetFileNameWithoutExtension(file);
			if (name.EndsWith(excludeSuffix))
				continue;
			var customPath = file.Substring(basePath.Length, file.Length - basePath.Length - Path.GetExtension(file).Length).Replace("\\", "/");
			file.Dump(customPath);
			var content = File.ReadAllText(file);
			var newContent = content.Replace($"CustomPath: {customPath}_2", $"CustomPath: {customPath}");
			File.WriteAllText(file, newContent);
		}
	}
}

static void Remove(string basePath, string[] targetMonths, string suffix)
{
	foreach (var path in targetMonths)
	{
		var files = Directory.EnumerateFiles(Path.Combine(basePath, path), "*.md", SearchOption.AllDirectories);
		foreach (var file in files)
		{
			var target = Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file) + suffix + Path.GetExtension(file));
			if (!File.Exists(target))
				continue;
			File.Delete(target);
		}
	}
}
