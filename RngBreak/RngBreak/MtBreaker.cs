using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	class MtBreaker : BaseBreaker
	{
		public MtBreaker(CasinoCaller caller): base(caller) { }

		public async Task HackMT(string ackountId, long offset)
		{
			/*
			var result = await caller.MakeABetUnsigned(1, 1, GameModes.MerseneTwister, ackountId);

			MersenneTwister twister = new MersenneTwister();
			for (int i = -50; i < 50; i++)
			{
				twister = new MersenneTwister();
				twister.init_genrand((uint)(offset + i));
				var randNo = twister.genrand_int32();
				if ((uint)randNo == result.RealNumber)
				{
					break;
				}
			}

			result = await caller.MakeABetUnsigned(1, (long)twister.genrand_int32(), GameModes.MerseneTwister, ackountId);
			Console.WriteLine(result.Message);
			*/
			
			List<MersenneTwister> twisters = new List<MersenneTwister>();
			Console.WriteLine(offset);
			for (int i = -50; i < 50; i++)
			{
				var twister = new MersenneTwister();
				twister.init_genrand((uint)(offset + i));
				twisters.Add(twister);
			}

			var result = await caller.MakeABetUnsigned(1, 1, GameModes.MerseneTwister, ackountId);
			Console.WriteLine("RealNUmber: " + result.RealNumber);
			MersenneTwister correct = twisters[0];
			foreach (var item in twisters)
			{
				var number = item.genrand_int32();
				Console.WriteLine("Random: " + number);
				if ((uint)number == result.RealNumber)
				{
					correct = item;
					break;
				}
			}
			result = await caller.MakeABetUnsigned(1, (long)correct.genrand_int32(), GameModes.MerseneTwister, ackountId);
			Console.WriteLine(result.Message);

			/*
			List<Random> twisters = new List<Random>();
			var longOffset = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			ulong offset = (ulong)longOffset - 7;

			for (int i = 0; i < 15; i++)
			{
				var twister = Randoms.Create((int)offset + i);
				twisters.Add(twister);
			}

			var result = await caller.MakeABet(1, 1, GameModes.MerseneTwister, ackountId);
			Console.WriteLine("RealNUmber: " + (uint)result.RealNumber);
			var correct = twisters[0];
			foreach (var item in twisters)
			{
				var number = item.Next();
				Console.WriteLine("Random: " + number);
				if (number == (uint)result.RealNumber)
				{
					correct = item;
					break;
				}
			}

			result = await caller.MakeABet(1, correct.Next(), GameModes.MerseneTwister, ackountId);
			*/
		}	
	}
}
