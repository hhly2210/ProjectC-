using System;
using System.IO;
using System.IO.Pipes;

class PipeClient
{
    static void Main(string[] args)
    {
        // nếu tiến trình có tham số 
        if (args.Length > 0)
        {
            // khởi tạo pipestream đọc dựa trên tham số vào
            using (PipeStream pipeClient =
                new AnonymousPipeClientStream(PipeDirection.In, args[0]))
            {
                Console.WriteLine("[CLIENT] Current TransmissionMode: {0}.",
                   pipeClient.TransmissionMode);
                //    Tạo 1 Stream đọc từ pipe kHach
                using (StreamReader sr = new StreamReader(pipeClient))
                {
                    string temp;
                    do
                    {
                        Console.WriteLine("[CLIENT] Wait for sync...");
                        temp = sr.ReadLine();
                    }
                    while (!temp.StartsWith("OK"));
                    do
                    {
                        // tạo task đa luồng đợi thông điệp từ máy chủ
                        var task = sr.ReadLineAsync();
                        task.Wait();
                        temp = task.Result;
                        Console.WriteLine("[CLIENT] Echo: " + temp);
                    } while (true);
                }
            }
        }
        Console.Write("[CLIENT] Press Enter to continue...");
        Console.ReadLine();
    }
}