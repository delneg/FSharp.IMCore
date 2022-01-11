namespace FSharp.IMCore

open System.Collections.Generic
open Funtom.Linq

type IMMap<'Key,'Value> = System.Collections.Immutable.ImmutableSortedDictionary<'Key,'Value>
type IMMap = System.Collections.Immutable.ImmutableSortedDictionary

//
//|      Method |     N |        Mean |     Error |    StdDev |   Gen 0 |   Gen 1 | Allocated |
//|------------ |------ |------------:|----------:|----------:|--------:|--------:|----------:|
//|     MapFold | 10000 |    14.67 us |  0.124 us |  0.116 us |       - |       - |         - |
//|   IMMapFold | 10000 |    48.58 us |  0.382 us |  0.339 us |       - |       - |         - |
//|      MapMap | 10000 |   209.19 us |  2.183 us |  2.042 us | 35.6445 | 17.3340 | 298,944 B |
//|    IMMapMap | 10000 | 2,508.64 us | 22.058 us | 20.633 us | 35.1563 | 15.6250 | 316,995 B |
//|   MapFilter | 10000 |    62.03 us |  0.768 us |  0.641 us |  7.0801 |  0.2441 |  59,368 B |
//| IMMapFilter | 10000 |   151.94 us |  1.780 us |  1.578 us |       - |       - |      64 B |


