using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tạo kết nối tới Server
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, 8080);
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Kết nối tới Server
                clientSocket.Connect(remoteEndPoint);
                Console.WriteLine($"Connected to {remoteEndPoint}");

                while (true)
                {
                    // Nhập dữ liệu từ CLI
                    Console.Write("Enter a message: ");
                    string message = Console.ReadLine();

                    // Gửi dữ liệu tới Server
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    clientSocket.Send(bytes);

                    // Nhận phản hồi từ Server
                    bytes = new byte[1024];
                    int numBytes = clientSocket.Receive(bytes);
                    string data = Encoding.ASCII.GetString(bytes, 0, numBytes);
                    Console.WriteLine($"Received: {data}");

                    // Nếu dữ liệu gửi đi là "exit" thì đóng kết nối
                    if (message == "exit")
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        clientSocket.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
