using System;
using System.Threading.Tasks;

namespace RngBreak
{
	class Program
	{
		static async Task Main(string[] args)
		{
			const string accId = "552261";
			CasinoCaller caller = new CasinoCaller();
			var acc = await caller.CreateAccount(accId);
			var response = await caller.MakeABet(1, 1, GameModes.LinearCongruential, accId);
			response.Print();
			

			Console.WriteLine(DateTime.Now);
			acc.Print();
			
			//var response = await caller.MakeABet(1, 1, GameModes.LinearCongruential, accId);
			response.Print();
		}
	}
}

// 5 % 2 = 3			5 % 3 = 2
// 10 % 8 = 2			10 % 2 = 8
// 8 % 10 = 8


// X2 = (aX1 + c) % M  X2 % M = (aX1 + c)
// X3 = (aX2 + c) % M  X3 % M = (aX2 + c)

// X2 % M - X3 % M = a(X1 - X2)
// a = (X2 % M - X3 % M) / (X1 - X2)