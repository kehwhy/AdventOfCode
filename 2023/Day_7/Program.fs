open System 

let parseHands (str:string) =
    let split = str.Split ' '
    (split[0], split[1] |> Int32.Parse)

let applyJoker handType = 
    match handType with
    | 0 -> 1
    | 1 -> 3
    | 2 -> 4
    | 3 -> 5
    | 4 -> 5
    | 5 -> 6
    | 6 -> 6
    | _ -> failwith "Unexpected value in hand length"

let getOriginalType (cardCounts: int array) =
    match cardCounts.Length with
    | 5 -> 0
    | 4 -> 1
    | 3 -> if cardCounts |> Array.contains 3 then 3 else 2
    | 2 -> if cardCounts |> Array.contains 4 then 5 else 4
    | 1 -> 6
    | _ -> failwith "Unexpected value in hand length"

let typeHand hand = 

    let countedCards = hand |> Seq.toArray |> Array.countBy(fun x -> x) 

    if countedCards |> Array.map(fun (x,y) -> x) |> Array.contains 'J' then
        let (_, jokerCount) = countedCards |> Array.find(fun (x,y) -> x='J')
        let modifiedHand = (countedCards |> Array.filter(fun (x,y) -> not (x='J'))) |> Array.map(fun (x,y) -> y) |> Array.append (Array.create jokerCount 1)

        let mutable i = jokerCount
        let mutable handType = getOriginalType modifiedHand
        while i > 0 do
            handType <- applyJoker handType
            i <- i - 1
        handType
    else 
        getOriginalType (countedCards |> Array.map(fun (x,y) -> y))
    
let mapHand hand = 
    hand |> String.map(fun x ->
        match x with
        | 'A' -> 'M'
        | 'K' -> 'L'
        | 'Q' -> 'K'
        | 'T' -> 'J'
        | '9' -> 'I'
        | '8' -> 'H'
        | '7' -> 'G'
        | '6' -> 'F'
        | '5' -> 'E'
        | '4' -> 'D'
        | '3' -> 'C'
        | '2' -> 'B'
        | 'J' -> 'A'
        | _ -> failwith "Unexpected card value in hand"
    )

let hands =
    let input = System.IO.File.ReadLines("./input.txt")
    input
    |> Seq.map(fun str -> 
        parseHands str
    )
    |> Seq.toArray

let result = 
    hands
    |> Array.Parallel.map(fun (hand, bet) -> 
        let handType = typeHand hand
        let handRep = mapHand hand
        (handType, handRep, bet)
    )
    |> Array.sort

let mutable total = 0
result 
|> Array.iteri(fun i (_, _, bet) ->
    total <- total + (bet*(i+1))
)

printfn "%A" total