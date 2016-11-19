using System;
using Newtonsoft.Json;

namespace SecretFiles
{
	public class Gateway
	{
		string id;

		[JsonProperty(PropertyName = "id")]
		public string ID
		{
			get { return id; }
			set { id = value; }
		}


		[JsonProperty("pass")]
		public string Pass { get; set; }
	}
}
