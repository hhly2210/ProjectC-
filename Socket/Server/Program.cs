﻿using System.Net;
using System.Net.Sockets;
using System.Text;

IPAddress ip =  new(new byte[] {127,0,0,1});

using Socket listener = new(
    ip.AddressFamily,
    SocketType.Stream,
    ProtocolType.Tcp);

listener.Bind(new  IPEndPoint(ip, 11000));
listener.Listen(100);

var handler = await listener.AcceptAsync();
while (true)
{
    // Receive message.
    var buffer = new byte[1_024];
    var received = await handler.ReceiveAsync(buffer, SocketFlags.None);

    var response = Encoding.UTF8.GetString(buffer, 0, received);
    
    var eom = "<|EOM|>";
    if (response.IndexOf(eom) > -1 /* is end of message */)
    {
        Console.WriteLine(
            $"Socket server received message: \"{response.Replace(eom, "")}\"");

        var ackMessage = "<|ACK|>";
        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
        await handler.SendAsync(echoBytes, 0);
        Console.WriteLine(
            $"Socket server sent acknowledgment: \"{ackMessage}\"");

        break;
    }
    // Sample output:
    //    Socket server received message: "Hi friends 👋!"
    //    Socket server sent acknowledgment: "<|ACK|>"
}