[<RequireQualifiedAccess>]
module IMMap = 

    [<CompiledName("IsEmpty")>]
    let isEmpty (table: IMMap<_, _>) =
        table.IsEmpty

    [<CompiledName("Add")>]
    let add key value (table: IMMap<_, _>) :IMMap<_, _>=
        table.Add (key, value)

    [<CompiledName("Find")>]
    let find key (table: IMMap<_, _>) =
        table.[key]

    [<CompiledName("TryFind")>]
    let tryFind key (table: IMMap<_, _>) =
        let mutable value = Unchecked.defaultof<_>
        if table.TryGetValue(key, &value) then Some value else None

    [<CompiledName("Change")>]
    let change (key: 'Key) (f:'T option -> 'T option) (table: IMMap<'Key, 'T>) :IMMap<'Key, 'T>=
        match f (tryFind key table) with
        | Some x -> table.SetItem(key, x)
        | None -> table
        
    [<CompiledName("Remove")>]
    let remove key (table: IMMap<_, _>) :IMMap<_, _>=
        table.Remove key

    [<CompiledName("ContainsKey")>]
    let containsKey key (table: IMMap<_, _>) =
        table.ContainsKey key

    [<CompiledName("Iterate")>]
    let iter (action:'Key -> 'T -> unit) (table: IMMap<_, _>) =
        Seq.iter (fun (KeyValue(key, value)) -> action key value) table

    [<CompiledName("TryPick")>]
    let tryPick (chooser:'Key -> 'T -> 'c option) (table: IMMap<_, _>) :'c option =
        if table.IsEmpty then None
        else
          Seq.tryPick (fun (KeyValue(key, value)) -> chooser key value) table

    [<CompiledName("Pick")>]
    let pick chooser (table: IMMap<_, _>) =
        match tryPick chooser table with
        | None -> raise (KeyNotFoundException())
        | Some res -> res

    [<CompiledName("Exists")>]
    let exists (predicate:'Key -> 'T -> bool) (table: IMMap<_, _>) :bool=
        Seq.exists (fun (KeyValue(key, value)) -> predicate key value) table

    [<CompiledName("Filter")>]
    let inline filter ([<InlineIfLambda>] predicate:'Key -> 'T -> bool) (table: IMMap<_, _>) :IMMap<_, _>=
        let builder = table.ToBuilder()
        for kvp in table do
            if predicate kvp.Key kvp.Value then
                builder.Add kvp
        builder.ToImmutable()

    [<CompiledName("Partition")>]
    let partition (predicate:'Key -> 'T -> bool) (table: IMMap<_, _>) :IMMap<'Key, 'T> * IMMap<'Key, 'T>=
        let first = filter predicate table
        (first, table.RemoveRange(first.Keys))

    [<CompiledName("ForAll")>]
    let forall predicate (table: IMMap<_, _>) =
        Seq.forall (fun (KeyValue(key, value)) -> predicate key value) table

    [<CompiledName("OfSeq")>]
    let ofSeq (elements: seq<'Key * 'T>) :IMMap<'Key, 'T> =
        IMMap.Empty.SetItems(elements |> Seq.map (fun (key,value) -> KeyValuePair(key,value)))
     
    [<CompiledName("Map")>]
    let inline map ([<InlineIfLambda>] mapping:'Key -> 'T -> 'U) (table: IMMap<'Key, 'T>) :IMMap<'Key, 'U>=
        let builder = IMMap.CreateBuilder()
        for kvp in table do
            builder.[kvp.Key] <- mapping kvp.Key kvp.Value
        builder.ToImmutable()

    [<CompiledName("Map2")>]
    let inline map2 ([<InlineIfLambda>] mapping:'Key -> 'T -> 'U) (table: IMMap<'Key, 'T>) :IMMap<'Key, 'U>=
        [|
            for kvp in table do
                yield KeyValuePair(kvp.Key, mapping kvp.Key kvp.Value)
        |] |> IMMap.CreateRange
        
    [<CompiledName("Map3")>]
    let inline map3 ([<InlineIfLambda>] mapping:'Key -> 'T -> 'U) (table: IMMap<'Key, 'T>) :IMMap<'Key, 'U>=
        let builder = IMMap.CreateBuilder()
        for kvp in table do
            builder.Add(KeyValuePair(kvp.Key,mapping kvp.Key kvp.Value))
        builder.ToImmutable()
        
    [<CompiledName("Fold")>]
    let inline fold<'Key, 'T, 'State when 'Key : comparison> ([<InlineIfLambda>] folder:'State -> 'Key -> 'T -> 'State) (state:'State) (table: IMMap<'Key, 'T>) =
        let mutable s = state
        for kvp in table do
            s <- folder s kvp.Key kvp.Value
        s
    

    [<CompiledName("FoldBack")>]
    let foldBack<'Key, 'T, 'State  when 'Key : comparison> folder (table: Map<'Key, 'T>) (state:'State) =
        Seq.foldBack (fun (KeyValue(key, value)) s -> folder s key value) table state
    
    [<CompiledName("ToSeq")>]
    let toSeq (table: IMMap<_, _>) =
        table |> Seq.map (fun kvp -> kvp.Key, kvp.Value)

    [<CompiledName("FindKey")>]
    let findKey predicate (table : IMMap<_, _>) =
        table |> Seq.pick (fun kvp -> let k = kvp.Key in if predicate k kvp.Value then Some k else None)

    [<CompiledName("TryFindKey")>]
    let tryFindKey predicate (table : IMMap<_, _>) =
        table |> Seq.tryPick (fun kvp -> let k = kvp.Key in if predicate k kvp.Value then Some k else None)

    [<CompiledName("OfList")>]
    let ofList (elements: ('Key * 'Value) list)  :IMMap<'Key, 'Value> =
        IMMap.Empty.SetItems(elements |> List.map (fun (key,value) -> KeyValuePair(key,value)))

    [<CompiledName("OfArray")>]
    let ofArray (elements: ('Key * 'Value) array) :IMMap<_, _> = 
       IMMap.Empty.SetItems(elements |> Array.map (fun (key,value) -> KeyValuePair(key,value)))

    [<CompiledName("ToList")>]
    let toList (table: IMMap<_, _>) =
        Seq.map (fun (KeyValue(key, value)) -> (key,value)) table |> Seq.toList

    [<CompiledName("ToArray")>]
    let toArray (table: IMMap<_, _>) =
        Seq.map (fun (KeyValue(key, value)) -> (key,value)) table |> Seq.toArray

    [<CompiledName("Empty")>]
    let empty<'Key, 'Value  when 'Key : comparison> :IMMap<_, _>=
        IMMap<'Key, 'Value>.Empty

    [<CompiledName("Count")>]
    let count (table: IMMap<_, _>) =
        table.Count