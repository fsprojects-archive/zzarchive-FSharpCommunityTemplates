namespace UnitTestProject1

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type UnitTest() = 
    [<TestMethod>]
    member x.TestMethod1 () = 
        let testVal = 1
        Assert.AreEqual(1, testVal)
