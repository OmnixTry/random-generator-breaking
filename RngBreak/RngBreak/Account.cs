using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	class Account
	{
		public string Id { get; set; }

		public int Money { get; set; }

		public DateTime DeletionTime { get; set; }

		public void Print()
		{
			Console.WriteLine("=============");
			Console.WriteLine($"Id: {Id}; Money: {Money}; Deletion Time: {DeletionTime}");
			Console.WriteLine("=============\n");
		}
	}
}
