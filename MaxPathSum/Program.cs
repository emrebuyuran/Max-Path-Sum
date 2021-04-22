using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace MaxPathSum
{
    public static class Program
    {
        private const string Input = @" 215
                                         193 124
                                         117 237 442
                                         218 935 347 235
                                         320 804 522 417 345
                                         229 601 723 835 133 124
                                         248 202 277 433 207 263 257
                                         359 464 504 528 516 716 871 182
                                         461 441 426 656 863 560 380 171 923
                                         381 348 573 533 447 632 387 176 975 449
                                         223 711 445 645 245 543 931 532 937 541 444
                                         330 131 333 928 377 733 017 778 839 168 197 197
                                         131 171 522 137 217 224 291 413 528 520 227 229 928
                                         223 626 034 683 839 053 627 310 713 999 629 817 410 121
                                         924 622 911 233 325 139 721 218 253 223 107 233 230 124 233";

        public static void Main(string[] args)
        {
            int[,] convertedTriangleInput = Input.ConvertTo2DArray();
            int maxSumInput = convertedTriangleInput.MoveDownwards();

            Console.WriteLine("The result for Question 4 : " + maxSumInput);
            if (maxSumInput == 0 || maxSumInput == -1)
                Console.WriteLine("There is no possible path");

            Console.WriteLine("Enter the file path: (Ex: " + @"C:\file.txt" + ")");
            try
            {
                string filePath = Console.ReadLine();
                string contents = File.ReadAllText(filePath);
                int[,] convertedTriangleFile = contents.ConvertTo2DArray();
                int maxSumFile = convertedTriangleFile.MoveDownwards();
                Console.WriteLine("The result from file : " + maxSumFile);
                Console.ReadLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Cannot find the file path, make sure you have entered the correct file path and try again");
            }
           
        }

        private static int MoveDownwards(this int[,] matrixOfTriangle)
        {
            int length = matrixOfTriangle.GetLength(0);

            int res = -1;
            for (int i = 0; i < length - 2; i++)
                res = Math.Max(res, matrixOfTriangle[0, i]);

            for (int i = 1; i < length; i++)
            {
                res = -1;
                for (int j = 0; j < length; j++)
                {
                    if (j == 0 && matrixOfTriangle[i, j] != -1)
                    {
                        if (matrixOfTriangle[i - 1, j] != -1)
                            matrixOfTriangle[i, j] += matrixOfTriangle[i - 1, j];
                        else
                            matrixOfTriangle[i, j] = -1;
                    }
                    else if (j > 0 && j < length - 1 && matrixOfTriangle[i, j] != -1)
                    {
                        int tmp = CalculateNodeValue(matrixOfTriangle[i - 1, j],
                                   matrixOfTriangle[i - 1, j - 1]);
                        if (tmp == -1)
                        {
                            matrixOfTriangle[i, j] = -1;
                        }
                        else
                            matrixOfTriangle[i, j] += tmp;
                    }

                    else if (j > 0 && matrixOfTriangle[i, j] != -1)
                    {
                        int tmp = CalculateNodeValue(matrixOfTriangle[i - 1, j],
                                         matrixOfTriangle[i - 1, j - 1]);
                        if (tmp == -1)
                        {
                            matrixOfTriangle[i, j] = -1;
                        }
                        else
                            matrixOfTriangle[i, j] += tmp;
                    }
                    else if (j != 0 && j < length - 1 && matrixOfTriangle[i, j] != -1)
                    {
                        int tmp = CalculateNodeValue(matrixOfTriangle[i - 1, j],
                                     matrixOfTriangle[i - 1, j - 1]);
                        if (tmp == -1)
                        {
                            matrixOfTriangle[i, j] = -1;
                        }
                        else
                            matrixOfTriangle[i, j] += tmp;
                    }
                    res = Math.Max(matrixOfTriangle[i, j], res);
                }
            }
            return res;
        }

        private static int[,] ConvertTo2DArray(this string input)
        {
            string[] array = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);  //editing the file, removing spaces

            int[,] matrixOfTriangle = new int[array.Length, array.Length + 1];      //converting to a matrix

            for (int row = 0; row < array.Length; row++)
            {
                int[] digitsInRow = Regex.Matches(array[row], "[0-9]+")
                    .Cast<Match>()
                    .Select(m => int.Parse(m.Value)).ToArray();

                
                for (int column = 0; column < digitsInRow.Length; column++)
                {
                    matrixOfTriangle[row, column] = digitsInRow[column];
                }
            }
            return matrixOfTriangle.ReplacePrimeNumbers();  // checks non-prime numbers and returns them from converted array
        }

        private static int CalculateNodeValue(int input1, int input2)  //returns max value
        {
            if (input1 == -1 && input2 == -1 || input1 == 0 && input2 == 0)
                return -1;
            else
                return Math.Max(input1, input2);
        }

        private static bool IsPrime(this int number)
        {
            if ((number & 1) == 0)
            {
                if (number == 2)
                {
                    return true;
                }
                return false;
            }
            for (int i = 3; (i * i) <= number; i += 2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        private static int[,] ReplacePrimeNumbers(this int[,] matrixOfTriangle)
        {
            int length = matrixOfTriangle.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (matrixOfTriangle[i, j] == 0)
                    {
                        continue;
                    }
                    else if (IsPrime(matrixOfTriangle[i, j]))       //replacing prime numbers with -1 in matrix
                    {
                        matrixOfTriangle[i, j] = -1;
                    }
                }
            }
            return matrixOfTriangle;
        }
    }
}
