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
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Liên kết socket với địa chỉ và cổng
            listener.Bind(localEndPoint);

            // Bắt đầu lắng nghe kết nối đến (chờ client kết nối)
            listener.Listen(100);

            Console.WriteLine("Server is listening on port 8080...");

            while (true)
            {
                // Chấp nhận kết nối từ client
                Socket clientSocket = listener.Accept();

                Console.WriteLine($"Accepted a new connection from {clientSocket.RemoteEndPoint}");

                // Tạo một Thread mới để phục vụ client này
                Thread clientThread = new Thread(() =>
                {
                    string data = null;

                    while (true)
                    {
                        data = null;
                        byte[] bytes = new byte[1024];
                        int numBytes = clientSocket.Receive(bytes);

                        data += Encoding.ASCII.GetString(bytes, 0, numBytes);

                        // Khi client gửi dữ liệu là "exit" thì đóng kết nối
                        if (data == "exit")
                        {
                            Console.WriteLine($"Client {clientSocket.RemoteEndPoint} closed the connection.");
                            clientSocket.Shutdown(SocketShutdown.Both);
                            clientSocket.Close();
                            break;
                        }

                        Console.WriteLine($"Received {data} from {clientSocket.RemoteEndPoint}");

                        // Phản hồi lại client
                        byte[] message = Encoding.ASCII.GetBytes($"You said: {data}");
                        clientSocket.Send(message);
                    }
                });

                // Khởi động Thread mới
                clientThread.Start();
            }
        }
    }
}
