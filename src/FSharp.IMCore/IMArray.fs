namespace FSharp.IMCore

open System.Linq
open Funtom.Linq

type IMArray<'T> = System.Collections.Immutable.ImmutableArray<'T>
type IMArray = System.Collections.Immutable.ImmutableArray



[<RequireQualifiedAccess>]
module IMArray = 

    [<CompiledName("IsEmpty")>]
    let isEmpty (table: IMArray<_>) =
        table.IsEmpty

    [<CompiledName("Add")>]
    let add key value (table: IMArray<_>) :IMArray<_>=
        table.Add (key, value)

    [<CompiledName("Find")>]
    let find key (table: IMArray<_>) =
        table.[key]
        
    [<CompiledName("Empty")>]
    let empty<'T when 'T : comparison> : IMArray<'T> = IMArray<'T>.Empty
    
    
    [<CompiledName("OfArray")>]
    let ofArray (elements: 'T[]) :IMArray<'T> = IMArray.CreateRange elements
    
    
    let ofSpan (span: System.ReadOnlySpan<'T>) :IMArray<'T>=
        let builder = IMArray.CreateBuilder<'T>(span.Length)
        for i in span do
            builder.Add(i)
        builder.MoveToImmutable()

    [<CompiledName("Filter")>]
    let inline filter ([<InlineIfLambda>] predicate: 'T -> bool) (array: IMArray<_>) :IMArray<_>=
        let builder = IMArray.CreateBuilder(array.Length)
        for k in array do
            if predicate k then
                builder.Add k
        builder.ToImmutable()
        
    [<CompiledName("Filter2")>]
    let inline filter2 ([<InlineIfLambda>] predicate: 'T -> bool) (array: IMArray<_>) :IMArray<_>=
        let builder = IMArray.CreateBuilder(array.Length)
        builder.AddRange(Linq.where predicate array)
        builder.ToImmutable()
    
    [<CompiledName("Filter3")>]
    let inline filter3 ([<InlineIfLambda>] predicate: 'T -> bool) (arr: IMArray<_>) :IMArray<_>=
        System.Array.FindAll(arr.AsSpan().ToArray(), predicate) |> ofArray
        
//    [<CompiledName("Filter4")>]
//    let inline filter4 ([<InlineIfLambda>] predicate: 'T -> bool) (arr: IMArray<_>) :IMArray<_>=
//        
//        let builder = IMArray.Cre(arr.Length)
//        for k in arr do
//            if predicate k then
//                builder.Add k
//        builder.ToImmutable()
//        
        
    [<CompiledName("Fold")>]
    let inline fold<'T, 'State  when 'T : comparison> ([<InlineIfLambda>] folder: 'State -> 'T -> 'State) (state:'State) (array: IMArray<'T>) =
        let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(folder)
        let mutable state = state
        for i = 0 to array.Length-1 do 
            state <- f.Invoke(state, array.[i])
        state
        
        
    [<CompiledName("Map")>]
    let inline map ([<InlineIfLambda>] mapping:'T -> 'U) (array: IMArray<'T>) :IMArray<'U> =
        let result = IMArray.CreateBuilder(array.Length)
        for k in array do
            result.Add(mapping k)
        result.ToImmutable()
        
    [<CompiledName("Map2")>]
    let inline map2 ([<InlineIfLambda>] mapping:'T -> 'U) (array: IMArray<'T>) :IMArray<'U> =
        IMArray.CreateRange(array, mapping)