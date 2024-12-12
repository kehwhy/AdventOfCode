open System 

let parseLine (str:string) (beginning:string) =
    let split = str.Split beginning
    let split2 = split[1].Split ' '
    let vals = split2 |> Array.filter(fun x -> x.Length > 0) |> Array.map(Int64.Parse)
    vals

let races = 
    let input = System.IO.File.ReadLines("./input.txt") |> Seq.toArray
    let times = parseLine input[0] "Time: "
    let distances = parseLine input[1] "Distance: "
    distances |> Array.zip times
    
let result =
    races |> Array.map(fun (time, distance) -> 
        let mutable count = 0
        let mutable isTooSlow = false
        let mutable timeHeld = time/2L
        while not isTooSlow do 
            let myDistance = timeHeld * (time-timeHeld)
            if myDistance > distance then
                count <- count + 1
                timeHeld <- timeHeld - 1L
            else 
                isTooSlow <- true
        if (time % 2L = 0) then
            count * 2 - 1
        else count * 2
    )

let stopWatch = System.Diagnostics.Stopwatch.StartNew()
printfn "%A" ((1, result) ||> Array.fold(fun acc x -> acc * x))
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds