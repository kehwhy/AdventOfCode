open System
open System.Threading;
open Microsoft.FSharp.Collections

let parseInput = 
    let input = System.IO.File.ReadLines("./input.txt") |> Seq.toArray
    let mutable start = 0
    let mutable i = 0
    let mutable parseMaps = [||]
    while i < input.Length do
        if input[i].Trim().Length = 0 then
            parseMaps <- Array.append parseMaps [|input[start..i-1]|]
            start <- i+1
        i <- i+1
    parseMaps <- Array.append parseMaps [|input[start..i]|]
    parseMaps

let checkForHorizontalMirror (myMap: string array) = 
    let initialIndex = myMap |> Array.pairwise |> Array.findIndex(fun (x1,x2) -> x1=x2)
    myMap |> Array.mapi(fun i x -> 
        if i < initialIndex then
            let correspondingIndex = initialIndex + (initialIndex - i) + 1
            if correspondingIndex < 0 || correspondingIndex > myMap.Length - 1 then
                true
            else 
                myMap[i] = myMap[correspondingIndex]
        else true
    ) |> fun newMap -> if Array.forall(fun x -> x) newMap then initialIndex + 1 else 0

let checkForMirror (myMap:char array array) = 
    [|0..myMap.Length-2|] 
    |> Array.map(fun initialIndex -> 
        let dist = min (initialIndex) (myMap.Length - 2 - initialIndex)
        [|0..dist|] 
        |> Array.sumBy(fun offset ->
            let a = initialIndex - offset
            let b = initialIndex + 1 + offset
            (Array.zip myMap[a] myMap[b] |> Array.filter(fun (x, y) -> not (x = y)) |> Array.length)
        )
    )
    |> Array.tryFindIndex(fun count -> count = 1)
    |> Option.defaultValue -1


let result = 
    parseInput
    |> Array.map(fun x ->
        let arrayVersion = x |> Array.map(fun y -> y |> Seq.toArray)
        let x1 = checkForMirror arrayVersion
        let x2 = checkForMirror (arrayVersion |> Array.transpose)
        (x1+1) * 100 + (x2+1)
    ) |> Array.filter(fun (x) -> x > 0)
    |> Array.sum

    

printfn "%A" result