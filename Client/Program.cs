using System;
using System.Net;
using System.Net.Sockets;

namespace Client {
	class Program {
		static void Main(string[] args) {
			IPAddress localAddr = IPAddress.Parse("127.0.0.1");

			while (true) {
				string message = Console.ReadLine();
				if (message == ".") {
					break;
				}
				Connect(localAddr.ToString(), message);				
			}
		}

		static void Connect(String server, String message) {
			try {
			//	string message = Console.ReadLine();
				
				Int32 port = 13000;
			//	IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				TcpClient client = new TcpClient(server, port);

				Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
				NetworkStream stream = client.GetStream();
				stream.Write(data, 0, data.Length);

				string responseData = string.Empty;
				while (!responseData.EndsWith(";")) {
					Int32 bytes = stream.Read(data, 0, data.Length);
					responseData += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
				}

				responseData = responseData.Substring(0, responseData.Length-1);
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
				Console.WriteLine("Lost connection.");

				//Console.WriteLine("SocketException: {0}", e);
			}
		}
	}
}

// (((9*8+3/2)-(8+7-6-1)+(-1))*2)

// (73.5 - 8 + -1) * 2

// 10/0
// -3-2-1