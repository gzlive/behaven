// PkgCmdID.cs
// MUST match PkgCmdID.h
using System;

namespace BehaveN.BehaveN_Extensions
{
    static class PkgCmdIDList
    {
        public const uint cmdidFindStepDefinitionCommand = 0x110;
        public const uint cmdidFindStepsCommand = 0x120;

        public const uint cmdidReplaceHumpsWithSpacesCommand = 0x210;
        public const uint cmdidReplaceHumpsWithUnderscoresCommand = 0x0220;
        public const uint cmdidReplaceSpacesWithHumpsCommand = 0x0230;
        public const uint cmdidReplaceSpacesWithUnderscoresCommand = 0x0240;
        public const uint cmdidReplaceUnderscoresWithHumpsCommand = 0x0250;
        public const uint cmdidReplaceUnderscoresWithSpacesCommand = 0x0260;
    };
}