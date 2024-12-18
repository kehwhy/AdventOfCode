using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var stoneString = streamReader.ReadLine();
var stones = stoneString?.Split(' ').Select(long.Parse).ToList();

var blinkCount = 0;
var stoneDictionary = stones!.ToDictionary<long, long, long>(stone => stone, stone => 1);
while (blinkCount < 75)
{
    var newStoneDictionary = new Dictionary<long, long>();
    foreach (var (item, count) in stoneDictionary)
    {
        var newStones = Blink(item);
        foreach (var stone in newStones)
        {
            if (newStoneDictionary.ContainsKey(stone))
            {
                newStoneDictionary[stone] += count;
            }
            else
            {
                newStoneDictionary.Add(stone, count);
            }
        }
    }
    stoneDictionary = newStoneDictionary;
    blinkCount++;
}

Console.WriteLine(stoneDictionary.Sum(x => x.Value));

return;

long[] Blink(long n)
{
    switch (n){
    case 0:
        return [1];
    case >0 when NumberOfDigits(n) % 2 == 0:
        var subStoneString = $"{n}";
        var newLeft = long.Parse(subStoneString.Substring(0, subStoneString.Length/2));
        var newRight = long.Parse(subStoneString.Substring(subStoneString.Length/2));
        return [newLeft, newRight];
    default:
        return [2024 * n];
    }

    long NumberOfDigits(long i)
    {
        var s = $"{i}";
        return s.Length;
    }
}