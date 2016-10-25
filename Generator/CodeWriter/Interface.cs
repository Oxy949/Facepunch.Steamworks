﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    public partial class CodeWriter
    {
        //
        // Writes the RustNative.Platform.Interface
        //
        void PlatformInterface()
        {
            StartBlock( $"internal static partial class Platform" );
            {
                StartBlock( $"public interface Interface" );
                {
                    WriteLine( "// Implementation should return true if _ptr is non null" );
                    WriteLine( "bool IsValid { get; } " );
                    WriteLine();


                    foreach ( var m in def.methods.OrderBy( x => x.ClassName ) )
                    {
                        if ( m.ClassName == "ISteamMatchmakingPingResponse" ) continue;
                        if ( m.ClassName == "ISteamMatchmakingServerListResponse" ) continue;
                        if ( m.ClassName == "ISteamMatchmakingPlayersResponse" ) continue;
                        if ( m.ClassName == "ISteamMatchmakingRulesResponse" ) continue;
                        if ( m.ClassName == "ISteamMatchmakingPingResponse" ) continue;

                        PlatformInterfaceMethod( m );
                    }

                }
                EndBlock();
            }
            EndBlock();
        }


        private void PlatformInterfaceMethod( SteamApiDefinition.MethodDef m )
        {
            var arguments = BuildArguments( m.Params );
            var ret = new Argument( "return", m.ReturnType, TypeDefs );

            var methodName = m.Name;

            if ( LastMethodName == methodName )
                methodName = methodName + "0";

            var flatName = $"SteamAPI_{m.ClassName}_{methodName}";

            if ( m.ClassName == "SteamApi" )
                flatName = methodName;

            var argstring = string.Join( ", ", arguments.Select( x => x.InteropParameter( true ) ) );
            if ( argstring != "" ) argstring = $" {argstring} ";

            WriteLine( $"{ret.Return()} {m.ClassName}_{methodName}({argstring});" );
            LastMethodName = m.Name;
        }
    }
}
