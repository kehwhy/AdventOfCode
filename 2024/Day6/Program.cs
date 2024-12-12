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

var start = (0, 0);
for (var i = 0; i < maps.Count; i++)
{
    for (var j = 0; j < maps[i].Length; j++)
    {
        if (maps[i][j] != '.' && maps[i][j] != '#')
        {
            start = (i, j);
            break;
        }
    }
}

var result = GetPath(start.Item1, start.Item2);

var positions = new List<(int, int)> ();
foreach (var (x, y) in result.Distinct())
{
    if (!(x == start.Item1 && y == start.Item2))
    {
        var newMap = new List<string>(maps);
        newMap[x] = string.Concat(newMap[x].AsSpan()[..y], "#", newMap[x].AsSpan(y + 1));
        // foreach (var line in newMap)
        // {
        //     Console.WriteLine(line);
        // }
        if (DoesCreateInfiniteLoop(newMap, start.Item1, start.Item2)) positions.Add((x, y));
        // Console.WriteLine("---------------------");
    }
}

Console.WriteLine(positions.Distinct().Count());

return;

bool DoesCreateInfiniteLoop(List<string> m, int a, int b)
{
    var direction = GetInitialDirection(m[a][b]);
    var x = a;
    var y = b;
    var path = new List<((int, int), (int, int))>();
    path.Add(((x,y), direction));
    while (x < m.Count && x >= 0 && y >= 0 && y < m[0].Length)
    {
        if (x + direction.Item1 >= m.Count || x + direction.Item1 < 0 || y + direction.Item2 < 0 || y + direction.Item2 >= m[0].Length)
        {
            return false;
        }
        if (m[x + direction.Item1][y + direction.Item2] == '#')
        {
            direction = GetNewDirection(direction);
        }
        else
        {
            x += direction.Item1;
            y += direction.Item2;
            if (path.Any(t => t.Item1 == (x,y) && t.Item2 == direction))
            {
                return true;
            }
            path.Add(((x,y), direction));
        }
        
    }

    return false;
}

List<(int, int)> GetPath(int a, int b)
{
    var direction = GetInitialDirection(maps[a][b]);
    var x = a;
    var y = b;
    var path = new List<(int, int)>();
    path.Add((x,y));
    while (x < maps.Count && x >= 0 && y >= 0 && y < maps[0].Length)
    {
        if (x + direction.Item1 >= maps.Count || x + direction.Item1 < 0 || y + direction.Item2 < 0 || y + direction.Item2 >= maps[0].Length)
        {
            return path;
        }
        if (maps[x + direction.Item1][y + direction.Item2] == '#')
        {
            direction = GetNewDirection(direction);
        }
        x += direction.Item1;
        y += direction.Item2;
        path.Add((x,y));
    }

    return path;
}

(int, int) GetInitialDirection(char s)
{
    return s switch
    {
        '^' => (-1, 0),
        'v' => (1, 0),
        '>' => (0, 1),
        '<' => (0, -1),
        _ => (0, 0)
    };
}

(int, int) GetNewDirection((int, int) x)
{
    return x switch
    {
        (1, 0) => (0, -1),
        (0, 1) => (1, 0),
        (-1, 0) => (0, 1),
        (0, -1) => (-1, 0),
        _ => (0, 0)
    };
}