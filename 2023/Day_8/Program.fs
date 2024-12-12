open System 
open Microsoft.FSharp.Collections

type Node = 
    {
        here: string
        left: string
        right: string
    }

let endsIn (str:string) (chr:char) = 
    str |> Seq.toArray |> fun str -> str[str.Length-1] = chr

let mutable nodeMap = Map.empty
let mutable startingNodes = [||]

let parseNodes (str:string) =
    let split = str.Split " = ("
    let split2 = split[1].Trim(')').Split ", "
    let myNode = {
        here = split[0]
        left = split2[0]
        right = split2[1]
    }
    nodeMap <- nodeMap |> Map.add split[0] myNode
    if (endsIn split[0] 'A') then
        startingNodes <- startingNodes |> Array.append [|myNode|]
    
let applyDirections (node:Node) (direction:char) = 
    let newNode = 
        match direction with 
        | 'R' -> nodeMap[node.right]
        | 'L' -> nodeMap[node.left]
        | _ -> failwith "Not a direction!"
    newNode

let input = System.IO.File.ReadLines("./input.txt") |> Seq.toArray

let staticDirections = input[0] |> Seq.toArray
let nodes = Array.sub input 2 (input.Length - 2)
nodes |> Array.iter(fun n -> parseNodes n)

let result = 
    startingNodes
    |> Array.map(fun node ->
        let mutable myNode = node
        let mutable i = 0
        let mutable count = 0

        while not (endsIn myNode.here 'Z') do 
            let newNode = applyDirections myNode staticDirections[i]
            i <- if (i = staticDirections.Length-1) then 0 else i + 1 
            count <- count + 1
            myNode <- newNode
        count
    )

printfn "%A" result