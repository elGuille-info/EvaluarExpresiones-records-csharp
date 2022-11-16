using System;

namespace EvaluarExpresiones;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Evaluar expresiones.");

        String expresion;
        int res;

        expresion = "((1+2)*(5+3+4)";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "(1+2)*5+3+4";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "((1+2)*5+3+4)";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "(1+2)*(5+3)+4";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "1+2*5+3+4";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "1 + 2 + 3 + 4*5";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

        expresion = "2*5 + 3*2 + 3 + 4*5 + 3*2";
        res = Evaluar.evalua(expresion);
        Console.WriteLine(expresion + " = " + res);

    }
}