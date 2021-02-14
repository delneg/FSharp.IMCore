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
        let first = [|for _ in 1..this.N do fake.Random.Long()|]
        let second = [|for _ in 1..this.N do fake.Random.Long()|]
        nums <-  first |> Set.ofArray
        nums2 <- second |> Set.ofArray
        numsNew <- first |> IMSet.ofArray
        nums2New <- second |> IMSet.ofArray
        
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
    
    [<Benchmark>]
    member _.SetUnion() = Set.union nums nums2
    [<Benchmark>]
    member _.IMSetUnion() = IMSet.union numsNew nums2New
    
    [<Benchmark>]
    member _.SetIntersect() = Set.intersect nums nums2
    [<Benchmark>]
    member _.IMSetIntersect() = IMSet.intersect numsNew nums2New
    
    [<Benchmark>]
    member _.SetSingleton() = Set.singleton (nums,nums2)
    [<Benchmark>]
    member _.IMSetSingleton() = IMSet.singleton (numsNew,nums2New)
    
    [<Benchmark>]
    member _.SetMinMax() = Set.minElement nums, Set.minElement nums2, Set.maxElement nums, Set.maxElement nums2
    [<Benchmark>]
    member _.IMSetMinMax() = IMSet.minElement numsNew, IMSet.minElement nums2New, IMSet.maxElement numsNew, IMSet.maxElement nums2New
    


[<PlainExporter; MemoryDiagnoser>]
type CustomMap() =
    [<DefaultValue; Params(1000,10000)>]
    val mutable public N : int
    
    let mutable strs = Map.empty
    let mutable strsNew = IMMap.empty

    [<GlobalSetup>]
    member this.Setup() =
        let first = [|for _ in 1..this.N do (fake.Random.Word(), fake.Random.Long())|]
        strs <-  first |> Map.ofArray
        strsNew <- first |> IMMap.ofArray
        
    [<Benchmark>]
    member _.MapForAll() = Map.forall (fun k v  -> v > 0L) strs
    [<Benchmark>]
    member _.IMMapForAll() =  IMMap.forall (fun k v  -> v > 0L) strsNew
    
        
    [<Benchmark>]
    member _.MapFold() = Map.fold (fun s _ value -> s + value) 0L strs
    [<Benchmark>]
    member _.IMMapFold() = IMMap.fold (fun s _ value -> s + value) 0L strsNew
    
    
    [<Benchmark>]
    member _.MapMap() = Map.map (fun _ value -> string value) strs
    [<Benchmark>]
    member _.IMMapMap() = IMMap.map (fun _ value -> string value) strsNew
    
    [<Benchmark>]
    member _.MapFilter() =
        Map.filter (fun (key:string) _ -> key.Length < 5) strs
        
    [<Benchmark>]
    member _.IMMapFilter() =
        IMMap.filter (fun (key:string) _ -> key.Length < 5) strsNew
    

[<EntryPoint>]
let main argv =
//    let sets = BenchmarkRunner.Run<CustomSet>()
    let maps = BenchmarkRunner.Run<CustomMap>()
    0 // return an integer exit code