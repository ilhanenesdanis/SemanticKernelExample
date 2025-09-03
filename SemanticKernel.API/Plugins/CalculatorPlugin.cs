using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace SemanticKernel.API.Plugins;

public sealed class CalculatorPlugin
{
    [KernelFunction("add")]
    [Description("iki sayısal değerin toplama işlemi gerçekleştirilir")]
    [return: Description("Toplam değerleri döndürür")]
    public int Add(int number1, int number2)
    {
        return number1 + number2;
    }
    [KernelFunction(nameof(CalculatorPlugin.Subtract))]
    [Description("iki sayısal değerin çıkarma işlemini gerçekleştirir")]
    [return: Description("Çıkarma işleminin sonucunu döndürür")]
    public int Subtract(int number1, int number2)
    {
        return number1 - number2;
    }
    [KernelFunction(nameof(CalculatorPlugin.Multiply))]
    [Description("iki sayısal değerin çarpımını yapar")]
    [return: Description("çarpma işleminin sonucunu döndürür")]
    public int Multiply(int number1, int number2)
    {
        return number1 * number2;
    }
    [KernelFunction(nameof(CalculatorPlugin.Divide))]
    [Description("iki sayısal değerin bölme işlemini yapar")]
    [return : Description("bölme işlemi sonucunu döndürür")]
    public int Divide(int number1, int number2)
    {
        return number2 != 0 ? number1 / number2 : 0;
    }
}
