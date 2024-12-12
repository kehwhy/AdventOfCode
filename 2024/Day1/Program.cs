// See https://aka.ms/new-console-template for more information

using System.Text;

const int bufferSize = 128;
List<int> listA = [];
List<int> listB = [];

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize);

var line = streamReader.ReadLine();
while (line != null)
{
    var split = line.Split("   ");
    listA = listA.Append(int.Parse(split[0])).ToList();
    listB = listB.Append(int.Parse(split[1])).ToList();

    line = streamReader.ReadLine();
}

listA.Sort();
listB.Sort();

var total = 0;
var lastBIndex = 0;

foreach (var t in listA)
{
    var similar = 0;
    for (var j = lastBIndex;j < listB.Count;) {
        if (listB[j] < t) {
            j++;
        } else if (listB[j] == t)
        {
            similar++;
            j++;
        }
        else
        {
            lastBIndex = j;
            break;
        }
    }
    total += t * similar;
}

Console.WriteLine(total);