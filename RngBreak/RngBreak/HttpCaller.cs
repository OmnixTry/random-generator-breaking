using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RngBreak
{
	class CasinoCaller
	{
		private const string urlBase = "http://95.217.177.249/casino/";
		public async Task<Account> CreateAccount(string accId)
		{
			using (HttpClient http = new HttpClient())
			{
				var responseMessage = await http.GetAsync($"{urlBase}createacc?id={accId}");
				var responseText = await responseMessage.Content.ReadAsStringAsync();
				//Console.WriteLine(responseText);
				var account = JsonConvert.DeserializeObject<Account>(responseText);

				return account;
			}
		}

		public async Task<BetResponse> MakeABet(int sumOfMoney, long theNumberYouBetOn, string mode, string accId)
		{
			using (HttpClient http = new HttpClient())
			{
				var responseMessage = await http.GetAsync($"{urlBase}play{mode}?id={accId}&bet={sumOfMoney}&number={theNumberYouBetOn}");
				var responseText = await responseMessage.Content.ReadAsStringAsync();
				Console.WriteLine(responseText);
				var betResponse = JsonConvert.DeserializeObject<BetResponse>(responseText);

				return betResponse;
			}
		}

	}	
}
