namespace FSharp.IMCore

type IMSet<'T> = System.Collections.Immutable.ImmutableSortedSet<'T>
type IMSet = System.Collections.Immutable.ImmutableSortedSet


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module IMSet =
    
    [<CompiledName("OfSeq")>]
    let ofSeq (elements: seq<'T>) :IMSet<'T> = IMSet.CreateRange elements

    [<CompiledName("IsEmpty")>]
    let isEmpty (set: IMSet<'T>) = set.IsEmpty

    [<CompiledName("Contains")>]
    let contains element (set: IMSet<'T>) = set.Contains element

    [<CompiledName("Add")>]
    let add value (set: IMSet<'T>) :IMSet<'T> = set.Add value

    [<CompiledName("Singleton")>]
    let singleton value :IMSet<'T>= (IMSet.Empty.Add value)

    [<CompiledName("Remove")>]
    let remove value (set: IMSet<'T>) :IMSet<'T> = set.Remove value

    [<CompiledName("Union")>]
    let union (set1: IMSet<'T>) (set2: IMSet<'T>)  :IMSet<'T> = set1.Union set2

    [<CompiledName("UnionMany")>]
    let unionMany (sets:seq<IMSet<'T>>) :IMSet<'T> =
        Seq.fold (fun s1 s2 -> union s1 s2) IMSet<'T>.Empty sets

    [<CompiledName("Intersect")>]
    let intersect (set1: IMSet<'T>) (set2: IMSet<'T>) :IMSet<'T> = set1.Intersect set2

    [<CompiledName("IntersectMany")>]
    let intersectMany sets  = Seq.reduce (fun s1 s2 -> intersect s1 s2) sets

    [<CompiledName("Iterate")>]
    let iter action (set: IMSet<'T>)  = Seq.iter action set

    [<CompiledName("Empty")>]
    let empty<'T when 'T : comparison> : IMSet<'T> = IMSet<'T>.Empty

    [<CompiledName("ForAll")>]
    let forall predicate (set: IMSet<'T>) =
        //a little bit faster than Seq.forall, but slower than Set.forall
        let mutable ret = true
        if set.IsEmpty then ret
        else
            for i in set do
                if not (predicate i) then
                    ret <- false
            ret
            
    [<CompiledName("Exists")>]
    let exists predicate (set: IMSet<'T>) = Seq.exists predicate set

    [<CompiledName("Filter")>]
    let filter predicate (set: IMSet<'T>) = Seq.filter predicate set |> ofSeq 

    [<CompiledName("Partition")>]
    let partition predicate (set: IMSet<'T>) = ((filter predicate set),filter (predicate >> not) set)

    [<CompiledName("Fold")>]
    let fold<'T, 'State  when 'T : comparison> folder (state:'State) (set: IMSet<'T>) =
        Seq.fold folder state set

    [<CompiledName("FoldBack")>]
    let foldBack<'T, 'State when 'T : comparison> folder (set: IMSet<'T>) (state:'State) =
        Seq.fold folder state set

    [<CompiledName("Map")>]
    let map mapping (set: IMSet<'T>) = Seq.map mapping set |> ofSeq

    [<CompiledName("Count")>]
    let count (set: IMSet<'T>) = set.Count

    [<CompiledName("OfList")>]
    let ofList elements = List.toSeq elements |> ofSeq

    [<CompiledName("OfArray")>]
    let ofArray (array: 'T array) = Seq.ofArray array |> ofSeq
    
    [<CompiledName("ToList")>]
    let toList (set: IMSet<'T>) = set |> Seq.toList

    [<CompiledName("ToArray")>]
    let toArray (set: IMSet<'T>) = set |> Seq.toArray

    [<CompiledName("ToSeq")>]
    let toSeq (set: IMSet<'T>) = (set:> seq<'T>)
    
    [<CompiledName("Difference")>]
    let difference (set1: IMSet<'T>) (set2: IMSet<'T>) :IMSet<'T> = set2.Except(set1)

    [<CompiledName("IsSubset")>]
    let isSubset (set1:IMSet<'T>) (set2: IMSet<'T>) =  set1.IsSubsetOf(set2)

    [<CompiledName("IsSuperset")>]
    let isSuperset (set1:IMSet<'T>) (set2: IMSet<'T>) =  set1.IsSupersetOf(set2)

    [<CompiledName("IsProperSubset")>]
    let isProperSubset (set1:IMSet<'T>) (set2: IMSet<'T>) =  set1.IsProperSubsetOf(set2)

    [<CompiledName("IsProperSuperset")>]
    let isProperSuperset (set1:IMSet<'T>) (set2: IMSet<'T>) = set1.IsProperSupersetOf(set2)

    [<CompiledName("MinElement")>]
    let minElement (set: IMSet<'T>) = set.Min

    [<CompiledName("MaxElement")>]
    let maxElement (set: IMSet<'T>) = set.Max
