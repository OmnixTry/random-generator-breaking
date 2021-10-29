using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	class BetResponse
	{
		public string Message { get; set; }

		public Account Account { get; set; }

		public int RealNumber { get; set; }

		public void Print()
		{
			Console.WriteLine("---------");
			Console.WriteLine($"Message: {Message}; RealNumber: {RealNumber};");
			//Account.Print();
			Console.WriteLine("---------");
		}
	}
}
