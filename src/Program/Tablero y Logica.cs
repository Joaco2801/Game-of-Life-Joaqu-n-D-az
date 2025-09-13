using System;
using System.IO;
using System.Threading;

namespace GameOfLifeSimple
{
    public static class BoardLoader  // lee el archivo para hacer el tablero
    {
        public static bool[,] LoadFromFile(string filePath)
        {
            // revisamos todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);
            int altura = lines.Length; 
            int andcho = lines[0].Length;
            bool[,] board = new bool[andcho, altura];
            // lee el archivo para poder identificar si hay una celula o no dependiendo si es 1 o 0
            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < andcho; x++)
                {
                    board[x, y] = lines[y][x] == '1';
                }
            }

            return board;
        }
    }
    public static class BoardPrinter //imprime el tablero
    {
        public static void Print(bool[,] board)
        {
            int width = board.GetLength(0);
            int height = board.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(board[x, y] ? "X" : ".");
                }
                Console.WriteLine();
            }
        }
    }
    public static class Logicadeljuego
    {
        public static void UpdateBoard(bool[,] board)
        {
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            //copia para el nuevo estado
            bool[,] temp = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int aliveNeighbors = 0;
                    
                    for (int i = x - 1; i <= x + 1; i++)
                    {
                        for (int j = y - 1; j <= y + 1; j++)
                        {
                            if (i >= 0 && i < width && j >= 0 && j < height)
                            {
                                if (board[i, j])
                                    aliveNeighbors++;
                            }
                        }
                    }
                    if (board[x, y])
                        aliveNeighbors--;
                    if (board[x, y] && (aliveNeighbors == 2 || aliveNeighbors == 3))
                        temp[x, y] = true;
                    else if (!board[x, y] && aliveNeighbors == 3)
                        temp[x, y] = true;
                    else
                        temp[x, y] = false;
                }
            }
            // Copiamos los resultados al tablero original así va a lo siguiente
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    board[x, y] = temp[x, y];
                }
            }
        }
    }
    public class GameBoard //maneja el tablero
    {
        private bool[,] board;

        public GameBoard(bool[,] initialBoard)
        {
            board = initialBoard;
        }

        public void Run()
        {
            Console.CursorVisible = false;

            while (true)
            {
                Console.SetCursorPosition(0, 0);
                BoardPrinter.Print(board);
                Logicadeljuego.UpdateBoard(board);
                Thread.Sleep(500);
            }
        }
    }
    
    class Program // inicia el programa
    {
        static void Main()
        {
            bool[,] tablero = BoardLoader.LoadFromFile("board.txt");
            GameBoard game = new GameBoard(tablero);
            game.Run();
        }
    }
}