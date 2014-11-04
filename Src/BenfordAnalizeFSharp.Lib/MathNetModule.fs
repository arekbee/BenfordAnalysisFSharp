namespace BenfordAnalizeFSharp.Lib
module MathNetModule=

open MathNet.Numerics
open MathNet.Numerics.LinearAlgebra
open System.Collections.Generic
open MathNet.Numerics.Data.Matlab

    let saveMatrices (fileName:string) (matrices: Matrix<_> seq ) (matricesName :string seq) :unit = 
            let results = new Dictionary<string,Matrix<_>> ()
            let seqAll = Seq.zip  matricesName matrices
            for matrix in seqAll do
                results.Add(fst matrix, snd matrix)

            MatlabWriter.Write(fileName, results )

    let toseq x =
        seq {yield x}

    
    let castValuesToFloat (tSeq :('a * 'b) seq) : (float * float) seq =
        tSeq |> Seq.map (fun (i1 , i2) -> (float(i1.ToString()) , float(i2.ToString()))  )

     
    let convertSeqTuple2ToMatrix (inputs :seq<float * float>) = 
        let length = inputs |> Seq.length
        let matrixTogo = Matrix<'T>.Build.Dense( 2,  length)
        
        let mutable  counter = ref 0
        for input in inputs do
            matrixTogo.At(0,!counter, (fst input))
            matrixTogo.At(1,!counter, (snd input))
            counter := !counter + 1

        matrixTogo