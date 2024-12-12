using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var line = streamReader.ReadLine();
var lineLength = line!.Length;
var wordSearch = new List<string>();
while (line != null)
{
    wordSearch.Add(line);
    line = streamReader.ReadLine();
}

var total = 0;
for (var i = 0; i < wordSearch.Count; i++)
{
    for (var j = 0; j < lineLength; j++)
    {
        if (wordSearch[i][j] == 'A')
        {
            total += SearchForMasX(i, j);
        }
    }
}

Console.WriteLine(total);

return;

int SearchForMasX(int i, int j)
{
    if (i-1 < 0 || i+1 >= wordSearch.Count || j-1 < 0 || j+1 >= lineLength)
    {
        return 0;
    }

    return SearchDiagonal(i, j, 1, 1) && SearchDiagonal(i, j, -1, 1) ? 1 : 0;
}

bool SearchDiagonal(int i, int j, int x, int y)
{
    return (wordSearch[i-x][j-y]=='M' && wordSearch[i+x][j+y]=='S') || (wordSearch[i-x][j-y]=='S' && wordSearch[i+x][j+y]=='M');
}

int SearchForMas(int i, int j)
{
    if (i < 0 || i >= wordSearch.Count || j < 0 || j >= lineLength || wordSearch[i][j] != 'X')
    {
        return 0;
    }

    return SearchDirection('M', i, j, -1, -1) 
           + SearchDirection('M', i, j, -1, 0)
           + SearchDirection('M', i, j, -1, 1)
           + SearchDirection('M', i, j, 0, -1)
           + SearchDirection('M', i, j, 0, 1)
           + SearchDirection('M', i, j, 1, -1)
           + SearchDirection('M', i, j, 1, 0)
           + SearchDirection('M', i, j, 1, 1);
}

int SearchDirection(char l, int i, int j, int x, int y)
{
    if (l == '.')
    {
        return 1;
    }
    if (i+x < 0 || i+x >= wordSearch.Count || j+y < 0 || j+y >= lineLength)
    {
        return 0;
    }
    return wordSearch[i + x][j + y] == l ? SearchDirection(GetNextChar(l), i + x, j + y, x, y) : 0;
}

char GetNextChar(char l)
{
    return l switch
    {
        'M' => 'A',
        'A' => 'S',
        _ => '.'
    };
}