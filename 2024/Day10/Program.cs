using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var map = streamReader.ReadLine();
var maps = new List<char[]>();
while (map != null)
{
    maps.Add(map.ToCharArray());
    map = streamReader.ReadLine();
}

var result = 0;
for (var i = 0; i < maps.Count; i++)
{
    for (var j = 0; j < maps[0].Length; j++)
    {
        if (maps[i][j] == '0')
        {
            var trailheadTotal = CheckForTrails((i, j));
            // Console.WriteLine(trailheadTotal);
            result += trailheadTotal;
        }
    }
}

Console.WriteLine(result);
return;


int CheckForTrails((int, int) trailhead)
{
    var workingList = new List<(int, int, int)>();
    workingList.Add((trailhead.Item1, trailhead.Item2, 0));
    var foundTops = new List<(int, int)> ();

    while (workingList.Count > 0)
    {
        var current = workingList[0];
        if (current.Item1 < 0 || current.Item1 >= maps.Count || current.Item2 < 0 || current.Item2 >= maps[0].Length)
        {
            workingList.Remove(current);
            continue;
        }
        var cell = int.Parse($"{maps[current.Item1][current.Item2]}");
        if (cell == current.Item3  && current.Item3 == 9)
        {
            foundTops.Add((current.Item1, current.Item2));
        }
        else if (cell == current.Item3)
        {
            workingList.Add((current.Item1 + 1, current.Item2, current.Item3 + 1));
            workingList.Add((current.Item1 - 1, current.Item2, current.Item3 + 1));
            workingList.Add((current.Item1, current.Item2 + 1, current.Item3 + 1));
            workingList.Add((current.Item1, current.Item2 - 1, current.Item3 + 1));
        }
        workingList.Remove(current);
    }

    return foundTops.Count;
}