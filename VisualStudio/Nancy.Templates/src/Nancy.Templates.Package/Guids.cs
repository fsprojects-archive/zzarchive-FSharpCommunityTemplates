// Guids.cs
// MUST match guids.h
using System;

namespace fsharporg.Nancy_Templates_Package
{
    static class GuidList
    {
        public const string guidNancy_Templates_PackagePkgString = "74726a1d-1ee2-4bb0-8e59-8657158a9434";
        public const string guidNancy_Templates_PackageCmdSetString = "20ba1c7f-32ac-4781-a4a3-4cc079924510";

        public static readonly Guid guidNancy_Templates_PackageCmdSet = new Guid(guidNancy_Templates_PackageCmdSetString);
    };
}