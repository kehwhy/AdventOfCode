open System

let expandStarMap (str:string array) =
    let mutable (fullStarMap:char array array) = [||]
    let mutable rows = []
    let mutable cols = []

    str |> Array.iteri(fun i row ->
        fullStarMap <- Array.append fullStarMap [|(row |> Seq.toArray)|]
        if row |> Seq.toArray |> Array.forall(fun x -> x='.') then
            rows <- List.append rows [i]
    )

    let mutable columns = [||]
    fullStarMap |> Array.iteri(fun i row -> 
        row |> Array.iteri(fun j col -> 
            if i = 0 then
                columns <- Array.append columns [|(col='.')|]
            else 
                columns <- Array.updateAt j (columns[j] && col='.') columns
        )
    )

    columns |> Array.iteri(fun j col -> 
        if col=true then
            cols <- List.append cols [j]
    )

    fullStarMap, rows, cols
    
let extractGalaxies (str:char array array) =
    let mutable galaxyList = []
    str |> Array.iteri(fun i row ->
        row |> Array.iteri(fun j item ->
            if item = '#' then
                galaxyList <- List.append galaxyList [(i,j)]
        )
    )

    galaxyList

let input, rows, cols = System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> expandStarMap

let newInput = input |> extractGalaxies

let mutable sum = 0L
let result = 
    newInput 
    |> List.iteri(fun i x -> 
        List.allPairs [x] newInput[i..newInput.Length-1]
        |> List.map(fun ((x1,y1), (x2,y2)) ->
            let intersectingRows = rows |> List.filter(fun x -> (x > x1 && x < x2) || (x > x2 && x < x1))
            let intersectingCols = cols |> List.filter(fun x -> (x > y1 && x < y2) || (x > y2 && x < y1))
            let xDiff = (x1-x2) |> int64 |> abs 
            let yDiff = (y1-y2) |> int64 |> abs
            xDiff + 999999L * (intersectingRows.Length |> int64) + yDiff + 999999L * (intersectingCols.Length |> int64)
        )
        |> List.sum
        |> fun x -> sum <- sum + x
    )

printfn "%A" sum