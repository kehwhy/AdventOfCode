using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var map = streamReader.ReadLine();
var maps = new List<string>();
while (map != null)
{
    maps.Add(map);
    map = streamReader.ReadLine();
}

var antennas = new Dictionary<char, List<(int, int)>>();
for (var i = 0; i < maps.Count; i++)
{
    for (var j = 0; j < maps[0].Length; j++)
    {
        if(char.IsNumber(maps[i][j]) || char.IsLetter(maps[i][j]))
        {
            if(antennas.TryGetValue(maps[i][j], out var value))
            {
                value.Add((i, j));
            } else
            {
                antennas.Add(maps[i][j], [(i, j)]);
            }
        }
    }
}

var result = antennas.Aggregate(new List<(int, int)>(), (current2, a) => a.Value.Aggregate(current2, (current1, x) => a.Value.Aggregate(current1, (current, y) => current.Concat(GetAntiNodes(x, y)).ToList())));

var print = result.Distinct()
    .Where(a => a.Item1 >= 0 && a.Item2 >= 0 && a.Item1 < maps.Count && a.Item2 < maps[0].Length);

Console.WriteLine(print.Count());
return;

List<(int, int)> GetAntiNodes((int, int) x, (int, int) y)
{
    var result = new List<(int, int)>();
    var d = (x.Item1 - y.Item1, x.Item2 - y.Item2);

    if (d == (0, 0))
        return [x, y];
    
    var a = (x.Item1+d.Item1, x.Item2+d.Item2);
    while(a.Item1 >= 0 && a.Item1 < maps.Count && a.Item2 >= 0 && a.Item2 < maps[0].Length)
    {
        result.Add(a);
        a = (a.Item1+d.Item1, a.Item2+d.Item2);
    }
    
    var b = (y.Item1-d.Item1, y.Item2-d.Item2);
    while(b.Item1 >= 0 && b.Item1 < maps.Count && b.Item2 >= 0 && b.Item2 < maps[0].Length)
    {
        result.Add(b);
        b = (b.Item1-d.Item1, b.Item2-d.Item2);
    }

    return result;
}