open System
open System.Threading;
open Microsoft.FSharp.Collections

type Record = 
    {
        record: string
        questionIndices: int list
        currentDamagedSpringCount: int
        damagedSpringPerms: int list
    }

let rec comb n l = 
    match n, l with
    | 0, _ -> [[]]
    | _, [] -> []
    | k, (x::xs) -> List.map ((@) [x]) (comb (k-1) xs) @ comb k xs

let parseRecords (str:string) =
    let split = str.Split ' '
    let split2 = split[1].Split ","
    let listToCopy = split2 |> Array.map(Int32.Parse) |> Array.toList
    let copiedList = [0..4] |> List.collect(fun x -> listToCopy)
    let copiedString = String.concat "?" ([0..4] |> List.map(fun x -> split[0]))
    {
        record = copiedString
        questionIndices = copiedString |> Seq.toArray |> Array.indexed |> Array.filter(fun (i,s) -> s = '?') |> Array.map(fun (i,s) -> i) |> Array.toList
        currentDamagedSpringCount = copiedString |> Seq.filter(fun x -> x='#') |> Seq.length
        damagedSpringPerms = copiedList
    }

let isValidPermutation x record = 
    let mutable newString = record.record |> Seq.toArray
    x |> List.iter(fun x -> newString <- newString |> Array.updateAt x '#')
    let myString = new string(newString)
    let newIndicies = myString.Split([|'?';'.'|]) |> Array.map(fun s -> s.Trim([|'?';'.'|])) |> Array.map(fun s -> s.Length) |> Array.filter(fun x -> not (x = 0)) |> Array.toList
    newIndicies = record.damagedSpringPerms
    
let input = System.IO.File.ReadLines("./input.txt")

let result = 
    input 
    |> Seq.toArray 
    |> Array.Parallel.map(fun s -> parseRecords s)
    |> Array.map(fun record -> 
        let i = (record.damagedSpringPerms |> List.sum) - record.currentDamagedSpringCount
        printfn "%A" i
        let indexs = comb i record.questionIndices |> List.distinct 
        printfn "%A" indexs.Length
        indexs |> List.filter(fun x -> isValidPermutation x record) |> List.length
    )
    |> Array.sum

printfn "%A" result