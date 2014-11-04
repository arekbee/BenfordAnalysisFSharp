namespace BenfordAnalizeFSharp.Lib
module BenfordModule = 
open System
open System.IO




 let getFirstNumber (value) = 
        let head :Char = value.ToString().ToCharArray() |> Seq.filter (fun x->Char.IsDigit x && x <> '0')  |> Seq.head
        int(head.ToString())
    
    let countGrouped (seqValue : seq<_>) = 
        let grouped = seqValue |> Seq.groupBy (fun x -> x) 
        grouped |> Seq.map (fun (key, values) -> (key, values |> Seq.length) )


    let getBenfordDistribution (value : seq<_>)  (baseValue : float) : ('a*float) seq = 
        let sorted = value |> Seq.sort
        sorted |> Seq.map (fun x->  (x,  Math.Log(1. +  1. / x,  baseValue) ) )

    let getDistribution  (value : seq<'key * int>) : ('key * float) seq = 
        let sum  = value |> Seq.map snd |> Seq.map float |> Seq.sum
        let sortedValues = value |> Seq.sort
        sortedValues |> Seq.map (fun (k,v)-> (k, float(v)/sum))


