using System;
using System.Threading.Tasks;
using MersenneTwister;

namespace RngBreak
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var rand = new Random();
			string accId = rand.Next().ToString();
			CasinoCaller caller = new CasinoCaller();
			var offset = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			var acc = await caller.CreateAccount(accId);
			var breaker = new LCGBreaker(caller);
			//await breaker.HackLcg(accId);
			Console.WriteLine(DateTimeOffset.UtcNow);
			await breaker.HackMT(accId, offset);

			var breakerMt = new GoodMTBreaker(caller);
			await breakerMt.Hacc(accId);

			long c = -5;
			int b = 2;
			Console.WriteLine((ulong)c >> b);


			/*
			Console.WriteLine("\n\n\n\n================");
			var twister = new MersenneTwister();
			twister.init_genrand(1635594486);
			Console.WriteLine("Next: " + twister.genrand_int32());
			Console.WriteLine("Next: " + twister.genrand_int32());
			Console.WriteLine("Next: " + twister.genrand_int32());
			Console.WriteLine("Next: " + twister.genrand_int32());
			Console.WriteLine("Next: " + twister.genrand_int32());
			*/


			//var response = await caller.MakeABet(1, 1, GameModes.LinearCongruential, accId);
			//response.Print();


			//Console.WriteLine(DateTime.Now);
			//acc.Print();

			//var response = await caller.MakeABet(1, 1, GameModes.LinearCongruential, accId);
			//response.Print();
		}
	}
}

// 5 % 2 = 1			5 % 3 = 2
// 10 % 8 = 2			10 % 2 = 0



// X2 = (aX1 + c) % M  X2 % M = (aX1 + c)
// X3 = (aX2 + c) % M  X3 % M = (aX2 + c)

// X2 = (aX1 + c) % M  x2 = aMX1 + cM
// X3 = (aX2 + c) % M  


// X2 % M - X3 % M = a(X1 - X2)
// a = (X2 % M - X3 % M) / (X1 - X2)
// c = X2 % M - aX1

/* a = (X2 - X3) / (X1- X2)
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */

// X2 = (aX1 + c) + i * M
// X3 = (aX2 + c) + j * M
// 
// X2 - X3 - i*M + j*M = a(X1-X2)
// X2 + X3 - (i + j)*M = a(x1+x2) + 2c