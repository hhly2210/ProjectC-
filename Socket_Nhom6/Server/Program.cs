using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tạo địa chỉ IP cho Server
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            // Tạo đối tượng kết nối (EndPoint)
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);

            // Tạo socket
            Socket serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Liên kết socket với địa chỉ và cổng
            serverSocket.Bind(localEndPoint);

            // Bắt đầu chờ kết nối từ các client
            serverSocket.Listen(10); // chỉ cho phép tối đa là 10 kết nối đồng thời

            Console.WriteLine("Server dc mo tren port 8080...");

            while (true)
            {
                // Chấp nhận kết nối từ client
                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine($"1 ket noi moi dc duyet tu dia chi {clientSocket.RemoteEndPoint}");

                // Tạo một Thread mới để phục vụ client này
                Thread clientThread = new Thread(() =>
                {
                    string data = null;

                    while (true)
                    {
                        // Nhận dữ liệu từ Client
                        data = null;
                        byte[] bytes = new byte[1024];
                        int numBytes = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numBytes);
                        
                        // Khi client gửi dữ liệu là "exit" thì đóng kết nối
                        if (data == "exit")
                        {
                            Console.WriteLine($"Client {clientSocket.RemoteEndPoint} dong ket noi.");
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                            break;
                        }

                        Console.WriteLine($"[Client]({clientSocket.RemoteEndPoint}): {data}");

                        // Phản hồi lại client
                        byte[] message = Encoding.ASCII.GetBytes($"{data}");
                        clientSocket.Send(message);
                        Console.Write("Nhap tin nhan: ");
                        string sendToClient = Console.ReadLine();
                        byte[] messageToClient = Encoding.ASCII.GetBytes($"{sendToClient}");
                        clientSocket.Send(messageToClient);
                    }
                });

                // Khởi động Thread mới
                clientThread.Start();
            }
        }
    }
}
