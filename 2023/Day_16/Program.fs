open System
open System.Threading;
open Microsoft.FSharp.Collections

type Beam = 
    {
        direction: char
        coordinate: int * int
    }

let (mirrorMap: char array array) = System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> Array.map(fun s -> s |> Seq.toArray)

let getNewCoordByDirection direction (x,y) = 

    let newX, newY = 
        match direction with
        | 'R' -> (x+1, y)
        | 'L' -> (x-1, y)
        | 'U' -> (x, y-1)
        | 'D' -> (x, y+1)
        | _ -> failwith "Direction does not exist"

    if newY < 0 || newY > mirrorMap.Length - 1 || newX < 0 || newX > mirrorMap[0].Length - 1 then
        None
    else Some (newX, newY)

let findNextBeam currentBeam =
    let (x, y) = currentBeam.coordinate

    match mirrorMap[y][x] with 
    | '/' ->
        let newDirection = 
            match currentBeam.direction with 
                | 'U' -> 'R'
                | 'D' -> 'L'
                | 'L' -> 'D'
                | 'R' -> 'U'
                | _ -> failwith "Unrecognized symbol in mirror map!"

        let newCoord = getNewCoordByDirection newDirection currentBeam.coordinate
        match newCoord with
        | Some (x, y) -> 
            {
                direction = newDirection
                coordinate = (x, y)
            } |> List.singleton
        | None -> []

    | '-' ->
        let newDirection = 
            match currentBeam.direction with 
                | 'U'
                | 'D' -> ['R';'L']
                | 'L'
                | 'R' -> [currentBeam.direction]
                | _ -> failwith "Unrecognized symbol in mirror map!"
        newDirection |> List.collect(fun dir ->
            let newCoord = getNewCoordByDirection dir currentBeam.coordinate
            match newCoord with
            | Some (x, y) -> 
                {
                    direction = dir
                    coordinate = (x, y)
                } |> List.singleton
            | None -> []
        )
    | '|' ->
        let newDirection = 
            match currentBeam.direction with 
                | 'U'
                | 'D' -> [currentBeam.direction]
                | 'L'
                | 'R' -> ['U';'D']
                | _ -> failwith "Unrecognized symbol in mirror map!"
        newDirection |> List.collect(fun dir ->
            let newCoord = getNewCoordByDirection dir currentBeam.coordinate
            match newCoord with
            | Some (x, y) -> 
                {
                    direction = dir
                    coordinate = (x, y)
                } |> List.singleton
            | None -> []
        )
    | '.' ->
        let newCoord = getNewCoordByDirection currentBeam.direction currentBeam.coordinate
        match newCoord with
        | Some (x, y) -> 
            {
                direction = currentBeam.direction
                coordinate = (x, y)
            } |> List.singleton
        | None -> []
    | _ -> 
        let newDirection = 
            match currentBeam.direction with 
                | 'U' -> 'L'
                | 'D' -> 'R'
                | 'L' -> 'U'
                | 'R' -> 'D'
                | _ -> failwith "Unrecognized symbol in mirror map!"
        
        let newCoord = getNewCoordByDirection newDirection currentBeam.coordinate
        match newCoord with
        | Some (x, y) -> 
            {
                direction = newDirection
                coordinate = (x, y)
            } |> List.singleton
        | None -> []

let startingBeams = 
    [|0..mirrorMap.Length-1|] 
    |> Array.collect(fun i ->
        [|
            {
                direction = 'D'
                coordinate = (i,0)
            }
            {
                direction = 'U'
                coordinate = (i,mirrorMap.Length-1)
            }
            {
                direction = 'R'
                coordinate = (0,i)
            }
            {
                direction = 'L'
                coordinate = (mirrorMap.Length-1,i)
            }
        |]
    )

let result = 
    startingBeams
    |> Array.Parallel.map(fun startingBeam -> 
        
        printfn "%A" startingBeam

        let mutable beamsToFollow = startingBeam |> List.singleton

        let mutable beamHistory = []

        let mutable illuminatedSquares = [beamsToFollow[0].coordinate]

        while beamsToFollow.Length > 0 do 
            let currentBeam = beamsToFollow.Head
            if (beamHistory |> List.filter(fun beam -> beam.direction = currentBeam.direction && beam.coordinate = currentBeam.coordinate) |> List.length |> fun x -> x >= 1) then 
                beamsToFollow <- List.removeAt 0 beamsToFollow
            else  
                beamHistory <- List.append beamHistory [currentBeam]
                beamsToFollow <- List.removeAt 0 beamsToFollow

                let newBeams = findNextBeam currentBeam
                illuminatedSquares <- List.append (newBeams |> List.map(fun x -> x.coordinate)) illuminatedSquares
                beamsToFollow <- List.append beamsToFollow newBeams
        
        (illuminatedSquares |> List.distinct |> List.length)
    )

printfn "%A" (result |> Array.max)