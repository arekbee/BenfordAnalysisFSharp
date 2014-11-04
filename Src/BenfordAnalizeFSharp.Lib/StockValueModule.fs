namespace BenfordAnalizeFSharp.Lib
module StockValueModule =


open System
open System.Diagnostics

[<DebuggerDisplay("{Name}{Date}")>] 
type StockValue = 
    { 
        Name :string;
        ID : Guid  ;
        Date: DateTime ;
        Open: decimal;
        High: decimal ;
        Low: decimal ;
        Close: decimal;
        Volume :int ;
    }


let toTuple (stockValue:StockValue) = 
    (stockValue.Date.ToString(), stockValue.Open, stockValue.Close, stockValue.Low, stockValue.High)
   

let invokeDynamic (stockValue : StockValue) (prop : string)  (fCast : ('T  -> 'R) ) : 'R = 
        let propType = stockValue.GetType()
        let  refProp = propType.GetProperty(prop)
        let valueObj = refProp.GetValue(stockValue, null) :?> 'R
        fCast(valueObj)

let toTimeSerie  (stockValue : StockValue) (prop : string) (fCast : ('T -> 'R) ) :(DateTime * 'R) =
    let dynvalue :'R =  invokeDynamic stockValue   prop  fCast
    (stockValue.Date , dynvalue)

