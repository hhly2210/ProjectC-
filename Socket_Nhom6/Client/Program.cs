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
            // Tạo Socket cho Client
            Socket clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Kết nối tới Server
                clientSocket.Connect(remoteEndPoint);
                Console.WriteLine($"Ket noi toi {remoteEndPoint}");

                while (true)
                {
                    // Nhập dữ liệu từ CLI
                    Console.Write("Nhap tin nhan: ");
                    string message = Console.ReadLine();

                    // Gửi dữ liệu tới Server
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    clientSocket.Send(bytes);

                    // Nhận phản hồi từ Server
                    // Hiện câu trả lời đã gửi
                    bytes = new byte[1024];
                    int numBytes = clientSocket.Receive(bytes);
                    string data = Encoding.ASCII.GetString(bytes, 0, numBytes);
                    Console.WriteLine($"Da gui: {data}");
                    // Nhận tin nhắn từ Server
                    byte[] messageToClient = new byte[1024];
                    int num_messageToClient = clientSocket.Receive(messageToClient);
                    string sendToClient = Encoding.ASCII.GetString(messageToClient, 0, num_messageToClient);
                    Console.WriteLine($"[SERVER]: {sendToClient}");

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
