using SSA.CfgParser;
using SSA.SsaParser;
using SSA.Visualisation;

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
                   k = 1 + k;
                   if (rr < 10)
                   {
                       throw new Exception();
                   }
                   else
                   {
                       rr = k + rr * 2;
                       k = rr + 1;
                   }
               }
           }


           while (i > 100 - 11)
           {
               b = b + 2;


               if (b > 30)
               {
                   continue;
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
var cfgRootNode = parser.ParseMethod(
    programText,
    "HelloWorld",
    "Program",
    "Main");

var cfgPainter = new CfgPainter();
await cfgPainter.Create(cfgRootNode);


var ssaFromCfgCreator = new SsaFromCfgConverter();
var ssaGraph = ssaFromCfgCreator.CreateGraph(cfgRootNode);

var ssaPainter = new SsaPainter();
await ssaPainter.Create(ssaGraph);
