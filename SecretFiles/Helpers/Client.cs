using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.AspNet.SignalR.Client;

namespace SecretFiles
{
	public class Client
	{
		private readonly string _platform;
		private readonly HubConnection _connection;
		private readonly IHubProxy _proxy;

		public event EventHandler<PostItem> OnMessageReceived;
		public event EventHandler<CommentItem> OnCommentReceived;
		public event EventHandler<string> OnBroadcastMessageReceived;

		public Client(string platform)
		{
			_platform = platform;
			_connection = new HubConnection("http://chrisdavetvsignalr.azurewebsites.net/");
			_proxy = _connection.CreateHubProxy("ChatHub");
		}

		public async Task Connect(Action<PostItem> postAction, Action<CommentItem> commentAction, Action<string> broadcastAction,
		                          bool broadcastConnected = true)
		{
			Console.WriteLine("Connecting");
			await _connection.Start();
			Console.WriteLine("Connected");

			_proxy.On("messageReceived", (PostItem message) =>
				{
					if (OnMessageReceived != null)
						OnMessageReceived(this, message);
				});

			_proxy.On("commentReceived", (CommentItem message) =>
				{
					if (OnCommentReceived != null)
						OnCommentReceived(this, message);
				});

			_proxy.On("broadcastMessage", (string name, string message) =>
				{
					if (OnBroadcastMessageReceived != null)
						OnBroadcastMessageReceived(this, string.Format("{0}: {1}", name, message));
				});

			PrepareToReceiveMessages(postAction, commentAction, broadcastAction);

			Console.WriteLine("broadcasting connected status to signalrserver");
			if(broadcastConnected) SendToWeb(Settings.Username, "Client connected");
		}
		bool PrepareToReceiveMessages(Action<PostItem> postAction, Action<CommentItem> commentAction, Action<string> broadcastAction)
		{
			this.OnBroadcastMessageReceived += (sender, message) => broadcastAction(message);
			this.OnMessageReceived += (sender, e) => postAction(e);
			this.OnCommentReceived += (sender, e) => commentAction(e);

			return true;
		}
		public Task SendPost(PostItem message)
		{
			Console.WriteLine("In SendPost");
			try
			{
				SendToWeb("Post", message.Body);
				return _proxy.Invoke("SendToSecretFilesClient", message);
			}
			catch (InvalidOperationException ie) {
				UserDialogs.Instance.Alert("Not yet connected to server");
			}
			return null;
		}
		public Task SendComment(CommentItem message)
		{
			try{
				_proxy.Invoke("SendComentsToClients", message);
			}
			catch (InvalidOperationException ie)
			{
				UserDialogs.Instance.Alert("Not yet connected to server");
			}
			return null;
		}
		public Task SendToWeb(string name, string message)
		{
			try{
				return _proxy.Invoke("Send", name, message);
			}
			catch (InvalidOperationException ie)
			{
				UserDialogs.Instance.Alert("Not yet connected to server");
			}
			return null;
		}

	}
}
