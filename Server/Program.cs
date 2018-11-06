using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Server {
	internal class Program {
		private static void Main(string[] args) {
			try {
				Int32 port = 13000;
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				TcpListener server = new TcpListener(localAddr, port);
				server.Start();

				while (true) {
					TcpClient client = server.AcceptTcpClient();
					NetworkStream stream = client.GetStream();

					Byte[] bytes = new Byte[256];
					int i = stream.Read(bytes, 0, bytes.Length);
					string data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
					string result = Calculate(data);
					byte[] res = System.Text.Encoding.ASCII.GetBytes(result);
					stream.Write(res, 0, res.Length);
					client.Close();
				}
			}
			catch (SocketException e) {
				Console.WriteLine("SocketException: {0}", e.Message);
			}
			catch (ArgumentNullException e) {
				Console.WriteLine("ArgumentNullException: {0}", e.Message);
			}
		}

		private const string Delimiter = " ";

		public static string Calculate(string input) {
			string output = GetExpression(input);
			double result = Counting(output);
			return result.ToString();
		}

		private static string GetExpression(string input) {
			string output = string.Empty;
			var operStack = new Stack<char>();

			for (var i = 0; i < input.Length; i++) {
				if (IsDelimeter(input[i])) continue;

				if (IsDigit(input[i])) {
					for (; i < input.Length; i++) {
						if (IsDelimeter(input[i]) || IsOperator(input[i])) break;
						output += input[i].ToString();
					}

					output += Delimiter;
					i--;
				}

				if (!IsOperator(input[i])) continue;

				if (input[i] == '(')
					operStack.Push(input[i]);
				else {
					if (input[i] == ')') {
						char c = operStack.Pop();

						while (c != '(') {
							output += c.ToString() + Delimiter;
							c = operStack.Pop();
						}
					}
					else {
						if (operStack.Count > 0)
							if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
								output += operStack.Pop() + Delimiter;

						operStack.Push(input[i]);
					}
				}
			}

			while (operStack.Count > 0)
				output += operStack.Pop().ToString() + Delimiter;

			return output;
		}

		private static double Counting(string input) {
			double result = 0;
			var temp = new Stack<double>();

			for (var i = 0; i < input.Length; i++) {
				if (IsDigit(input[i])) {
					string toNubmer = string.Empty;

					for (; i < input.Length; i++) {
						if (IsDelimeter(input[i]) || IsOperator(input[i])) break;
						toNubmer += input[i];
					}

					temp.Push(double.Parse(toNubmer));
					i--;
				}
				else if (IsOperator(input[i])) {
					double a = temp.Pop();
					double b = temp.Pop();
					switch (input[i]) {
						case '+':
							result = b + a;
							break;
						case '-':
							result = b - a;
							break;
						case '*':
							result = b * a;
							break;
						case '/':
							result = b / a;
							break;
					}

					temp.Push(result);
				}
			}

			return temp.Peek();
		}

		public static bool IsOperator(char с) {
			return "+-/*()".IndexOf(с) != -1;
		}

		public static byte GetPriority(char c) {
			switch (c) {
				case '(':
				case ')':
					return 0;
				case '+':
				case '-':
					return 1;
				case '*':
				case '/':
					return 2;
				default:
					return 3;
			}
		}

		public static bool IsDigit(char c) {
			return char.IsDigit(c);
		}

		public static bool IsDelimeter(char c) {
			return c == ' ';
		}
	}
}