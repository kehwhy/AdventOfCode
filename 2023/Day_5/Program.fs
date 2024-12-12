open System 

type Range = 
    {
        sourceStart: Int64
        sourceEnd: Int64
        destinationStart: Int64
        delta: Int64
    }

type CustomMap = 
    {
        source: string
        destination: string
        ranges: Range list
    }

let parseSeeds (str:string) = 
    let split1 = (str.Split "seeds: ")
    let split2 = split1[1].Split ' '
    
    let vals = split2 |> Array.map (Int64.Parse)

    let mutable seedList = [||]
    let mutable i = 0
    while i < vals.Length do 
        seedList <- seedList |> Array.append [|(vals[i], vals[i+1])|]
        i <- i + 2
    seedList

let parseTitle (str:string) = 
    let split1 = str.Split ' '
    let split2 = split1[0].Split '-'
    (split2[0], split2[2])

let parseRange (str:string) = 
    let split1 = str.Split ' '
    let vals = split1 |> Array.map (Int64.Parse)

    {
        sourceStart = vals[1]
        sourceEnd = vals[1] + vals[2]
        destinationStart = vals[0]
        delta = vals[0] - vals[1]
    } |> List.singleton

let parseMaps (input: string array) = 
    let mutable myMaps = []
    let mutable i = 1
    let mutable ranges = []
    let mutable source = ""
    let mutable destination = ""

    while i < input.Length do
        match input[i].Trim().Length > 0 with 
        | false -> 
            if ranges.Length > 0 then 
                let newMap = {
                    source = source
                    destination = destination
                    ranges = ranges
                }
                myMaps <-List.append myMaps [newMap]
                ranges <- []
                source <- ""
                destination <- ""
            i <- i + 1
        | true -> 
            match (input[i].Split ' ').Length with
            | 2 -> 
                let (sourcet, destinationt) = parseTitle input[i]
                source <- sourcet
                destination <- destinationt
            | 3 -> 
                ranges <- ranges |> List.append (parseRange input[i])
            | _ -> failwith "The input was not in the expected format."
            i <- i + 1
    if ranges.Length > 0 then 
        let newMap = 
            {
                source = source
                destination = destination
                ranges = ranges
            }
        myMaps <-List.append myMaps [newMap]
    myMaps

let input = System.IO.File.ReadLines("./input.txt") |> Seq.toArray
let seeds = parseSeeds input[0]
let parsedMaps = parseMaps input

let findMapBySource source = 
    parsedMaps |> List.find(fun x -> x.source = source)

let checkRanges map seedList =
    // printfn "%A-to-%A" map.source map.destination
    seedList |> Array.Parallel.map(fun index -> 
        let mutable result = index
        let mutable isMapped = false
        map.ranges |> List.iter(fun range ->
            if index >= range.sourceStart && index < range.sourceEnd && not isMapped then
                // printfn "INDEX: %A" index
                // printfn "RANGE SELECTED: %A" range
                result <- index + range.delta
                // printfn "RESULT: %A" result
                isMapped <- true
        )
        result
    )

let result = seeds |> Array.Parallel.map (fun (seed, range) ->
    let stopWatch = System.Diagnostics.Stopwatch.StartNew()
    let mutable workingSeedsList = [|seed..seed+range|]
    let mutable workingMap = findMapBySource "seed"
    let mutable applingMaps = true
    printfn "SEED:%A" seed
    while applingMaps do 
        workingSeedsList <- checkRanges workingMap workingSeedsList
        if workingMap.destination = "location" then 
            applingMaps <- false
        else workingMap <- findMapBySource workingMap.destination
    stopWatch.Stop()
    printfn "%f" stopWatch.Elapsed.TotalMilliseconds
    workingSeedsList |> Array.min
)

printfn "Total: %A" result
printfn "Minimum location: %A" (result |> Array.min)