open System
open System.Threading;
open Microsoft.FSharp.Collections

let mutable myMap = Map.empty
let mutable startPoint = (0,0)

let parseMap (str:string array) =

    str |> Array.iteri(fun i row ->
        row |> Seq.toArray |> Array.iteri(fun j item ->
            myMap <- myMap |> Map.add (i,j) item
            if item = 'S' then 
                startPoint <- (i,j)
        )
    )
    
let findStartingDirection (x,y) = 
    
    let up = myMap[(x-1,y)] 
    let down = myMap[(x+1,y)]
    let left = myMap[(x,y-1)]
    let right = myMap[(x,y+1)] 
    
    if up = '|' || up = '7' || up = 'F' then
        (x-1,y), 'N'
    else if down = '|' || down = 'L' || down = 'J' then
        (x+1,y), 'S'
    else if right = '-' || right = 'J' || right = '7' then
        (x,y+1), 'E'
    else if left = '-' || left = 'L' || left = 'F' then
        (x,y-1), 'W'
    else failwith "Invalid starting point."
        

let findNextCoord (x,y) prevDir = 
    match myMap[(x,y)], prevDir with 
    | '|', 'N' -> (x-1, y), 'N'
    | '|', 'S' -> (x+1, y), 'S'
    | '-', 'E' -> (x, y+1), 'E'
    | '-', 'W' -> (x, y-1), 'W'
    | 'L', 'S' -> (x, y+1), 'E'
    | 'L', 'W' -> (x-1, y), 'N'
    | 'J', 'S' -> (x, y-1), 'W'
    | 'J', 'E' -> (x-1, y), 'N'
    | '7', 'N' -> (x, y-1), 'W'
    | '7', 'E' -> (x+1, y), 'S'
    | 'F', 'N' -> (x, y+1), 'E'
    | 'F', 'W' -> (x+1, y), 'S'
    | s, d -> 
        printfn "Symbol: %A Direction: %A" s d
        failwith "Wrong direction!"

System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> parseMap

let mutable pathPoints = [|startPoint|]
let result = 

    let mutable findingLoop = true
    let mutable currentCoord, prevDirection = findStartingDirection startPoint
    pathPoints <- Array.append pathPoints [|currentCoord|]
    let mutable count = 1

    while not (currentCoord = startPoint)  do 
        let newCord, newDir = findNextCoord currentCoord prevDirection
        currentCoord <- newCord
        prevDirection <- newDir
        pathPoints <- Array.append pathPoints [|currentCoord|]
        count <- count + 1
    
    count/2

let result2 = 
    let zippedArray = Array.pairwise pathPoints
    zippedArray 
    |> Array.map(fun ((x1,y1),(x2,y2))-> 
        (x1 * y2) - (x2 * y1)
    )
    |> Array.sum
    |> fun x -> x/2 |> abs |> float

let result3 = (result2 - 0.5 * ((pathPoints.Length-1) |> float) + 1.0) |> int

printfn "%A" result3