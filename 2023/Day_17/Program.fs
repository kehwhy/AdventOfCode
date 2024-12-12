open System
open System.Threading;
open Microsoft.FSharp.Collections

type Node = 
    {
        numConsecMoves: int
        position: int * int
        direction: int * int
        g: int
        h: int
    }

let (grid: int array array) = System.IO.File.ReadLines("./input.txt") |> Seq.toArray |> Array.map(fun s -> s |> Seq.toArray |> Array.map(fun c -> c |> Char.GetNumericValue |> int))

let getH (x, y) = grid.Length - 1 - y + grid[0].Length - 1 - x

let isInList (list:Node list) (item:Node) = 
    list |> List.exists(fun n -> 
        n.numConsecMoves = item.numConsecMoves &&
        n.direction = item.direction &&
        n.position = item.position &&
        n.g <= item.g
    )

let getValidMoves (node:Node) = 
    let dx, dy = node.direction
    let validMoves =
        if node.numConsecMoves < 4 then 
            [node.direction]
        else if node.numConsecMoves >= 10 then
            [(dy, dx); (-dy, -dx)]
        else [node.direction; (dy, dx); (-dy, -dx)]
        
    validMoves |> List.filter(fun (x, y) -> 
        let (prevX, prevY) = node.position
        let newX = prevX + x  
        let newY = prevY + y
        newX >= 0 && newX < grid[0].Length && newY >= 0 && newY < grid.Length
    )

let getChildren (node:Node) = 
    let prevX, prevY = node.position
    (getValidMoves node)
    |> List.map(fun (x, y) ->
        {
            numConsecMoves = if (x, y) = node.direction then node.numConsecMoves + 1 else 1
            position = 
                let prevX, prevY = node.position
                (prevX + x, prevY + y)
            direction = (x, y)
            g = node.g + grid[prevY + y][prevX + x]
            h = (prevX + x, prevY + y) |> getH
        }
    )

let mutable priorityQueue = 
    [
        {
            numConsecMoves = 0
            position = (0, 0)
            direction = (0, 1)
            g = 0
            h = getH (0, 0)
        } 
        {
            numConsecMoves = 0
            position = (0, 0)
            direction = (1, 0)
            g = 0
            h = getH (0, 0)
        } 
    ]

let mutable i = 0
let mutable finalPathLengths = []

let mutable closedList = []

while priorityQueue.Length > 0  && finalPathLengths.Length = 0 do
    let currentNode = priorityQueue |> List.sortBy(fun n -> n.g + n.h) |> List.head
    closedList <- List.append closedList [currentNode]
    printfn "%A" (currentNode.g + currentNode.h)

    let children = getChildren currentNode
    let childrenToExplore = children |> List.filter(fun n -> 
        if n.position = (grid[0].Length-1, grid.Length-1) && n.numConsecMoves > 3 then
            finalPathLengths <- List.append finalPathLengths [n.g]
            false
        else not (isInList closedList n) && not (isInList priorityQueue n)
    )
    i <- i + 1
    priorityQueue <- priorityQueue |> List.sortBy(fun n -> n.g + n.h) |> List.removeAt 0
    priorityQueue <- List.append priorityQueue childrenToExplore

printfn "%A" finalPathLengths