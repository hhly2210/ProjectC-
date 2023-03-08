using System;
namespace InXY
{
    class Program
    {
        public static void Main()
        {
            Thread t = new Thread(WriteY);
            t.Start();
            for(int i = 0; i < 500; i++)
            {
                Console.Write("x");
            }
            Console.ReadKey();
        }
        static void WriteY()
        {
            for(int i = 0; i < 500; i++)
            {
                Console.Write("y");
            }
        }
    }
}
