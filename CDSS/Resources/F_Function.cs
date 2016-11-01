using System;

public class F_Function
{
    private const double SQTPI = 2.50662827463100050242E0;

    // Returns the gamma function of the specified number.
    public static double gamma(double x)
    {
        double[] P = {
						 1.60119522476751861407E-4,
						 1.19135147006586384913E-3,
						 1.04213797561761569935E-2,
						 4.76367800457137231464E-2,
						 2.07448227648435975150E-1,
						 4.94214826801497100753E-1,
						 9.99999999999999996796E-1
					 };
        double[] Q = {
						 -2.31581873324120129819E-5,
						 5.39605580493303397842E-4,
						 -4.45641913851797240494E-3,
						 1.18139785222060435552E-2,
						 3.58236398605498653373E-2,
						 -2.34591795718243348568E-1,
						 7.14304917030273074085E-2,
						 1.00000000000000000320E0
					 };

        double p, z;

        double q = Math.Abs(x);

        if (q > 33.0)
        {
            if (x < 0.0)
            {
                p = Math.Floor(q);
                if (p == q) throw new ArithmeticException("gamma: overflow");
                //int i = (int)p;
                z = q - p;
                if (z > 0.5)
                {
                    p += 1.0;
                    z = q - p;
                }
                z = q * Math.Sin(Math.PI * z);
                if (z == 0.0) throw new ArithmeticException("gamma: overflow");
                z = Math.Abs(z);
                z = Math.PI / (z * stirf(q));

                return -z;
            }
            else
            {
                return stirf(x);
            }
        }

        z = 1.0;
        while (x >= 3.0)
        {
            x -= 1.0;
            z *= x;
        }

        while (x < 0.0)
        {
            if (x == 0.0)
            {
                throw new ArithmeticException("gamma: singular");
            }
            else if (x > -1.0E-9)
            {
                return (z / ((1.0 + 0.5772156649015329 * x) * x));
            }
            z /= x;
            x += 1.0;
        }

        while (x < 2.0)
        {
            if (x == 0.0)
            {
                throw new ArithmeticException("gamma: singular");
            }
            else if (x < 1.0E-9)
            {
                return (z / ((1.0 + 0.5772156649015329 * x) * x));
            }
            z /= x;
            x += 1.0;
        }

        if ((x == 2.0) || (x == 3.0)) return z;

        x -= 2.0;
        p = polevl(x, P, 6);
        q = polevl(x, Q, 7);
        return z * p / q;

    }


    // Return the gamma function computed by Stirling's formula.
    private static double stirf(double x)
    {
        double[] STIR = {
							7.87311395793093628397E-4,
							-2.29549961613378126380E-4,
							-2.68132617805781232825E-3,
							3.47222221605458667310E-3,
							8.33333333333482257126E-2,
		};
        double MAXSTIR = 143.01608;

        double w = 1.0 / x;
        double y = Math.Exp(x);

        w = 1.0 + w * polevl(w, STIR, 4);

        if (x > MAXSTIR)
        {
            /* Avoid overflow in Math.Pow() */
            double v = Math.Pow(x, 0.5 * x - 0.25);
            y = v * (v / y);
        }
        else
        {
            y = Math.Pow(x, x - 0.5) / y;
        }
        y = SQTPI * y * w;
        return y;
    }


    // Evaluates polynomial of degree N
    private static double polevl(double x, double[] coef, int N)
    {
        double ans;

        ans = coef[0];

        for (int i = 1; i <= N; i++)
        {
            ans = ans * x + coef[i];
        }

        return ans;
    }

    //Return F distribution 
    //F number:x 
    //The first degree of freedom: a
    //the sencond degree of freedom: b
    public static double F_dist(double x, double a, double b)
    {
        double result;
        double num1;
        double num2;
        double num3;
        num1 = gamma((a + b) / 2.0) / (gamma(a / 2.0) * gamma(b / 2.0));
        num2 = Math.Pow(b, b / 2.0) * Math.Pow(a, a / 2);
        num3 = Math.Pow(x, a / 2.0 - 1.0) / Math.Pow(a * x + b, (a + b) / 2.0);
        result = num1 * num2 * num3;
        return result;
    }
    //Return F distribution with Decimal precision
    //F number:x 
    //The first degree of freedom: a
    //the sencond degree of freedom: b
    //Decimal precision: c
    public static double F_dist(double x, double a, double b,int c)
    {
        double result;
        double temp;
        double num1;
        double num2;
        double num3;
        num1 = gamma((a + b) / 2.0) / (gamma(a / 2.0) * gamma(b / 2.0));
        num2 = Math.Pow(b, b / 2.0) * Math.Pow(a, a / 2);
        num3 = Math.Pow(x, a / 2.0 - 1.0) / Math.Pow(a * x + b, (a + b) / 2.0);
        result = num1 * num2 * num3;
        result = Math.Round(result, c);
        return result;
    }

}
