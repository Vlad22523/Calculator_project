using System;

public class CalculatorLogic
{
    public decimal FirstNumber { get; set; }
    public string Operation { get; set; } = "";
    public decimal Result { get; private set; }

    public void Calculate(decimal secondNumber)
    {
        switch (Operation)
        {
            case "+": Result = FirstNumber + secondNumber; break;
            case "-": Result = FirstNumber - secondNumber; break;
            case "×": Result = FirstNumber * secondNumber; break;
            case "÷":
                if (secondNumber == 0) throw new DivideByZeroException();
                Result = FirstNumber / secondNumber; break;
            default: throw new ArgumentException("Invalid operation");
        }
    }

    public void TrigonometricButtonPressed(string func)
    {
        double angle = double.Parse(FirstNumber.ToString());
        switch (func)
        {
            case "sin": Result = (decimal)Math.Sin(angle); break;
            case "cos": Result = (decimal)Math.Cos(angle); break;
            case "tg":
                if (Math.Cos(angle) == 0) throw new DivideByZeroException("Cannot divide by zero in tangent calculation.");
                Result = (decimal)Math.Tan(angle); break;
            case "ctg":
                if (Math.Sin(angle) == 0) throw new DivideByZeroException("Cannot divide by zero in cotangent calculation.");
                Result = (decimal)(1 / Math.Tan(angle)); break;
            default: throw new ArgumentException("Invalid trigonometric function");
        }
    }

    public void ConversionButtonPressed(string system)
    {
        int number = (int)FirstNumber;
        switch (system)
        {
            case "bin": Result = Convert.ToInt32(Convert.ToString(number, 2)); break;
            case "hex": Result = Convert.ToInt32(Convert.ToString(number, 16)); break;
            case "dec": Result = number; break;
            case "oct": Result = Convert.ToInt32(Convert.ToString(number, 8)); break;
            default: throw new ArgumentException("Invalid number system");
        }
    }

    public void Clear()
    {
        FirstNumber = 0;
        Operation = "";
        Result = 0;
    }
}