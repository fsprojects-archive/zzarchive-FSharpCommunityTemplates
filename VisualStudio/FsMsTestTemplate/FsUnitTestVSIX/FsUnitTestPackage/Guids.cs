// Guids.cs
// MUST match guids.h
using System;

namespace fsharporg.FsUnitTestPackage
{
    static class GuidList
    {
        public const string guidFsUnitTestPackagePkgString = "63b09e22-4f58-4fdc-8130-bf6d5ed9183c";
        public const string guidFsUnitTestPackageCmdSetString = "53ac59c2-85e7-429a-b932-678040d13a5e";

        public static readonly Guid guidFsUnitTestPackageCmdSet = new Guid(guidFsUnitTestPackageCmdSetString);
    };
}