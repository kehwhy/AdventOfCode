open System 

type Draw = 
    {
        color: string
        number: int
    }

type Turn = 
    {
        draws: Draw array
    }

type Game = 
    {
        index: int
        turns: Turn array
    }

type Result = 
    {
        red: int
        blue: int
        green: int
    }

let parseGame (str: string) =
    let split = str.Split ':'
    let index = 
        split[0].Split ' '
    let split2 = split[1].Split ';'
    {
      index = index[1] |> int
      turns = split2|> Array.map(fun x -> 
        let split3 = x.Split ','
        {
            draws = split3 |> Array.map(fun y -> 
                let split4 = y.Split ' '
                {
                    color = split4[2]
                    number = split4[1].Trim() |> int
                }
            )
        }
      )
    }

let applyMoves (arr:int array) (draws:Draw array) = 
    (arr, draws)
    ||> Array.fold (
        fun arr draw ->
            let index = 
                match draw.color with
                | "red" -> 0
                | "blue" -> 1
                | "green" -> 2
                | _ -> 0
            if (arr[index] |> int) < draw.number then
                arr |> Array.updateAt index draw.number
            else
                arr
    )

let input = System.IO.File.ReadLines("./input.txt")
let games = input |> Seq.map(fun x -> parseGame x)

let result = 
    (0, games) ||> Seq.fold (fun acc game -> 
        let maxValues = 
            ([|0; 0; 0|], game.turns) ||> Seq.fold ( 
                fun acc2 turn -> applyMoves acc2 turn.draws
            )
        acc + maxValues[0] * maxValues[1] * maxValues[2]
    )


printfn "%d" result