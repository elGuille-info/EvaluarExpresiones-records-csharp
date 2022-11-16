using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using java.lang;

//using StringJava = java.lang.String;

namespace EvaluarExpresiones;

// Evaluar expresiones simples usando los record definidos en SealedRecord.
// Se permite *, /, +, - con este nivel de precedencia.
// Las expresiones entre paréntesis se evalúan primero.
//
//  op1 + op2 * op3 se evalúa como op1 + (op2*op3)
//  op1 * op2 + op3 se evalúa como (op1*op2) + op3

//package com.example.evaluar;

public class Evaluar
{
    public static int evalua(String expresion) //throws Exception
    {
        if (expresion == null || expresion.Trim().Equals(""))
            return -1;
        // Quitar todos los caracteres en blanco.
        expresion = expresion.Replace(" ", "");


        // Evaluar todas las expresiones entre paréntesis.
        var res = evaluaParentesis(expresion);

        int resultado;
        // Si hay operadores, evaluarlo.
        if (hayOperador(res))
        {
            resultado = evaluar(res);
        }
        else
        {
            // Si no hay operadores, es el resultado.
            resultado = int.Parse(res);
        }

        return resultado;
    }

    private static String evaluaParentesis(String expresion) //throws Exception
    {
        bool hay = false;
        do
        {
            int ini = expresion.IndexOf('(');
            if (ini > -1)
            {
                int fin = expresion.IndexOf(')', ini);
                if (fin > -1)
                {
                    // Comprobar si hay otro de empiece antes del cierre
                    var ini2 = expresion.IndexOf('(', ini + 1);
                    if (ini2 > -1 && ini2 < fin)
                    {
                        //System.err.println(ini2);
                        ini = ini2;
                    }
                    // En .NET es posInicio, longitud
                    // En java es posinicio, posFinExclusiva
                    var exp = expresion.Substring(ini + 1, fin - ini - 1);
                    int res = evaluar(exp);
                    expresion = expresion.Replace("(" + exp + ")", res.ToString());
                }
            }
            // Si no hay más paréntesis, salir.
            // Por seguridad, comprobar que haya los dos paréntesis
            //if (expresion.indexOf('(') == -1){
            // Si hay de apertura y cierre, continuar.
            if (expresion.IndexOf('(') > -1 && expresion.IndexOf(')') > -1)
            {
                hay = true;
            }
            else
            {
                // Quitar los que hubiera (por si no están emparejados)
                if (expresion.IndexOf('(') > -1 || expresion.IndexOf(')') > -1)
                {
                    var col = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Los paréntesis no están emparejados:\n    " + expresion);
                    Console.ForegroundColor = col;
                    expresion = expresion.Replace("(", "").Replace(")", "");
                }
                hay = false;
            }
        } while (hay);

        return expresion;
    }

    private static int evaluar(String expresion) //throws Exception
    {
        if (expresion == null || expresion.Trim().Equals(""))
            return 0;

        // Quitar todos los caracteres en blanco.
        expresion = expresion.Replace(" ", "");

        // Evaluar la expresión indicada.

        String op1, op2;
        int posicion;
        char signo;
        int resultado = 0;

        do
        {
            // Buscar la operación a realizar.
            signo = buscarSiguienteSigno(expresion);
            // Si no hay más operaciones, salir.
            if (signo == '\u0000') { break; }
            // Posición del signo en la cadena a evaluar.
            posicion = posSigno(expresion, signo);
            // Si no se ha hallado, salir (esto no debería ocurrir)
            if (posicion == -1)
            {
                break;
            }

            ConstantExpr res1, res2;

            // Si la posición es cero es que delante no hay nada
            if (posicion == 0)
            {
                Console.WriteLine(posicion);
                throw new Exception("Esto no debe ocurrir.");
            }

            // Asignar todos los caracteres hasta el signo al primer operador
            op1 = expresion.Substring(0, posicion).Trim();
            var op11 = buscarNumeroAnterior(op1);
            op1 = op11;
            res1 = new ConstantExpr(int.Parse(op1));

            op2 = expresion.Substring(posicion + 1).Trim();
            var op22 = buscarNumeroSiguiente(op2);
            op2 = op22;
            res2 = new ConstantExpr(int.Parse(op2));

            resultado = signo switch
            {
                '+' => new PlusExpr(res1, res2).eval(),
                '-' => new MinusExpr(res1, res2).eval(),
                '*' => new TimesExpr(res1, res2).eval(),
                '/' => new DivideExpr(res1, res2).eval(),
                _ => 0
            };
            var operacion = op1 + signo + op2;
            var elResultado = resultado.ToString();
            expresion = expresion.Replace(operacion, elResultado);
        } while (hayOperador(expresion));

        return resultado;
    }


    /**
     * Comprueba si la cadena indicada tiene alguno de los operadores aceptados.
     * @param expresion La cadena a comprobar.
     * @return True si contiene algún operador, false en caso contrario.
     */
    static bool hayOperador(String expresion)
    {
        foreach (char c in signos)
        {
            int p = expresion.IndexOf(c);
            if (p > -1)
            {
                return true;
            }
        }
        return false;
    }

    /**
     * Devuelve la posición del carácter indicado en la cadena.
     * @param expresion La cadena a comprobar.
     * @param signo El caracter a buscar.
     * @return La posición o -1 si no se ha encontrado.
     */
    static int posSigno(String expresion, char signo)
    {
        var pos = expresion.IndexOf(signo);
        return pos;
    }

    /**
     * Los operadores en el orden de precedencia.
     */
    //static readonly String operadores = "()*/+-";
    static readonly String operadores = "*/+-";

    /**
     * Array de tipo char con los operadores.
     */
    static readonly char[] signos = operadores.ToCharArray();

    /**
     * Busca el siguiente signo de operación, con este orden: * / + -
     * @param expresion La expresión a evaluar.
     * @return El signo hallado o 0 ('\u0000') si no se halla.
     */
    static char buscarSiguienteSigno(String expresion)
    {
        //var signos = operadores.toCharArray();
        for (int i = 0; i < signos.Length; i++)
        {
            int p = expresion.IndexOf(signos[i]);
            if (p > -1)
            {
                return signos[i];
            }
        }
        return '\u0000';
    }

    /**
     * Busca un número desde el final hasta el principio (o si encuentra un signo de operación)
     * @param expresion La expresión a evaluar.
     * @return Una cadena con el número hallado.
     */
    static String buscarNumeroAnterior(String expresion)
    {
        java.lang.StringBuilder sb = new();

        var a = expresion.ToCharArray();
        int inicio = a.Length - 1;
        int fin = 0;

        for (int i = inicio; i >= fin; i--)
        {
            if (operadores.IndexOf(a[i]) == -1)
            {
                sb.append(a[i]);
            }
            else
            {
                break;
            }
        }
        if (sb.length() > 1)
        {
            sb.reverse();
        }
        return sb.toString().Trim();
    }


    /**
     * Busca un número desde el principio hasta el final (o si encuentra un signo de operación)
     * @param expresion La expresión a evaluar.
     * @return Una cadena con el número hallado.
     */
    static String buscarNumeroSiguiente(String expresion)
    {
        System.Text.StringBuilder sb = new();

        var a = expresion.ToCharArray();
        int inicio = 0;
        int fin = a.Length - 1;
        for (int i = inicio; i <= fin; i++)
        {
            if (operadores.IndexOf(a[i]) == -1)
            {
                sb.Append(a[i]);
            }
            else
            {
                break;
            }
        }
        return sb.ToString().Trim();
    }
}
