open System 

type Card = 
    {
        index: int
        wins: int list
        myNumbers: int list
    }

let parseCard (str: string) =
    let split = str.Split ':'
    let index = 
        split[0].Split ' '
    let split2 = split[1].Split '|'
    {
        index = index[index.Length-1].Trim() |> Int32.Parse
        wins = (split2[0].Split ' ') |> Array.filter(fun x -> x.Trim().Length > 0) |> Array.map (Int32.Parse) |> Array.toList
        myNumbers = (split2[1].Split ' ') |> Array.filter(fun x -> x.Trim().Length > 0) |> Array.map (Int32.Parse) |> Array.toList
    }

let mutable cardMap = Map.empty

let scoreCard card maxCard multiplier = 
    let mutable count = 0
    card.wins
    |> List.iter(fun win -> 
        if card.myNumbers |> List.contains win then
            count <- count + 1
    )
    let last = 
        if card.index+count < maxCard then 
            card.index+count 
        else 
            maxCard

    for c in card.index+1..last do
        let total = cardMap[c]
        cardMap <- cardMap.Add (c, (total+multiplier))


let input = System.IO.File.ReadLines("./input.txt")
let cards = input |> Seq.map(fun x -> parseCard x) |> Seq.toArray
let mutable cardTotal = 0

cards |> Array.iter(fun x -> cardMap <- cardMap.Add(x.index, 1))

let mutable i = 0
while i < cards.Length do 
    cardTotal <- cardTotal + cardMap.[i+1]
    scoreCard cards[i] cards.Length cardMap[i+1]
    i <- i + 1
printfn "%A" cardMap
printfn "Total: %d" cardTotal