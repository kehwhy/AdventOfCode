using System.Text;
using System.Text.RegularExpressions;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var memory = streamReader.ReadToEnd();

var pattern = new Regex(@"mul\((?<number1>\d+),(?<number2>\d+)\)");
var doPattern = new Regex(@"do(?!n't)");
var doNotPattern = new Regex(@"don't");

var doIndexes = new List<int>();
doIndexes.Add(0);

var doNotIndexes = new List<int>();

for (var i = 0; i < memory.Length-5; i++)
{
    if (doPattern.IsMatch(memory.Substring(i, 5)))
    {
        if (doIndexes.Count == doNotIndexes.Count)
            doIndexes.Add(i);
    }
    if (doNotPattern.IsMatch(memory.Substring(i, 5)))
    {
        if (doIndexes.Count > doNotIndexes.Count)
            doNotIndexes.Add(i);
    }
}

var result = 0;
for (var i = 0; i < doNotIndexes.Count; i++)
{
    result += ExtractAndCount(memory.Substring(doIndexes[i], doNotIndexes[i]-doIndexes[i]), pattern);
}
if (doIndexes.Count > doNotIndexes.Count)
    result += ExtractAndCount(memory.Substring(doIndexes[^1]), pattern);

Console.WriteLine(result);

return;

int ExtractAndCount(string s, Regex p)
{
    var collection = p.Matches(s);

    var i = 0;
    foreach (Match match in collection)
    {
        var number1 = int.Parse(match.Groups["number1"].Value);
        var number2 = int.Parse(match.Groups["number2"].Value);
        i += number1 * number2;
    }

    return i;
}

