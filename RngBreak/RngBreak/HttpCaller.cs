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
		public async Task<Account> CreateAccount(int accId)
		{
			using (HttpClient http = new HttpClient())
			{
				var responseMessage = await http.GetAsync($"http://95.217.177.249/casino/createacc?/createacc?id={accId}");
				var responseText = await responseMessage.Content.ReadAsStringAsync();
				var account = JsonConvert.DeserializeObject<Account>(responseText);

				return account;
			}
		}



	}	
}
