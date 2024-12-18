using System.Text;
using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var memory = streamReader.ReadToEnd();
var memoryList = new List<int>();
var files = new List<FileItem>();
var emptySpaces = new List<EmptySpace>();

for(var i=0;i<memory.Length;i++)
{
    if (i % 2 == 0)
    {
        files.Add(new FileItem { StartingIndex = memoryList.Count, Size = int.Parse($"{memory[i]}"), FileId = i/2});
        for(var j=0; j<int.Parse($"{memory[i]}"); j++)
        {
            memoryList.Add(i/2);
        }
    }
    else
    {
        emptySpaces.Add(new EmptySpace{StartingIndex = memoryList.Count, Size = int.Parse($"{memory[i]}") });
        for(var j=0; j<int.Parse($"{memory[i]}"); j++)
        {
            memoryList.Add(-1);
        }
    }
}

// PART 1

// var result = 0;
// var back = memoryList.Count - 1;
// var front = 0;
// while(front <= back || memoryList[front] == -1)
// {
//     switch (memoryList[front])
//     {
//         case -1 when memoryList[back] == -1:
//             back--;
//             break;
//         case -1:
//             result += int.Parse($"{memoryList[back]}") * front;
//             front++;
//             back--;
//             break;
//         default:
//             result += int.Parse($"{memoryList[front]}") * front;
//             front++;
//             break;
//     }
// }
//
// Console.WriteLine(result);

// PART 2

while (files.Count > 0)
{
    var file = files.Last();
    files.RemoveAt(files.Count - 1);
    
    var emptySpace = emptySpaces.FirstOrDefault(x => x.Size >= file.Size, new EmptySpace{Size = 0});
    if (emptySpace.Size != 0 && emptySpace.StartingIndex < file.StartingIndex)
    {
        for(var i=emptySpace.StartingIndex; i<emptySpace.StartingIndex+file.Size; i++)
        {
            memoryList[i] = file.FileId;
        }
        for(var i=file.StartingIndex; i<file.StartingIndex+file.Size; i++)
        {
            memoryList[i] = -1;
        }
        emptySpaces[emptySpaces.IndexOf(emptySpace)] = new EmptySpace{StartingIndex = emptySpace.StartingIndex + file.Size, Size = emptySpace.Size - file.Size};
    }
}

long result = 0;

// Calculate the result
for (var i = 0; i < memoryList.Count; i++)
{
    if (memoryList[i] != -1)
    {
        result += i * memoryList[i];
    }
}

// Print the result
Console.WriteLine("Result: " + result);

// Print the modified memory list
Console.WriteLine("Modified memory list: " + string.Join(", ", memoryList));

internal struct FileItem
{
    public int StartingIndex { get; set; }
    public int Size { get; set; }
    public int FileId { get; set; }
}

internal struct EmptySpace
{
    public int StartingIndex { get; set; }
    public int Size { get; set; }
}