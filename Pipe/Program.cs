using System;
using System.IO;
using System.IO.Pipes;
using System.Diagnostics;

class PipeServer
{
    static void Main()
    {
        // Khởi tạo 1 tiến trình con làm máy khách
        Process pipeClient = new Process();
        // Chỉ định tiến trình sẽ chạy chương trình gì
        pipeClient.StartInfo.FileName = "D:\\Laptrinh\\ProjectC#\\khach\\bin\\Debug\\net6.0\\khach.exe";
        // Tạo Stream Xuất để giao tiếp với tiến trình con (máy khách)
        using (AnonymousPipeServerStream pipeServer =
            new AnonymousPipeServerStream(PipeDirection.Out,
            HandleInheritability.Inheritable))
        {
            Console.WriteLine("[SERVER] Current TransmissionMode: {0}.",
                pipeServer.TransmissionMode);
                // lấy Handl của Stream đưa cho máy khachs
            pipeClient.StartInfo.Arguments =
                pipeServer.GetClientHandleAsString();
            pipeClient.StartInfo.UseShellExecute = false;
            pipeClient.Start();
            // Xoá bản sao Handl máy khachs
            pipeServer.DisposeLocalCopyOfClientHandle();
            try
            {
                // Bắt đầu tạo Stream ghi với nguồn là PipeSever
                using (StreamWriter sw = new StreamWriter(pipeServer))
                {
                    // tắt chế độ tự động đẩy
                    sw.AutoFlush = true;
                    sw.WriteLine("OK");
                    while (true)
                    {
                        // đợi máy khách nhận hết thông điệp
                        pipeServer.WaitForPipeDrain();
                        Console.Write("[SERVER] Enter text: ");
                        sw.WriteLine(Console.ReadLine());
                        // Luồng ngủ
                        Thread.Sleep(300);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine("[SERVER] Error: {0}", e.Message);
            }
        }
        pipeClient.WaitForExit();
        pipeClient.Close();
        Console.WriteLine("[SERVER] Client quit. Server terminating.");
    }
}