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

		public static long unBitshiftRightXor(long value, int shift)
		{
			// we part of the value we are up to (with a width of shift bits)
			long i = 0;
			// we accumulate the result here
			long result = 0;
			// iterate until we've done the full 32 bits
			while (i * shift < 32)
			{
				// create a mask for this part
				long partMask = (int)((uint)(-1 << (32 - shift)) >> (int)(shift * i));
				// obtain the part
				long part = value & partMask;
				// unapply the xor from the next part of the integer
				value ^= (long)((ulong)part >> shift);
				// add the part to the result
				result |= part;
				i++;
			}
			return result;
		}

		public static long unBitshiftLeftXor(long value, int shift, long mask)
		{
			// we part of the value we are up to (with a width of shift bits)
			long i = 0;
			// we accumulate the result here
			long result = 0;
			// iterate until we've done the full 32 bits
			while (i * shift < 32)
			{
				// create a mask for this part
				long partMask = ((int)(unchecked((uint)-1) >> (32 - shift))) << (int)(shift * i);
				// obtain the part
				long part = value & partMask;
				// unapply the xor from the next part of the integer
				value ^= (part << shift) & mask;
				// add the part to the result
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
	}
}
