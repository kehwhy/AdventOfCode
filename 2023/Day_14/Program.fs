open System
open System.Threading;
open Microsoft.FSharp.Collections

let goNorthOrWest rocks =     
    let mutable finalRocks = [||]
    rocks 
    |> Array.iter(fun column ->
        let mutable newColumn = [||]
        column |> Array.iteri(fun i c ->
            if c = 'O' then
                newColumn <- Array.append newColumn [|c|]
            else if c = '#' then
                let mutable j = newColumn.Length
                while j < i do
                    newColumn <- Array.append newColumn [|'.'|]
                    j <- newColumn.Length
                newColumn <- Array.append newColumn [|c|]
        )
        let mutable j = newColumn.Length
        while j < column.Length do
            newColumn <- Array.append newColumn [|'.'|]
            j <- newColumn.Length
        finalRocks <- Array.append finalRocks [|newColumn|]
    )
    finalRocks

let goSouthOrEast columnOrientation =     
    columnOrientation 
    |> Array.map(fun s -> s |> Array.rev)
    |> goNorthOrWest
    |> Array.map(fun s -> s |> Array.rev)

let calculateTotalLoad (arr: char array array) = 
    arr 
    |> Array.transpose 
    |> Array.mapi(fun i s -> 
        s |> Array.filter (fun c -> c = 'O') 
        |> Array.length 
        |> fun l -> (l * (arr.Length - i))
    ) |> Array.sum

let originalOrientation = System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> Array.map(fun x -> x |> Seq.toArray)
let columnOrientation = originalOrientation |> Array.transpose
let numCycles = 1000000000

let result = 
    let mutable newRocks = columnOrientation
    [|1..numCycles|]
    |> Array.iter(fun i ->
        newRocks <- newRocks |> goNorthOrWest |> Array.transpose |> goNorthOrWest |> Array.transpose |> goSouthOrEast |> Array.transpose |> goSouthOrEast |> Array.transpose
        printfn "Cycle %A: %A" i (calculateTotalLoad newRocks)
    )

printfn "%A" result