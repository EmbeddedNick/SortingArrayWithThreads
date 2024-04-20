using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    class Program
    {
        class MetaInfoForSortArray 
        {
            int[,] _matrix;
            int _n, _m;
            int _firstRow;
            int _lastRow;
            public int[,] Matrix { get { return _matrix; } }
            public int N { get { return _n; } }
            public int M { get { return _m; } }
            public int FirstRow { get { return _firstRow; } }
            public int LastRow { get { return _lastRow; } }

            public MetaInfoForSortArray(int[,] matrix, int n, int m, int firstRow, int lastRow) 
            {
                _matrix = matrix;
                _n = n;
                _m = m;
                _firstRow = firstRow;
                _lastRow = lastRow;
            }
        }
        static void SortArray(object obj)
        {
            MetaInfoForSortArray sortInfo = obj as MetaInfoForSortArray;

            Console.WriteLine("Thread {0} is sorting from {1} to {2}", Threading.Thread.CurrentThread.Name, sortInfo.FirstRow, sortInfo.LastRow);

            int[] column = new int[sortInfo.M];
            for (int i = sortInfo.FirstRow; i < sortInfo.LastRow; i++)
            {
                for (int j = 0; j < sortInfo.M; j++)
                {
                    column[j] = sortInfo.Matrix[i, j];
                }
                Array.Sort(column);
                for (int j = 0; j < sortInfo.M; j++)
                {
                    sortInfo.Matrix[i, j] = column[j];
                }
            }
        }
        static void Main()
        {
            /*
            Console.WriteLine("Введите размер матрицы (кол-во строк и, затем кол-во столбцов):");
            int n = int.Parse(Console.ReadLine());
            int m = int.Parse(Console.ReadLine());
            */
            int n = 10;
            int m = 30;
            int[,] matrix = new int[n, m];
            int threadCount = 2;
            int rowsPerThread = n / threadCount;
            // create threads 
            Threading.Thread[] threads = new Threading.Thread[threadCount];
            for (int i = 0; i < threadCount; ++i)
            {
                threads[i] = new Threading.Thread(SortArray);
            }
            // Заполнение матрицы случайными числами
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = random.Next(100); // случайное число от 0 до 99
                }
            }
            Console.WriteLine("Исходная матрица:");
            PrintMatrix(matrix);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < threadCount; ++i) 
            {
                threads[i].Name = string.Format("Thread {0}", i); 
                threads[i].Start(new MetaInfoForSortArray(matrix, n, m, i * rowsPerThread, (i + 1) * rowsPerThread));
            }
            for (int i = 0; i < threadCount; ++i)
            {
                threads[i].Join();
            }
            stopwatch.Stop();

            Console.WriteLine("Отсортированная матрица:");
            PrintMatrix(matrix);

            Console.WriteLine($"Время, потраченное на сортировку: {stopwatch.Elapsed}");

            Console.ReadKey();
        }

        static void PrintMatrix(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
