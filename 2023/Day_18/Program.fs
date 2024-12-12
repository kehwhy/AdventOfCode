open System
open System.Threading;
open Microsoft.FSharp.Collections

type Instruction = 
    {
        color: string
        direction: char
        count: int
    }

let mapToInstruction (i: string) = 
    let split = i.Split(' ')
    {
        color = split[2]
        direction = 
            let directionString = split[0] |> Seq.toArray
            directionString[0]
        count = split[1] |> Int32.Parse
    }

let polygon_area (vertices: (int * int) list) = 
    

    let mutable psum = 0
    let mutable nsum = 0

    for i in 0..vertices.Length-1 do
        let sindex = (i + 1) % vertices.Length
        let x, y = vertices[sindex]
        let xi, yi = vertices[i]
        let prod = xi * y
        psum <- psum + prod

    for i in 0..vertices.Length-1 do
        let sindex = (i + 1) % vertices.Length
        let x, y = vertices[sindex]
        let xi, yi = vertices[i]
        let prod = x * yi
        nsum <- nsum + prod

    let y = (psum - nsum) |> float
    let h = 0.5 * y
    abs h

let instructions = System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> Array.map(fun s -> s |> mapToInstruction)

let mutable xCoordinate = 0
let mutable yCoordinate = 0
let mutable coordinates = [(0, 0)]

instructions |> Array.iter(fun instruction ->
    match instruction.direction with
    | 'R' -> xCoordinate <- xCoordinate + instruction.count
    | 'L' -> xCoordinate <- xCoordinate - instruction.count
    | 'U' -> yCoordinate <- yCoordinate - instruction.count
    | 'D' -> yCoordinate <- yCoordinate + instruction.count
    | _ -> failwith "NOT A DIRECTION!"

    coordinates <- List.append coordinates [(yCoordinate, xCoordinate)]
)

let sortedCoordinates = polygon_area coordinates
    

printfn "%A" coordinates