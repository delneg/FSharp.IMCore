namespace FSharp.IMCore

open System.Collections.Generic
open System.Threading.Tasks

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
    
    [<CompiledName("Filter")>]
    let inline filter ([<InlineIfLambda>] predicate: 'T -> bool) (array: IMArray<_>) :IMArray<_>=
        let builder = IMArray.CreateBuilder(array.Length)
        for k in array do
            if predicate k then
                builder.Add k
        builder.ToImmutable()
        
        
    [<CompiledName("Fold")>]
    let inline fold<'T, 'State  when 'T : comparison> ([<InlineIfLambda>] folder: 'State -> 'T -> 'State) (state:'State) (array: IMArray<'T>) =
        let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(folder)
        let mutable state = state
        for i = 0 to array.Length-1 do 
            state <- f.Invoke(state, array.[i])
        state
        
        
    [<CompiledName("Map")>]
    let inline map ([<InlineIfLambda>] mapping:'T -> 'U) (array: IMArray<'T>) :IMArray<'U>=
        let inputLength = array.Length
        let result = IMArray.CreateBuilder(inputLength)
        Parallel.For(0, inputLength, fun i ->
            result.Add(mapping array.[i])) |> ignore
        result.ToImmutable()