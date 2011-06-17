// Guids.cs
// MUST match guids.h
using System;

namespace BehaveN.BehaveN_Extensions
{
    static class GuidList
    {
        public const string guidBehaveN_ExtensionsPkgString = "df42ef71-3b83-4ff6-9aec-a02a89082dbd";
        public const string guidBehaveN_ExtensionsCmdSetString = "eaee4941-571a-44d4-bdf8-dfb20c5ab9d9";

        public static readonly Guid guidBehaveN_ExtensionsCmdSet = new Guid(guidBehaveN_ExtensionsCmdSetString);
    };
}