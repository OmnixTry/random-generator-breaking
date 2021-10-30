using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	class LCGBreaker : BaseBreaker
	{
		private readonly Int64 M = (Int64)Math.Pow(2, 32);

		public LCGBreaker(CasinoCaller caller): base(caller) {}

		public async Task HackLcg(string ackountId)
		{
			List<int> randoms = new List<int>();
			var account = await caller.CreateAccount(ackountId);
			for(int i = 0; i < 3; i++)
			{
				var resultt = await caller.MakeABet(1, 1, GameModes.LinearCongruential, ackountId);
				resultt.Print();
				randoms.Add(resultt.RealNumber);
			}
			// a = (X2 % M - X3 % M) / (X1 - X2)\
			int multiplyM = 0;
			long a = 0;
			for(multiplyM = -1000000; multiplyM < int.MaxValue; multiplyM++)
			{
				double doubleA = ((double)randoms[1] - (double)randoms[2] - multiplyM * M) / (randoms[0] - randoms[1]);
				if(((int)doubleA) == doubleA)
				{
					Console.WriteLine(((double)randoms[1] - (double)randoms[2] - multiplyM * M));
					Console.WriteLine((randoms[0] - randoms[1]));
					a = (int)doubleA;
					break;
				}
			}
			
			// c = X2 % M - aX1
			long c = 0; //= randoms[1] - a * randoms[0] + M* multiplyM;

			// X2 + X3 - (i + j)*M = a(x1+x2) + 2c
			// c = (X2 + X3 - (i + j)*M - a(x1+x2)) / 2
			c = (randoms[2] + randoms[1] - M * multiplyM - a * (randoms[0] + randoms[1])) / 2;
			if(c < 0)
			{
				while(c < int.MinValue)
				{
					c += M;
				}
			}
			if (c > 0)
			{
				while (c > int.MaxValue)
				{
					c -= M;
				}
			}

			long Next = (int)((a * randoms[2] + c) % M);
			BetResponse result = null;
			do
			{
				result = await caller.MakeABet(result == null ? 1 : result.Account.Money , Next, GameModes.LinearCongruential, ackountId);
				Console.WriteLine(Next);
				result.Print();
				Console.WriteLine($"A: {a}; C: {c}");
				Next = (int)((a * result.RealNumber + c) % M);
			} while (result.Account.Money < 1000000);			
		}
		
		


	}
}
