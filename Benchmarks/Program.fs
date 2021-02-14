open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open Bogus
open FSharp.IMCore
let fake = Faker()
let inline filterPred (x:int64) = x % 2L = 0L

[<PlainExporter; MemoryDiagnoser>]
type CustomSet() =
    [<DefaultValue; Params(1000,10000)>]
    val mutable public N : int
    
    let mutable nums = Set.empty
    let mutable nums2 = Set.empty
    let mutable numsNew = IMSet.empty
    let mutable nums2New = IMSet.empty

    [<GlobalSetup>]
    member this.Setup() =
        nums <- [|for _ in 1..this.N do fake.Random.Long()|] |> Set.ofArray
        nums2 <- [|for _ in 1..this.N do fake.Random.Long()|] |> Set.ofArray
        numsNew <- [|for _ in 1..this.N do fake.Random.Long()|] |> IMSet.ofArray
        nums2New <- [|for _ in 1..this.N do fake.Random.Long()|] |> IMSet.ofArray
        
    [<Benchmark>]
    member _.SetForAll() = Set.forall (fun x -> x > 0L) nums
    [<Benchmark>]
    member _.IMSetForAll() =  IMSet.forall (fun x -> x > 0L) numsNew
    
    [<Benchmark>]
    member _.SetDifference() =
        Set.difference nums nums2
    [<Benchmark>]
    member _.IMSetDifference() =
        IMSet.difference numsNew nums2New
        
    [<Benchmark>]
    member _.SetFold() = Set.fold (+) 0L nums
    [<Benchmark>]
    member _.IMSetFold() = IMSet.fold (+) 0L numsNew
    
    [<Benchmark>]
    member _.SetMap() = Set.map string nums
    [<Benchmark>]
    member _.IMSetMap() = IMSet.map string numsNew
    
    [<Benchmark>]
    member _.SetFilter() = Set.filter filterPred nums
    [<Benchmark>]
    member _.IMSetFilter() = IMSet.filter filterPred numsNew
    

[<EntryPoint>]
let main argv =
    let sets = BenchmarkRunner.Run<CustomSet>()
    0 // return an integer exit code