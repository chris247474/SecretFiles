using System;
using Newtonsoft.Json;

namespace SecretFiles
{
	public class PostItem
	{
		string id;
		string body;
		string title;

		[JsonProperty(PropertyName = "id")]
		public string ID
		{
			get { return id; }
			set { id = value; }
		}
		[JsonProperty("postImage")]
		public string PostImage { get; set; }
		[JsonProperty("userId")]
		public string UserId { get; set; }
		[JsonProperty("groupID")]
		public string GroupID { get; set; }
		[JsonProperty("reactionCount")]
		public int reactionCount { get; set; } = 0;
		[JsonProperty(PropertyName = "body")]
		public string Body
		{
			get { return body; }
			set { body = value; }
		}
		[JsonProperty(PropertyName = "title")]
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }
	}
}
