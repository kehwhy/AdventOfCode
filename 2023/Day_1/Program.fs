open System 

let input = System.IO.File.ReadLines("./input.txt")

let findNumber (i:int) (c:char) str = 
    let charArray = str |> Seq.toArray
    if i < charArray.Length - 4 then
        match (System.Char.ToLower(charArray[i+0]), System.Char.ToLower(charArray[i+1]), System.Char.ToLower(charArray[i+2]), System.Char.ToLower(charArray[i+3]), System.Char.ToLower(charArray[i+4])) with
            | ('o','n','e', _, _) -> '1'
            | ('t', 'w', 'o', _, _) -> '2'
            | ('t', 'h', 'r', 'e', 'e') -> '3'
            | ('f', 'o', 'u', 'r', _ ) -> '4'
            | ('f','i','v', 'e', _) -> '5'
            | ('s', 'i', 'x', _, _) -> '6'
            | ('s', 'e', 'v', 'e', 'n') -> '7'
            | ('e', 'i', 'g', 'h', 't') -> '8'
            | ('n', 'i', 'n', 'e', _ ) -> '9'
            | (_, _, _, _, _) -> c
    else if i < charArray.Length - 3 then
        match (System.Char.ToLower(charArray[i+0]), System.Char.ToLower(charArray[i+1]), System.Char.ToLower(charArray[i+2]), System.Char.ToLower(charArray[i+3])) with
            | ('o','n','e', _) -> '1'
            | ('t', 'w', 'o', _) -> '2'
            | ('f', 'o', 'u', 'r') -> '4'
            | ('f','i','v', 'e') -> '5'
            | ('s', 'i', 'x', _) -> '6'
            | ('n', 'i', 'n', 'e') -> '9'
            | (_, _, _, _) -> c
    else if i < charArray.Length - 2 then
        match (System.Char.ToLower(charArray[i+0]), System.Char.ToLower(charArray[i+1]), System.Char.ToLower(charArray[i+2])) with
            | ('o','n','e') -> '1'
            | ('t', 'w', 'o') -> '2'
            | ('s', 'i', 'x') -> '6'
            | (_, _, _) -> c
    else c

let result = 
    (0.0, input) ||> Seq.fold (fun acc str -> 
        let numArray = 
            str
            |> Seq.mapi (fun i c -> findNumber i c str)
            |> Seq.filter (fun c -> System.Char.IsDigit(c))
            |> Seq.toArray
        
        numArray |> Array.iter(fun x -> printfn "%c" x)

        numArray |> fun numArray -> acc + Char.GetNumericValue(numArray[0]) * 10.0 + Char.GetNumericValue(numArray[numArray.Length-1])
    )

printfn "%f" result
