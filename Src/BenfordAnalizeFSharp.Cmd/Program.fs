




open BenfordAnalizeFSharp.Lib



[<EntryPoint>]
let main argv = 

    let mutable inputDataPath =  ref "C:\\UBS\Dev\\data\\wse shares\\" 
    if argv.Length > 0  then 
        inputDataPath := argv.[0]

    let records = IOModule.getStockRecords !inputDataPath |> Seq.map (fun (name , vStr) -> IOModule.toStockValue name vStr ) 

    let getTimeSerie (stocks : seq<StockValueModule.StockValue>) (propStr:string) =
        stocks |> Seq.map (fun x-> StockValueModule.toTimeSerie x  propStr decimal )

    let groupByDate (stocks : seq<StockValueModule.StockValue>) =
        query { for stock in stocks do 
                 where (stock.Date.Year  > 1999)
                 groupBy stock.Date.Year into g
                 let ts = getTimeSerie  g "Open" |> Seq.toList  //"Close"  "Volume"
                 sortBy g.Key
                 select (g.Key, ts  )
                 }

    let allDates = groupByDate records 

    let datesAndDist = seq { 
        for (k,v) in allDates do
        let numbers = v |> Seq.map snd |> Seq.filter (fun x-> x > 0M)
        let fisrtNumers = [ for n in numbers do 
                          yield BenfordModule.getFirstNumber n ]

        let countGroup = fisrtNumers  |> BenfordModule.countGrouped 
        let benford  = BenfordModule.getDistribution countGroup 
        let benfordSum = benford |> Seq.sumBy snd
        yield  (k,benford)
        }


    let years = datesAndDist |> Seq.map (fun (k,v)-> k.ToString())
    let matrixes = seq {
        for k,b in datesAndDist do
            let bFloat = b |> Seq.map (fun (i1,i2) -> (float(i1), i2) )
            let matrix = MathNetModule.convertSeqTuple2ToMatrix bFloat
            printfn "%A " matrix
            yield matrix
        }

    
    MathNetModule.saveMatrices "file.m"  matrixes years



    0 // return an integer exit code
