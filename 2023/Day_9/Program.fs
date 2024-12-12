open System

let parseSequence (str:string) =
    let split = str.Split ' '
    split |> Array.map(Int32.Parse)

let generateNextSequence (mySeq:int array) = 
    let mutable anArray = [||]
    mySeq |> Array.iteri(fun i s -> 
        if i < mySeq.Length - 1 then 
            anArray <- Array.append anArray [|mySeq[i+1]-mySeq[i]|] 
    )
    anArray
    
let input = System.IO.File.ReadLines("./input.txt") |> Seq.toArray
let seqs = input |> Array.map(parseSequence)

let result = (0,seqs) ||> Array.fold(fun acc mySeq -> 
    let mutable seqMap = Map.empty
    seqMap <- seqMap |> Map.add 0 mySeq
    let mutable i = 0
    while not (seqMap[i] |> Array.forall(fun n -> n=0)) do 
        i <- i+1
        let newSeq = generateNextSequence seqMap[i-1]
        seqMap <- seqMap |> Map.add i newSeq
    
    let mutable prediction = 0
    while i > 0 do 
        let myArray = seqMap[i-1]
        prediction <- myArray[0] - prediction
        i <- i-1
    acc + prediction
)

printfn "%A" result