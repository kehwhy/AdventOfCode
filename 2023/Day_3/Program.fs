open System 

let input = System.IO.File.ReadLines("./input.txt")

let mutable symbolMap = Map.empty
let mutable accessMap = Map.empty

let findSymbol (row:int) (col:int) (line:string) = 
    let isPrevNonDigit = if col = 0 then true else ( not (System.Char.IsDigit(line[col-1])))
    let mutable i = col
    if isPrevNonDigit then
        while (i < line.Length) && System.Char.IsDigit(line[i]) do
            i <- i + 1
        let number = line[col..i-1] |> int
        // printfn "NUMBER: %A" number
        for r = (row-1) to (row+1) do
            // printfn "ROW: %A" r
            for c = (col-1) to (i) do
                // printfn "COLUMN: %A" c
                if symbolMap |> Map.containsKey (r, c) then
                    if accessMap |> Map.containsKey (r, c) then
                        let count, total = accessMap[(r, c)]
                        accessMap <- accessMap |> Map.remove (r, c) 
                        accessMap <- accessMap |> Map.add (r,c) (count+1, total*number)
                    else 
                        accessMap <- accessMap |> Map.add (r,c) (1, number)
    else 
        ()
    
input
|> Seq.iteri(fun row line ->
    line
    |> String.iteri(fun col c ->
    if (not (System.Char.IsDigit(c))) && (not (c = '.')) then 
        symbolMap <- symbolMap.Add((row, col), c) 
    ) 
)  

input
|> Seq.iteri(fun row line ->
    line
    |> String.iteri(fun col c -> 
        if System.Char.IsDigit(c) then
            findSymbol row col line
        else
            ()
    )
)

let mutable result = 0

accessMap.Values |> Seq.iter (fun x -> printfn "%A" x)

accessMap 
|> Map.values 
|> Seq.iter (fun (n, t) -> 
    if n = 2 then
        result <- result + t
)

printfn "%A" result