using System.Text;

using var fileStream = File.OpenRead("input.txt");
using var streamReader = new StreamReader(fileStream, Encoding.UTF8, true);

var rule = streamReader.ReadLine();
var ruleDict = new Dictionary<string, bool>();
while (rule.Trim().Length != 0)
{
    ruleDict.Add(rule, true);
    rule = streamReader.ReadLine();
}

var order = streamReader.ReadLine();
var total = 0;
while (order != null)
{
    Console.WriteLine(order);
    if (!ValidateOrder(order))
    {
        Console.WriteLine("NO");
        var newOrder = FixOrder(order);
        total += newOrder[(int)Math.Floor(newOrder.Count/2.0)];
    }
    order = streamReader.ReadLine();
}

Console.WriteLine(total);

return;

bool ValidateOrder(string o)
{
    var orderParts = o.Split(',');
    for (var i = 0; i < orderParts.Length-1; i++)
    {
        var a = int.Parse(orderParts[i]);
        var b = int.Parse(orderParts[i+1]);
        var s = $"{a}|{b}";
        if (!ruleDict.ContainsKey(s))
        {
            return false;
        }
    }

    return true;
}

List<int> FixOrder(string o)
{
    var orderParts = o.Split(',').Select(int.Parse).ToList();
    orderParts.Sort( new Comparison<int>((s1, s2)=> {
        if (ruleDict.ContainsKey($"{s1}|{s2}"))
        {
            return -1;
        }
        return 1;
    }));

    return orderParts.ToList();
}
    