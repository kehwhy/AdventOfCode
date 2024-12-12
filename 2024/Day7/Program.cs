using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var line = streamReader.ReadLine();
var equations = new List<(long, List<long>)>();
while (line != null)
{
    var parts = line.Split(':');
    var numbers = parts[1].Trim().Split(' ').Select(long.Parse).ToList();
    equations.Add((long.Parse(parts[0]), numbers));
    line = streamReader.ReadLine();
}

long total = 0;
foreach(var (result, numbers) in equations)
{
    var combinations = GetCombinations(numbers);

    if (combinations.Contains(result))
    {
        total += result;
    }
}

Console.WriteLine(total);

return;

List<long> GetCombinations(List<long> longs)
{
    var list = new List<long>();
    list.Add(longs[0]);
    foreach (var t in longs.Slice(1, longs.Count-1))
    {
        foreach (var combo in list.ToList())
        {
            list.Remove(combo);
            list.Add(GetSquish(combo, t));    
            list.Add(combo + t);
            list.Add(combo * t);
            
        }
    }
    return list;
}

long GetSquish(long a, long b)
{
    return long.Parse($"{a}{b}");
}