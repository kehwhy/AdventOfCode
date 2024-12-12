open System
open System.Threading;
open Microsoft.FSharp.Collections

let applyHASH step = 
    let mutable currentValue = 0
    step 
    |> String.iter(fun c ->
        let ascii = c |> int 
        currentValue <- (currentValue + ascii) * 17 % 256
    )
    currentValue

let initializationSteps = System.IO.File.ReadAllText("./input.txt") |> fun x -> x.Split(',') |> Seq.toArray

let mutable (boxes: (string * string) array array) = Array.create 256 [||]

initializationSteps 
|> Array.iter(fun step -> 
    if step |> String.exists(fun c -> c='-') then 
        let split = step.Split('-')
        let box = applyHASH split[0]
        match Array.tryFindIndex(fun (l, f) -> l = split[0]) boxes[box] with 
        | None -> ()
        | Some i -> boxes[box] <- Array.removeAt i boxes[box]
    if step |> String.exists(fun c -> c='=') then
        let split = step.Split('=')
        let box = applyHASH split[0]
        match Array.tryFindIndex(fun (l, f) -> l = split[0]) boxes[box] with 
        | None -> 
            boxes[box] <- Array.append boxes[box] [|(split[0], split[1])|]
        | Some i -> boxes[box] <- Array.updateAt i (split[0], split[1]) boxes[box]
)

let result = 
    boxes |> Array.mapi(fun i box -> 
        box 
        |> Array.mapi(fun j (label, focal) -> 
            (1 + i) * (1 + j) * (focal |> Int32.Parse)
        ) |> Array.sum
    ) |> Array.sum

printfn "%A" result