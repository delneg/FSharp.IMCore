namespace FSharp.IMCore
open System.Collections.Generic
open System.Collections.Immutable


type IMList<'T> = System.Collections.Immutable.ImmutableList<'T>
type IMList = System.Collections.Immutable.ImmutableList

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module IMList =
    
    let inline checkNotNull name arg =
        match arg with
        |null -> nullArg name
        |_ -> ()
    let inline internal check (list: IImmutableList<_>) = checkNotNull "list" list
    
    let inline tryItem index (list:IMList<'T>) =
        check list
        if index >= list.Count || index < 0 then None
        else Some(list.[index])
        
    let inline removeAt index (list:IMList<'T>) :IMList<'T> =
        check list
        list.RemoveAt index

    
    [<CompiledName("Length")>]
    let length (list: IMList<'T>) = list.Count

    [<CompiledName("Last")>]
    let last (list: IMList<'T>) =
        check list
        list.[list.Count - 1]

    [<CompiledName("TryLast")>]
    let tryLast (list: IMList<'T>) = tryItem (list.Count - 1) list

    [<CompiledName("Reverse")>]
    let rev (list: IMList<'T>) :IMList<'T> = list.Reverse()

    [<CompiledName("Concat")>]
    let concat (lists: seq<IMList<'T>>) :IMList<'T> =
        let builder = IMList.CreateBuilder()
        for list in lists do
            builder.AddRange(list)
        builder.ToImmutable()

    [<CompiledName("OfSeq")>]
    let ofSeq (source:seq<'a>) :IMList<'a> = IMList.CreateRange source
//    let inline countByImpl (comparer:IEqualityComparer<'SafeKey>) (projection:'T->'SafeKey) (getKey:'SafeKey->'Key) (list:IMList<'T>) =
//        match list with
//        | [] -> []
//        | _ ->
//
//        let dict = Dictionary comparer
//        let rec loop srcList  =
//            match srcList with
//            | [] -> ()
//            | h :: t ->
//                let safeKey = projection h
//                let mutable prev = 0
//                if dict.TryGetValue(safeKey, &prev) then dict.[safeKey] <- prev + 1 else dict.[safeKey] <- 1
//                loop t
//        loop list
//        Microsoft.FSharp.Primitives.Basics.List.countBy dict getKey
//
//    // We avoid wrapping a StructBox, because under 64 JIT we get some "hard" tailcalls which affect performance
//    let countByValueType (projection:'T->'Key) (list:IMList<'T>) = countByImpl HashIdentity.Structural<'Key> projection id list
//
//    // Wrap a StructBox around all keys in case the key type is itself a type using null as a representation
//    let countByRefType   (projection:'T->'Key) (list:IMList<'T>) = countByImpl RuntimeHelpers.StructBox<'Key>.Comparer (fun t -> RuntimeHelpers.StructBox (projection t)) (fun sb -> sb.Value) list
//
//    [<CompiledName("CountBy")>]
//    let countBy (projection:'T->'Key) (list:IMList<'T>) =
//        if typeof<'Key>.IsValueType
//            then countByValueType projection list
//            else countByRefType   projection list

    [<CompiledName("Map")>]
    let map mapping (list:IMList<'T>) :IMList<'U> =
        System.Linq.Enumerable.Select(list,new System.Func<'T,'U>(mapping) ) |> ofSeq
        
//    [<CompiledName("MapIndexed")>]
//    let mapi mapping list = Microsoft.FSharp.Primitives.Basics.List.mapi mapping list
//
//    [<CompiledName("Indexed")>]
//    let indexed list = Microsoft.FSharp.Primitives.Basics.List.indexed list
//
//    [<CompiledName("MapFold")>]
//    let mapFold<'T, 'State, 'Result> (mapping:'State -> 'T -> 'Result * 'State) state list =
//        Microsoft.FSharp.Primitives.Basics.List.mapFold mapping state list
//
//    [<CompiledName("MapFoldBack")>]
//    let mapFoldBack<'T, 'State, 'Result> (mapping:'T -> 'State -> 'Result * 'State) list state =
//        match list with
//        | [] -> [], state
//        | [h] -> let h', s' = mapping h state in [h'], s'
//        | _ ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(mapping)
//            let rec loop res list =
//                match list, res with
//                | [], _ -> res
//                | h :: t, (list', acc') ->
//                    let h', s' = f.Invoke(h, acc')
//                    loop (h' :: list', s') t
//            loop ([], state) (rev list)

    [<CompiledName("Iterate")>]
    let inline iter action (list:IMList<'T>) = for x in list do action x

    [<CompiledName("Distinct")>]
    let distinct (list:IMList<'T>) =
        Seq.distinct list |> ofSeq

    [<CompiledName("DistinctBy")>]
    let distinctBy projection (list:IMList<'T>) =
        Seq.distinctBy projection list |> ofSeq

    [<CompiledName("OfArray")>]
    let ofArray (array:'T array) = Seq.ofArray array |> ofSeq

    [<CompiledName("ToArray")>]
    let toArray (list:IMList<'T>) = Seq.toArray list
    
    [<CompiledName("OfList")>]
    let ofList (l:'T list) = ofSeq l

    [<CompiledName("Empty")>]
    let empty<'T> :IMList<'T> = IMList.Empty

    [<CompiledName("Head")>]
    let head (list: IMList<'T>) = list.[0]

    [<CompiledName("TryHead")>]
    let tryHead list = tryItem 0 list

    [<CompiledName("Tail")>]
    let tail (list:IMList<'T>) = removeAt 0 list

    [<CompiledName("IsEmpty")>]
    let isEmpty (list:IMList<'T>) = list.Count = 0

    [<CompiledName("Append")>]
    let append (list1:IMList<'T>) (list2:IMList<'T>) =
        concat (seq { list1 ; list2 })

    [<CompiledName("Item")>]
    let item index (list:IMList<'T>) = check list; list.[index]

    [<CompiledName("Get")>]
    let nth (list:IMList<'T>) index = item index list

    [<CompiledName("Choose")>]
    let choose (chooser:'T -> 'U option) (list:IMList<'T>) :IMList<'U> =
        let builder = IMList.CreateBuilder()
        for item in list do
            match chooser item with
            |Some item -> builder.Add item
            |None -> ()
        builder.ToImmutable()
    
    [<CompiledName("Choose2")>]
    let choose2 (chooser:'T -> 'U option) (list:IMList<'T>) :IMList<'U> =
        Seq.choose chooser list |> ofSeq

    [<CompiledName("Skip")>]
    let skip index (list:IMList<'T>) :IMList<'T>=
        check list
        list.RemoveRange(0, index)
    
    [<CompiledName("Take")>]
    let take count (list: IMList<'T>) :IMList<'T>=
        check list
        list.RemoveRange(count, list.Count - count)

    [<CompiledName("SplitAt")>]
    let splitAt index (list:IMList<'T>) = take index list, skip index list


    [<CompiledName("IterateIndexed")>]
    let inline iteri action (list: IMList<'T>) =
        let mutable n = 0
        for x in list do action n x; n <- n + 1

    [<CompiledName("Initialize")>]
    let init length initializer =
        let builder = IMList.CreateBuilder()
        for i = 0 to length - 1 do
            builder.Add <| initializer i

    [<CompiledName("Replicate")>]
    let replicate (count:int) (initial: 'T) :IMList<'T> =
        let builder = IMList.CreateBuilder()
        for i in 0..count-1 do
           builder.Add(initial)
        builder.ToImmutable()
//
//    [<CompiledName("Iterate2")>]
//    let iter2 action list1 list2 =
//        let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(action)
//        let rec loop list1 list2 =
//            match list1, list2 with
//            | [], [] -> ()
//            | h1 :: t1, h2 :: t2 -> f.Invoke(h1, h2); loop t1 t2
//            | [], xs2 -> invalidArgDifferentListLength "list1" "list2" xs2.Length
//            | xs1, [] -> invalidArgDifferentListLength "list2" "list1" xs1.Length
//        loop list1 list2
//
//    [<CompiledName("IterateIndexed2")>]
//    let iteri2 action list1 list2 =
//        let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt(action)
//        let rec loop n list1 list2 =
//            match list1, list2 with
//            | [], [] -> ()
//            | h1 :: t1, h2 :: t2 -> f.Invoke(n, h1, h2); loop (n+1) t1 t2
//            | [], xs2 -> invalidArgDifferentListLength "list1" "list2" xs2.Length
//            | xs1, [] -> invalidArgDifferentListLength "list2" "list1" xs1.Length
//        loop 0 list1 list2
//
//    [<CompiledName("Map3")>]
//    let map3 mapping list1 list2 list3 =
//        Microsoft.FSharp.Primitives.Basics.List.map3 mapping list1 list2 list3
//
//    [<CompiledName("MapIndexed2")>]
//    let mapi2 mapping list1 list2 =
//        Microsoft.FSharp.Primitives.Basics.List.mapi2 mapping list1 list2
//
//    [<CompiledName("Map2")>]
//    let map2 mapping list1 list2 = Microsoft.FSharp.Primitives.Basics.List.map2 mapping list1 list2
//
    [<CompiledName("Fold")>]
    let fold<'T, 'State> folder (state:'State) (list: IMList<'T>) =
        match list.Count with
        | 0 -> state
        | _ ->
            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(folder)
            let mutable acc = state
            for x in list do
                acc <- f.Invoke(acc, x)
            acc
//
//    [<CompiledName("Pairwise")>]
//    let pairwise (list: IMList<'T>) =
//        Microsoft.FSharp.Primitives.Basics.List.pairwise list
//
    [<CompiledName("Reduce")>]
    let reduce (reduction:'T -> 'T -> 'T) (list: IMList<'T>) =
        System.Linq.Enumerable.Aggregate(list,new System.Func<'T,'T,'T>(reduction))
//
//    [<CompiledName("Scan")>]
//    let scan<'T, 'State> folder (state:'State) (list:IMList<'T>) =
//        Microsoft.FSharp.Primitives.Basics.List.scan folder state list
//
    [<CompiledName("Singleton")>]
    let inline singleton value :IMList<'T> = IMList.Create(item = value)
//
//    [<CompiledName("Fold2")>]
//    let fold2<'T1, 'T2, 'State> folder (state:'State) (list1:list<'T1>) (list2:list<'T2>) =
//        let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt(folder)
//        let rec loop acc list1 list2 =
//            match list1, list2 with
//            | [], [] -> acc
//            | h1 :: t1, h2 :: t2 -> loop (f.Invoke(acc, h1, h2)) t1 t2
//            | [], xs2 -> invalidArgDifferentListLength "list1" "list2" xs2.Length
//            | xs1, [] -> invalidArgDifferentListLength "list2" "list1" xs1.Length
//        loop state list1 list2
//
//    let foldArraySubRight (f:OptimizedClosures.FSharpFunc<'T, _, _>) (arr: 'T[]) start fin acc =
//        let mutable state = acc
//        for i = fin downto start do
//            state <- f.Invoke(arr.[i], state)
//        state
//
//    // this version doesn't causes stack overflow - it uses a private stack
//    [<CompiledName("FoldBack")>]
//    let foldBack<'T, 'State> folder (list:IMList<'T>) (state:'State) =
//        let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(folder)
//        match list with
//        | [] -> state
//        | [h] -> f.Invoke(h, state)
//        | [h1; h2] -> f.Invoke(h1, f.Invoke(h2, state))
//        | [h1; h2; h3] -> f.Invoke(h1, f.Invoke(h2, f.Invoke(h3, state)))
//        | [h1; h2; h3; h4] -> f.Invoke(h1, f.Invoke(h2, f.Invoke(h3, f.Invoke(h4, state))))
//        | _ ->
//            // It is faster to allocate and iterate an array than to create all those
//            // highly nested stacks.  It also means we won't get stack overflows here.
//            let arr = toArray list
//            let arrn = arr.Length
//            foldArraySubRight f arr 0 (arrn - 1) state
//
//    [<CompiledName("ReduceBack")>]
//    let reduceBack reduction list =
//        match list with
//        | [] -> invalidArg "list" (SR.GetString(SR.inputListWasEmpty))
//        | _ ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(reduction)
//            let arr = toArray list
//            let arrn = arr.Length
//            foldArraySubRight f arr 0 (arrn - 2) arr.[arrn - 1]
//
//    let scanArraySubRight<'T, 'State> (f:OptimizedClosures.FSharpFunc<'T, 'State, 'State>) (arr:_[]) start fin initState =
//        let mutable state = initState
//        let mutable res = [state]
//        for i = fin downto start do
//            state <- f.Invoke(arr.[i], state)
//            res <- state :: res
//        res
//
//    [<CompiledName("ScanBack")>]
//    let scanBack<'T, 'State> folder (list:IMList<'T>) (state:'State) =
//        match list with
//        | [] -> [state]
//        | [h] ->
//            [folder h state; state]
//        | _ ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(folder)
//            // It is faster to allocate and iterate an array than to create all those
//            // highly nested stacks.  It also means we won't get stack overflows here.
//            let arr = toArray list
//            let arrn = arr.Length
//            scanArraySubRight f arr 0 (arrn - 1) state
//
//    let foldBack2UsingArrays (f:OptimizedClosures.FSharpFunc<_, _, _, _>) list1 list2 acc =
//        let arr1 = toArray list1
//        let arr2 = toArray list2
//        let n1 = arr1.Length
//        let n2 = arr2.Length
//        if n1 <> n2 then
//            invalidArgFmt "list1, list2"
//                "{0}\nlist1.Length = {1}, list2.Length = {2}"
//                [|SR.GetString SR.listsHadDifferentLengths; arr1.Length; arr2.Length|]
//        let mutable res = acc
//        for i = n1 - 1 downto 0 do
//            res <- f.Invoke(arr1.[i], arr2.[i], res)
//        res
//
//    [<CompiledName("FoldBack2")>]
//    let rec foldBack2<'T1, 'T2, 'State> folder (list1:'T1 list) (list2:'T2 list) (state:'State) =
//        match list1, list2 with
//        | [], [] -> state
//        | h1 :: rest1, k1 :: rest2 ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _, _>.Adapt(folder)
//            match rest1, rest2 with
//            | [], [] -> f.Invoke(h1, k1, state)
//            | [h2], [k2] -> f.Invoke(h1, k1, f.Invoke(h2, k2, state))
//            | [h2; h3], [k2; k3] -> f.Invoke(h1, k1, f.Invoke(h2, k2, f.Invoke(h3, k3, state)))
//            | [h2; h3; h4], [k2; k3; k4] -> f.Invoke(h1, k1, f.Invoke(h2, k2, f.Invoke(h3, k3, f.Invoke(h4, k4, state))))
//            | _ -> foldBack2UsingArrays f list1 list2 state
//        | [], xs2 -> invalidArgDifferentListLength "list1" "list2" xs2.Length
//        | xs1, [] -> invalidArgDifferentListLength "list2" "list1" xs1.Length
//
//    let rec forall2aux (f:OptimizedClosures.FSharpFunc<_, _, _>) list1 list2 =
//        match list1, list2 with
//        | [], [] -> true
//        | h1 :: t1, h2 :: t2 -> f.Invoke(h1, h2)  && forall2aux f t1 t2
//        | [], xs2 -> invalidArgDifferentListLength "list1" "list2" xs2.Length
//        | xs1, [] -> invalidArgDifferentListLength "list2" "list1" xs1.Length
//
//    [<CompiledName("ForAll2")>]
//    let forall2 predicate list1 list2 =
//        match list1, list2 with
//        | [], [] -> true
//        | _ ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(predicate)
//            forall2aux f list1 list2
//
    [<CompiledName("ForAll")>]
    let forall predicate (list: IMList<'T>) = list.TrueForAll(System.Predicate(predicate))

    [<CompiledName("Exists")>]
    let exists predicate (list: IMList<'T>) =
        list.Exists(System.Predicate(predicate))

    [<CompiledName("Contains")>]
    let inline contains value (source:IMList<'T>) :bool =
        source.Contains(value)
//
//    let rec exists2aux (f:OptimizedClosures.FSharpFunc<_, _, _>) list1 list2 =
//        match list1, list2 with
//        | [], [] -> false
//        | h1 :: t1, h2 :: t2 ->f.Invoke(h1, h2)  || exists2aux f t1 t2
//        | _ -> invalidArg "list2" (SR.GetString(SR.listsHadDifferentLengths))
//
//    [<CompiledName("Exists2")>]
//    let rec exists2 predicate list1 list2 =
//        match list1, list2 with
//        | [], [] -> false
//        | _ ->
//            let f = OptimizedClosures.FSharpFunc<_, _, _>.Adapt(predicate)
//            exists2aux f list1 list2
//
    [<CompiledName("Find")>]
    let rec find predicate (list: IMList<'T>) = 
        list.Find(System.Predicate(predicate))

    [<CompiledName("TryFind")>]
    let rec tryFind predicate (list: IMList<'T>) =
        match list.FindIndex(System.Predicate(predicate)) with
        | -1 -> None
        | x -> Some list.[x]
//
//    [<CompiledName("FindBack")>]
//    let findBack predicate list = list |> toArray |> Microsoft.FSharp.Primitives.Basics.Array.findBack predicate
//
//    [<CompiledName("TryFindBack")>]
//    let tryFindBack predicate list = list |> toArray |> Microsoft.FSharp.Primitives.Basics.Array.tryFindBack predicate
//
    [<CompiledName("TryPick")>]
    let rec tryPick (chooser:'T -> 'U option) (list: IMList<'T>) =
        if list.Count = 0 then None
        else
            match chooser (head list) with
            | None -> tryPick chooser  (tail list)
            | r -> r

//    [<CompiledName("Pick")>]
//    let rec pick chooser list =
//        match list with
//        | [] -> indexNotFound()
//        | h :: t ->
//            match chooser h with
//            | None -> pick chooser t
//            | Some r -> r
//
    [<CompiledName("Filter")>]
    let filter predicate (list:IMList<'T>) :IMList<'T>=
        check list
        list.RemoveAll(System.Predicate(predicate >> not))

    [<CompiledName("ExceptWith")>]
    let exceptWith (comparer: IEqualityComparer<_>) items (list: IMList<'a>) :IMList<'a> =
        check list
        list.RemoveRange(items, comparer)
        
    [<CompiledName("Except")>]
    let except (itemsToExclude: seq<'T>) list =
        exceptWith HashIdentity.Structural itemsToExclude list
//
//    [<CompiledName("Where")>]
//    let where predicate list = Microsoft.FSharp.Primitives.Basics.List.filter predicate list
//
//    let inline groupByImpl (comparer:IEqualityComparer<'SafeKey>) (keyf:'T->'SafeKey) (getKey:'SafeKey->'Key) (list: IMList<'T>) =
//        Microsoft.FSharp.Primitives.Basics.List.groupBy comparer keyf getKey list
//
//    // We avoid wrapping a StructBox, because under 64 JIT we get some "hard" tailcalls which affect performance
//    let groupByValueType (keyf:'T->'Key) (list:IMList<'T>) = groupByImpl HashIdentity.Structural<'Key> keyf id list
//
//    // Wrap a StructBox around all keys in case the key type is itself a type using null as a representation
//    let groupByRefType   (keyf:'T->'Key) (list:IMList<'T>) = groupByImpl RuntimeHelpers.StructBox<'Key>.Comparer (fun t -> RuntimeHelpers.StructBox (keyf t)) (fun sb -> sb.Value) list
//
//    [<CompiledName("GroupBy")>]
//    let groupBy (projection:'T->'Key) (list:IMList<'T>) =
//        if typeof<'Key>.IsValueType
//            then groupByValueType projection list
//            else groupByRefType   projection list
//
//    [<CompiledName("Partition")>]
//    let partition predicate list = Microsoft.FSharp.Primitives.Basics.List.partition predicate list
//
//    [<CompiledName("Unzip")>]
//    let unzip list = Microsoft.FSharp.Primitives.Basics.List.unzip list
//
//    [<CompiledName("Unzip3")>]
//    let unzip3 list = Microsoft.FSharp.Primitives.Basics.List.unzip3 list
//
//    [<CompiledName("Windowed")>]
//    let windowed windowSize list = Microsoft.FSharp.Primitives.Basics.List.windowed windowSize list
//
//    [<CompiledName("ChunkBySize")>]
//    let chunkBySize chunkSize list = Microsoft.FSharp.Primitives.Basics.List.chunkBySize chunkSize list
//
//    [<CompiledName("SplitInto")>]
//    let splitInto count list = Microsoft.FSharp.Primitives.Basics.List.splitInto count list
//
//    [<CompiledName("Zip")>]
//    let zip list1 list2 = Microsoft.FSharp.Primitives.Basics.List.zip list1 list2
//
//    [<CompiledName("Zip3")>]
//    let zip3 list1 list2 list3 = Microsoft.FSharp.Primitives.Basics.List.zip3 list1 list2 list3
//


    [<CompiledName("SkipWhile")>]
    let skipWhile predicate list =
        let condition = ref true
        filter (fun item ->
            if !condition then
                condition := !condition && predicate item
                !condition
            else false) list

    [<CompiledName("SkipUntil")>]
    let skipUntil predicate list = skipWhile (not << predicate) list

    [<CompiledName("TakeWhile")>]
    let takeWhile predicate list =
        let condition = ref true
        filter (fun item ->
            if !condition then
                condition := !condition && predicate item
                not !condition
            else true) list
    
    [<CompiledName("TakeUntil")>]
    let takeUntil predicate list = takeWhile (not << predicate) list
//
//    [<CompiledName("SortWith")>]
//    let sortWith comparer list =
//        match list with
//        | [] | [_] -> list
//        | _ ->
//            let array = Microsoft.FSharp.Primitives.Basics.List.toArray list
//            Microsoft.FSharp.Primitives.Basics.Array.stableSortInPlaceWith comparer array
//            Microsoft.FSharp.Primitives.Basics.List.ofArray array
//
//    [<CompiledName("SortBy")>]
//    let sortBy projection list =
//        match list with
//        | [] | [_] -> list
//        | _ ->
//            let array = Microsoft.FSharp.Primitives.Basics.List.toArray list
//            Microsoft.FSharp.Primitives.Basics.Array.stableSortInPlaceBy projection array
//            Microsoft.FSharp.Primitives.Basics.List.ofArray array
//
    [<CompiledName("Sort")>]
    let sort (list: IMList<'T>) =
        match list.Count with
        | 0 | 1 -> list
        | _ -> list.Sort()
//
//    [<CompiledName("SortByDescending")>]
//    let inline sortByDescending projection list =
//        let inline compareDescending a b = compare (projection b) (projection a)
//        sortWith compareDescending list
//
//    [<CompiledName("SortDescending")>]
//    let inline sortDescending list =
//        let inline compareDescending a b = compare b a
//        sortWith compareDescending list
//
//    
//
    [<CompiledName("ToSeq")>]
    let toSeq (list:IMList<'T>) = list :> seq<'T>

//    [<CompiledName("FindIndex")>]
//    let findIndex predicate list =
//        let rec loop n list = 
//            match list with 
//            | [] -> indexNotFound()
//            | h :: t -> if predicate h then n else loop (n + 1) t
//
//        loop 0 list
//
//    [<CompiledName("TryFindIndex")>]
//    let tryFindIndex predicate list =
//        let rec loop n list = 
//            match list with
//            | [] -> None
//            | h :: t -> if predicate h then Some n else loop (n + 1) t
//
//        loop 0 list
//
//    [<CompiledName("FindIndexBack")>]
//    let findIndexBack predicate list = list |> toArray |> Microsoft.FSharp.Primitives.Basics.Array.findIndexBack predicate
//
//    [<CompiledName("TryFindIndexBack")>]
//    let tryFindIndexBack predicate list = list |> toArray |> Microsoft.FSharp.Primitives.Basics.Array.tryFindIndexBack predicate
//
    [<CompiledName("Sum")>]
    let inline sum (list:IMList<'T>) =
        if list.Count = 0 then
            LanguagePrimitives.GenericZero<'T>
        else
            let mutable acc = LanguagePrimitives.GenericZero<'T>
            for x in list do
                acc <- Checked.(+) acc x
            acc

    [<CompiledName("SumBy")>]
    let inline sumBy (projection: 'T -> 'U) (list:IMList<'T>) =
        if list.Count = 0 then
            LanguagePrimitives.GenericZero<'T>
        else
            let mutable acc = LanguagePrimitives.GenericZero<'T>
            for x in list do
                acc <- Checked.(+) acc (projection x)
            acc
            
//
//    [<CompiledName("Max")>]
//    let inline max (list:list<_>) =
//        match list with
//        | [] -> invalidArg "list" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | h :: t ->
//            let mutable acc = h
//            for x in t do
//                if x > acc then
//                    acc <- x
//            acc
//
//    [<CompiledName("MaxBy")>]
//    let inline maxBy projection (list:list<_>) =
//        match list with
//        | [] -> invalidArg "list" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | h :: t ->
//            let mutable acc = h
//            let mutable accv = projection h
//            for x in t do
//                let currv = projection x
//                if currv > accv then
//                    acc <- x
//                    accv <- currv
//            acc
//
//    [<CompiledName("Min")>]
//    let inline min (list:list<_>) =
//        match list with
//        | [] -> invalidArg "list" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | h :: t ->
//            let mutable acc = h
//            for x in t do
//                if x < acc then
//                    acc <- x
//            acc
//
//    [<CompiledName("MinBy")>]
//    let inline minBy projection (list:list<_>) =
//        match list with
//        | [] -> invalidArg "list" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | h :: t ->
//            let mutable acc = h
//            let mutable accv = projection h
//            for x in t do
//                let currv = projection x
//                if currv < accv then
//                    acc <- x
//                    accv <- currv
//            acc
//
//    [<CompiledName("Average")>]
//    let inline average (list:list<'T>) =
//        match list with
//        | [] -> invalidArg "source" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | xs ->
//            let mutable sum = LanguagePrimitives.GenericZero<'T>
//            let mutable count = 0
//            for x in xs do
//                sum <- Checked.(+) sum x
//                count <- count + 1
//            LanguagePrimitives.DivideByInt sum count
//
//    [<CompiledName("AverageBy")>]
//    let inline averageBy (projection: 'T -> 'U) (list:list<'T>) =
//        match list with
//        | [] -> invalidArg "source" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | xs ->
//            let mutable sum = LanguagePrimitives.GenericZero<'U>
//            let mutable count = 0
//            for x in xs do
//                sum <- Checked.(+) sum (projection x)
//                count <- count + 1
//            LanguagePrimitives.DivideByInt sum count
//
    [<CompiledName("Collect")>]
    let collect mapping list =
        concat <| map mapping list
//
//    [<CompiledName("AllPairs")>]
//    let allPairs list1 list2 = Microsoft.FSharp.Primitives.Basics.List.allPairs list1 list2
//
//    [<CompiledName("CompareWith")>]
//    let inline compareWith (comparer:'T -> 'T -> int) (list1: IMList<'T>) (list2: IMList<'T>) =
//        let rec loop list1 list2 =
//             match list1, list2 with
//             | head1 :: tail1, head2 :: tail2 ->
//                   let c = comparer head1 head2
//                   if c = 0 then loop tail1 tail2 else c
//             | [], [] -> 0
//             | _, [] -> 1
//             | [], _ -> -1
//
//        loop list1 list2
//
//    [<CompiledName("Permute")>]
//    let permute indexMap list = list |> toArray |> Microsoft.FSharp.Primitives.Basics.Array.permute indexMap |> ofArray
//
//    [<CompiledName("ExactlyOne")>]
//    let exactlyOne (list: list<_>) =
//        match list with
//        | [x] -> x
//        | []  -> invalidArg "source" LanguagePrimitives.ErrorStrings.InputSequenceEmptyString
//        | _   -> invalidArg "source" (SR.GetString(SR.inputSequenceTooLong))
//
    [<CompiledName("TryExactlyOne")>]
    let tryExactlyOne (list: IMList<'a>) =
        if list.Count = 1 then Some list.[0] else None
//
//    [<CompiledName("Transpose")>]
//    let transpose (lists: seq<IMList<'T>>) =
//        checkNonNull "lists" lists
//        Microsoft.FSharp.Primitives.Basics.List.transpose (ofSeq lists)
//
    [<CompiledName("Truncate")>]
    let truncate count list =
        if count < length list then take count list else list
//
//    [<CompiledName("Unfold")>]
//    let unfold<'T, 'State> (generator:'State -> ('T*'State) option) (state:'State) = Microsoft.FSharp.Primitives.Basics.List.unfold generator state
