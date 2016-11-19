using System;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace SecretFiles
{
	public class ChatMessageService
	{
		public static Client ChatClient;

		public static async Task ConnectChatClientToServer()
		{
			ChatClient = new Client("iOS");

			try
			{
				await ChatClient.Connect(HandlePost, HandleComment, HandleConnectionBroadcast);
			}
			catch (Exception e)
			{
				Console.WriteLine("Could not connect to SignalR server: {0} - {1}", e.InnerException, e.Message);
				try
				{
					await ChatClient.Connect(HandlePost, HandleComment, HandleConnectionBroadcast);
				}
				catch (Exception ex)
				{
					UserDialogs.Instance.Alert("Can't connect to Secret Files", "Try quitting and reopening the app");
				}
			}
		}

		static void HandleConnectionBroadcast(string message) { 
			Console.WriteLine("Recieved from broadcastMessage: {0}", message);
			//SignalRReply(message, true);
		}
		static void HandlePost(PostItem postMessage) { //not working check server
			Console.WriteLine("Post data received, processing");
			//add data to new postitemstacklayout, add to top of scrollfeed.content
			if (postMessage is PostItem)
			{
				Console.WriteLine("Recieved from SignalR: post by {0}, post body: {1}, in group {2}", 
				                  postMessage.Title, postMessage.Body, postMessage.GroupID);

				GlobalVars.CurrentChat.AddReceivedPostUI(postMessage);
			}

			//ChatClient.SendToWeb("intercepted post", postMessage.Title);
		}
		static void HandleComment(CommentItem commentMessage) { 
			if (commentMessage is CommentItem)
			{
				Console.WriteLine("Recieved Comment from SignalR: comment by {0}, comment body: {1}, in group {2}, in post {3}",
								  commentMessage.UserCommentName, commentMessage.CommentText, commentMessage.GroupID, commentMessage.PostID);

				/*if (string.Equals(message.PostID, post.ID))
				{
					Console.WriteLine("Found post where comment was typed, adding it in");
					//post comment
					//UIBuilder.ReplaceTransparentCommentOrAdd(container, message, parentPost, post);
				}*/
			}
		}

		public static void PostToThread(PostItem post) {
			Console.WriteLine("In PostToThread");
			if (post != null && Reachability.HasInternetConnection())
			{
				ChatClient.SendPost(post);
				GlobalVars.CloudDB.SavePostItemsTaskAsync(post);
			}
		}
		public static void CommentToPost(CommentItem comment) {
			if(comment != null) ChatClient.SendComment(comment);
		}

		public static async void SignalRReply(string message, bool web)
		{
			var result = await UserDialogs.Instance.PromptAsync(message, "Reply", "Ok", "Cancel");
			if (result.Ok)
			{
				if (string.Equals(result.Text, "stop"))
				{
					return;
				}
				else {
					if (web)
						ChatClient.SendToWeb("iOS", result.Text);
					//else
					//ChatClient.Send (result.Text);
				}
			}
		}

		/*void UpdatePosts(//PullToRefreshViewModel refreshModel, StackLayout PostsContainer, 
						 PostItem message, string groupid)
		{
			if (string.Equals(message.GroupID, groupid))
			{
				Console.WriteLine("signalr message and current group match!");
				if (message != null)
				{
					Console.WriteLine("Recieved PostItem, generated ID is: {0}", message.ID);

					//add to top stack
if (PostsContainer == null)
						{
							Console.WriteLine("no posts in feed yet");
							refreshModel.ExecuteRefreshCommand();
						}
						else {
							Console.WriteLine("{0}, posts in feed", PostsContainer.Children.Count);
							PostsContainer.Children.Insert(1, new PostItemStackLayout(message));
						}
				}
				else {
					Console.WriteLine("Couldn't fetch PostItem from db");
				}

			}
			else {
				Console.WriteLine("PostItem {0} received, does not match group id {1}", message.ID, message.GroupID);
			}
		}*/
	}
}
