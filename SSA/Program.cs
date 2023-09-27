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

namespace HelloWorld1
{
    class Program1
    {
        static void Main(string[] args)
        {
            var i = 123;
            var a, b = 0, c = i + 12;

            a = 10;
            a = 10 + 10;

            if (a > 25)
            { 
                b = b * 2;

                return a + b;         
            }
            else
            {            
                a = 10 + 10 + 10 + 10;

                с = a * 2;
            }

            if (c % 2 == 1)
            { 
                return c - 1;         
            }
            
            return c; 

            var i = 123;
            var a, b = 0, c = i + 12;

            a = 10;
            a = 10 + 10;
            a = 10 + 10 + 10;
            a = 10 + 10 + 10 + 10;
            i = i * 2 + b;

            Console.WriteLine(""Hello, World!"");
        }
    }
}";

var parser = new CodeParser();
var rootNode = parser.ParseMethod(
    programText,
    "HelloWorld",
    "Program",
    "Main");

var painter = new Painter();
await painter.Create(rootNode);

var x = 0;