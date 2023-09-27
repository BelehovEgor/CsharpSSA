using SSA.DotGraph;
using SSA.Parser;

const string programText = @"
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        int Main(string[] args)
        {
            var i = 123;
            var a, b = 0, c = i + 12;

            for (var k = 0; k < 21 + b; k = k + 1)
            {
                var rr = 1;
                rr = rr + 1;

                if(rr > 0)
                {
                    throw new Exception();
                } 
            }

            while (i > 100 - 11)
            {
                b = b + 2;

                if (b > 30)
                {
                    return c - 1;    
                }

                i = i - b;
            }

            if (c % 2 == 1)
            { 
                if (c % 3 == 0)
                {
                    return c - 1;    
                }
            }
            else 
            {
                b = b + 1;
                c = b * (2 - c) + 13
            }
            
            return c; 
        }
    }
}
";

var parser = new CodeParser();
var rootNode = parser.ParseMethod(
    programText,
    "HelloWorld",
    "Program",
    "Main");

var painter = new Painter();
await painter.Create(rootNode);
