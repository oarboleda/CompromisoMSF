using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;


namespace CompromisoMSF
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creación del modelo
            SolverContext context = SolverContext.GetContext();
            Model model = context.CreateModel();

            // Definición de las variables
            Decision x1 = new Decision(Domain.IntegerRange(0,450), "x1");
            Decision x2 = new Decision(Domain.IntegerRange(20,500), "x2");
            Decision x3 = new Decision(Domain.IntegerRange(30,520), "x3");
            Decision desigualdad1 = new Decision(Domain.IntegerRange(380, Rational.PositiveInfinity), "desig1");
            Decision desigualdad2 = new Decision(Domain.IntegerRange(380,480), "desig2");

            model.AddDecisions(x1, x2, x3,desigualdad1,desigualdad2);

            // Construccion de las restricciones
            model.AddConstraints("suma", x1 + x2 + x3 >= 350);
            /*
            model.AddConstraints("desigualdad1", 2*x1 + x2 == desigualdad1);
            model.AddConstraints("desigualdad2", 2*x2 + 3*x3 == desigualdad2);
            model.AddConstraints("final", desigualdad1 <= desigualdad2);
             */
            model.AddConstraints("desigualdades", 
                2 * x1 + x2 == desigualdad1,
                2 * x2 + 3 * x3 == desigualdad2,
                desigualdad1 <= desigualdad2);

            // Adiciona funcion objetivo
            model.AddGoal("cost", GoalKind.Minimize, 62*x1 - x2 + 30*x3);

            int solverDeseado;
            do
            {
                Console.Write("\nAplicativo CompromisoMSF 2013_3"+
                              "\n==============================="+
                              "\n\nOpciones: \n1. Simplex \n2. CSP\nDigite valor para el solver deseado: ");
                solverDeseado = int.Parse(Console.ReadLine());
            } while (solverDeseado < 1 || solverDeseado > 2);

            Solution solution;
            switch (solverDeseado)
            {
                case 1: solution = context.Solve(new SimplexDirective());
                    break;
                default: solution = context.Solve(new ConstraintProgrammingDirective());
                    break;
            }

            // Despliega el resultado
            Report report = solution.GetReport();
            Console.WriteLine("x1: {0}, x2: {1}, x3: {2}, d1: {3}, d2: {4}  ", x1, x2, x3, desigualdad1, desigualdad2);
            Console.Write("{0}", report);

            Console.Write("\n Pulse una tecla para finalizar ");
            Console.ReadKey(true);
        }
    }
}
