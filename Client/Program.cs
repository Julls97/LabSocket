using System;
using System.Net;
using System.Net.Sockets;

namespace Client {
	class Program {
		static void Main(string[] args) {
			try {
				Int32 port = 13000;
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				TcpClient client = new TcpClient(localAddr.ToString(), port);

				string message = Console.ReadLine();
				Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

				NetworkStream stream = client.GetStream();
				stream.Write(data, 0, data.Length);

				Int32 bytes = stream.Read(data, 0, data.Length);
				string responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				Console.WriteLine(responseData);
				
				stream.Close();
				client.Close();
			}
			catch (ArgumentNullException e)
			{
				Console.WriteLine("ArgumentNullException: {0}", e);
			}
			catch (SocketException e)
			{
				Console.WriteLine("SocketException: {0}", e);
			}
		}
	}
}