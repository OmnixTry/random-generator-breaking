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

		public long unBitshiftRightXor(long receivedValue, int numOfShifts)
		{
			long i = 0;
			long result = 0;
			while (i * numOfShifts < 32)
			{
				long minusOneShifted = -1 << (32 - numOfShifts);
				long partMask = UnginedRightShiftInt(minusOneShifted, (int)(numOfShifts * i));
				long part = receivedValue & partMask;
				receivedValue ^= UnginedRightShift(part, numOfShifts);
				result |= part;
				i++;
			}
			return result;
		}

		public long unBitshiftLeftXor(long receivedValue, int numOfShifts, long AndMask)
		{
			long i = 0;
			long result = 0;
			while (i * numOfShifts < 32)
			{
				long minusOneShifted = (int)(unchecked((uint)-1) >> (32 - numOfShifts));
				long partMask =  minusOneShifted << (int)(numOfShifts * i);
				long part = receivedValue & partMask;
				receivedValue ^= (part << numOfShifts) & AndMask;
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

		private long UnginedRightShift(long valueToShift, int numOfShifts)
		{
			return (long)((ulong)valueToShift >> numOfShifts);
		}

		private int UnginedRightShiftInt(long valueToShift, int numOfShifts)
		{
			return (int)((uint)valueToShift >> numOfShifts);
		}
	}
}
