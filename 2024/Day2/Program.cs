using System.Text;

List<int> IsIncreasingDecreasing(List<int> list)
{
    var increasingLevels = new List<int>();;
    var decreasingLevels = new List<int>();;
    for (var i = 1; i < list.Count; i++)
    {
        if (list[i] > list[i - 1])
        {
            decreasingLevels.Add(i-1);
            decreasingLevels.Add(i);
        }
        else if (list[i] < list[i - 1])
        {
            increasingLevels.Add(i-1);
            increasingLevels.Add(i);
        }
    }

    return increasingLevels.Count < decreasingLevels.Count ? increasingLevels : decreasingLevels;
}

List<int> IsGradual(List<int> list)
{
    var gradual = new List<int>();
    int[] safeValues = [1, 2, 3];
    for (var i = 1; i < list.Count; i++)
    {
        if (!safeValues.Contains(Math.Abs(list[i] - list[i - 1])))
        {
            gradual.Add(i - 1);
            gradual.Add(i);
        }
    }

    return gradual;
}

const int bufferSize = 128;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize);

var safeLineCount = 0;
var line = streamReader.ReadLine();
while (line != null)
{
    var split = line.Split(" ").Select(int.Parse).ToList();
    var listA = IsIncreasingDecreasing(split);
    var listB = IsGradual(split);
    switch (listA.Count + listB.Count)
    {
        case 0:
            safeLineCount++;
            break;
        default:
        {
            var removeIndexs = listA.Concat(listB).Distinct().ToList();
            foreach (var removeIndex in removeIndexs)
            {
                int[] newSplit = new int[split.Count];
                split.CopyTo(newSplit);
                var testSplit = newSplit.ToList();
                testSplit.RemoveAt(removeIndex);
                if (IsIncreasingDecreasing(testSplit).Count + IsGradual(testSplit).Count == 0)
                {
                    safeLineCount++;
                    break;
                }
            }
            break;
        }
    }

    line = streamReader.ReadLine();
}

Console.WriteLine(safeLineCount);

