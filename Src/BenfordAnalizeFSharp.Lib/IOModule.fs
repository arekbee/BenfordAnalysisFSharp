namespace BenfordAnalizeFSharp.Lib
module IOModule=

open System
open System.IO
open StockValueModule

    let readLine (filePath:string) = seq {
        let underlierName = Path.GetFileNameWithoutExtension filePath
        use sr= new StreamReader (filePath)
        while not sr.EndOfStream do
            yield (underlierName, sr.ReadLine())
        }
    
    let getStockRecords (dirWithCsv:string) =
        let files = Directory.GetFiles(dirWithCsv,"*.txt")
        seq {
            for file in files do
                yield! (readLine(file) |> Seq.skip 1)
        }

            
    let splitCommas (l:string) =
        l.Split(',')

    let toStockValue (stockName:string) (valueStr:string) =
        let values = splitCommas valueStr 
            
        {
            Name =stockName; ID=Guid.NewGuid(); 
            Date = DateTime.ParseExact(values.[0],"yyyyMMdd",null); 
            Open  = decimal(values.[1]);
            High  = decimal(values.[2]);
            Low  = decimal(values.[3]);
            Close  = decimal(values.[4]);
            Volume  = int(values.[5]);
        }