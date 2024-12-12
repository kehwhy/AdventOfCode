using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var memory = streamReader.ReadToEnd();
var memoryList = new List<string>();

for(var i=0;i<memory.Length;i++)
{
    if (i % 2 == 0)
    {
        for(var j=0; j<int.Parse($"{memory[i]}"); j++)
        {
            memoryList.Add($"{i/2}");
        }
    }
    else
    {
        for(var j=0; j<int.Parse($"{memory[i]}"); j++)
        {
            memoryList.Add("-1");
        }
    }
}

var result = 0;
var back = memoryList.Count - 1;
var front = 0;
while(front <= back || memoryList[front] == "-1")
{
    switch (memoryList[front])
    {
        case "-1" when memoryList[back] == "-1":
            back--;
            break;
        case "-1":
            result += int.Parse($"{memoryList[back]}") * front;
            front++;
            back--;
            break;
        default:
            result += int.Parse($"{memoryList[front]}") * front;
            front++;
            break;
    }
}

Console.WriteLine(result);