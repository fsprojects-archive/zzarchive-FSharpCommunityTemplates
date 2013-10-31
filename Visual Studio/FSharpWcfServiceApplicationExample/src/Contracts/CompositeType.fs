namespace FSharpWcfServiceApplicationTemplate.Contracts

open System.Runtime.Serialization
open System.ServiceModel

// Note: When running serialization code in partial trust, you may need to convert 
//       this to a class with a default constructor.
[<DataContract>]
type CompositeType =
    { [<DataMember>] mutable BoolValue : bool
 
      [<DataMember>] mutable StringValue : string }

