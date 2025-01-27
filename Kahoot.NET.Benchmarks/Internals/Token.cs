﻿using Kahoot.NET.Internal.Token;
using Kahoot.NET.Internal.Token.Processing;

namespace Kahoot.NET.Benchmarks.Internals;


[MemoryDiagnoser]
public class TokenBenchmarks
{
    const string HeaderToken = "RzluUWgBCRRmB387agoJUzh8BQdjQyBWTG9qZ1oNK0xnE18BUH96DnVabX4ECWEtUjYjZ0ELB3xyAmtXUXhWRThQJhJpeRZiJ2wnYVxIXEImNTdURngEYykIUHRbWAhw";
    const string ChallengeToken = "decode.call(this, 'U8jRopLnfOpDeGDuB4pZcMulWCg81EDz41O0tgN6a8fw6Rs2HN26VVIreTJjLluMrNfMXHykefBZ8NGMYEf1MoLRJsrfv06mLYj3'); function decode(message) {var offset = 93 \t +\t 71   *83 \t +\t 84; if( this   .angular\t .\t isArray\t ( \t offset)) \t console .\t log \t (\"Offset derived as: {\", offset, \"}\"); return    _ \t . \t replace ( message,/./g, function(char, position) {return String.fromCharCode((((char.charCodeAt(0)*position)+ offset ) % 77) + 48);});}";


    [Benchmark]
    public void CreateToken()
    {
        string token = Merger.Create(Header.CreateHeaderToken(HeaderToken), Challenge.CreateToken(ChallengeToken));
    }

}
