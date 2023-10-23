namespace Compiler;

public static class WrapperFunctions {
    public static double FloatDiv(int op1, int op2) => op1 / (double)op2;

    public static double FloatDiv(double op1, double op2) => op1 / op2;

    public static double FloatDiv(int op1, double op2) => op1 / op2;

    public static double FloatDiv(double op1, int op2) => op1 / op2;

}