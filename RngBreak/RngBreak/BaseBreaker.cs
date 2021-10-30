using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngBreak
{
	abstract class BaseBreaker
	{
		protected readonly CasinoCaller caller;

		public BaseBreaker(CasinoCaller caller)
		{
			this.caller = caller;
		}
	}
}
