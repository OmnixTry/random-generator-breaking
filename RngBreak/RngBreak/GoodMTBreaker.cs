using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	class GoodMTBreaker : BaseBreaker
	{
		public GoodMTBreaker(CasinoCaller caller) : base(caller) { }

		public static long unBitshiftRightXor(long receivedValue, int shift)
		{
			long i = 0;
			long result = 0;
			while (i * shift < 32)
			{
				long partMask = (int)((uint)(-1 << (32 - shift)) >> (int)(shift * i));
				long part = receivedValue & partMask;
				receivedValue ^= (long)((ulong)part >> shift);
				result |= part;
				i++;
			}
			return result;
		}

		public static long unBitshiftLeftXor(long receivedValue, int shift, long mask)
		{
			long i = 0;
			long result = 0;
			while (i * shift < 32)
			{
				long partMask = ((int)(unchecked((uint)-1) >> (32 - shift))) << (int)(shift * i);
				long part = receivedValue & partMask;
				receivedValue ^= (part << shift) & mask;
				result |= part;
				i++;
			}
			return result;
		}

		public async Task Hacc(string accId)
		{
			BetResponseUnsigned result;
			MersenneTwister mersenneTwister = new MersenneTwister();
			ulong[] state = new ulong[624];
			for (int i = 0; i < 624; i++)
			{
				result = await caller.MakeABetUnsigned(1, 1, GameModes.BetterMt, accId);
				long value = result.RealNumber;
				//long value = (long)mersenneTwister.genrand_int32();
				value = unBitshiftRightXor(value, 18);
				value = unBitshiftLeftXor(value, 15, 0xefc60000);
				value = unBitshiftLeftXor(value, 7, 0x9d2c5680);
				value = unBitshiftRightXor(value, 11);
				state[i] = (ulong)value;
			}

			/*
			for (int i = 0; i < 624; i++)
			{
				Console.WriteLine($"Mine: {state[i]}; Correct: {MersenneTwister.mt[i]}");
			}
			*/


			MersenneTwister.mt = state;
			long num = (long)mersenneTwister.genrand_int32();
			result = await caller.MakeABetUnsigned(1, num, GameModes.BetterMt, accId);
			result.Print();
		}

		private int UnginedRightShift()
		{

		}
	}
}
