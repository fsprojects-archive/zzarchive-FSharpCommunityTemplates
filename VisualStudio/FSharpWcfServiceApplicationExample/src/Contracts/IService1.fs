namespace FSharpWcfServiceApplicationTemplate.Contracts

open System.Runtime.Serialization
open System.ServiceModel

[<ServiceContract>]
type IService1 =
    [<OperationContract>]
    abstract GetData: value:int -> string
    [<OperationContract>]
    abstract GetDataUsingDataContract: composite:CompositeType -> CompositeType

