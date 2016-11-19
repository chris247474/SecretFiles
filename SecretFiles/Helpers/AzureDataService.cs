using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Microsoft.WindowsAzure.MobileServices;

namespace SecretFiles
{
	public class AzureDataService
	{
		public MobileServiceClient Client { get; set; }

		IMobileServiceTable<PostItem> postsTable;
		IMobileServiceTable<GroupItem> groupsTable;
		IMobileServiceTable<AccountItem> accountsTable;
		IMobileServiceTable<CommentItem> commentTable;
		IMobileServiceTable<Gateway> gateTable;

		public bool IsInitialized { get; set; }

		public AzureDataService() {
			Initialize();
		}

		public void Initialize()
		{
			//Create our client
			var handler = new Authenticator();
			//Create our client and pass in Authentication handler
			this.Client = new MobileServiceClient(
				Values.ApplicationURL);//, handler);
			
			//assign mobile client to handler
			//handler.Client = Client;

			//Client.CurrentUser = new MobileServiceUser(Settings.UserId);
			//Client.CurrentUser.MobileServiceAuthenticationToken = Settings.AuthToken;

			this.postsTable = Client.GetTable<PostItem>();
			this.groupsTable = Client.GetTable<GroupItem>();
			this.accountsTable = Client.GetTable<AccountItem>();
			this.commentTable = Client.GetTable<CommentItem>();
			this.gateTable = Client.GetTable<Gateway>();

			IsInitialized = true;
		}
		public bool IsOfflineEnabled
		{
			get { return postsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<PostItem>; }
		}
		public async Task<List<Gateway>> GetGateway()
		{
			try
			{
				List<Gateway> items = await gateTable
					.Where(gate => gate.ID != null)
					.ToListAsync();

				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task CreatePass(Gateway item)
		{
			try
			{
				if (item.ID == null)
				{
					await gateTable.InsertAsync(item);
				}
				else
				{
					await gateTable.UpdateAsync(item);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("create pass error: " + e.Message);
			}
		}
		public async Task<List<PostItem>> GetSinglePostByPostID(string postid, bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.ID == postid)
					.ToListAsync();

				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<PostItem> GetSinglePostByPostTextTitleUserIDGroupID(string text, string title, string userID, string groupid, bool syncItems = false)
		{
			try
			{
				UserDialogs.Instance.ShowLoading();
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.Body == text).Where(post => post.Title == title).Where(post => post.UserId == userID).Where(post => post.GroupID == groupid)
					.ToListAsync();
				UserDialogs.Instance.HideLoading();
				return items.ElementAtOrDefault(0);
				/*var itemArr = items.ToArray ();
				for(int c = 0;c < itemArr.Length;c++){
					if(string.Equals (itemArr[c].Body, text) && string.Equals (itemArr[c].Title, title) && string.Equals (itemArr[c].UserId, userID) && string.Equals (itemArr[c].GroupID, groupid)){
						return itemArr[c];
					}
				}*/

				return null;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<PostItem>> GetPostItemsAsync(bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.Title != null)
					.ToListAsync();

				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<PostItem>> GetPostItemsByUserIDAsync(string userId, bool syncItems = false)
		{
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.UserId == userId)
					.ToListAsync();

				items.Reverse();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<CommentItem>> GetCommentsByPostAsync(string postID, bool syncItems = false)
		{
			try
			{
				List<CommentItem> items = await commentTable
					.Where(Comment => Comment.PostID == postID)
					.ToListAsync();

				//	items.Reverse ();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<CommentItem>> GetCommentsByGroupIDAsync(string groupId, bool syncItems = false)
		{
			try
			{
				List<CommentItem> items = await commentTable
					.Where(Comment => Comment.GroupID == groupId)
					.ToListAsync();

				//	items.Reverse ();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<AccountItem>> GetAccountsAsync(bool syncItems = false)
		{
			try
			{
				List<AccountItem> items = await accountsTable
					.Where(x => x.ID != null)
					.ToListAsync();

				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}

		public async Task<bool> SaveAccountTaskAsync(AccountItem item)
		{
			try
			{
				if (Reachability.HasInternetConnection()) { 
					if (item.ID == null)
					{
						await accountsTable.InsertAsync(item);
					}
					else
					{
						await accountsTable.UpdateAsync(item);
					}
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("New account Save error: " + e.Message);
			}
			return false;
		}

		public async Task<String> CreateNewAccount()
		{
			Console.WriteLine("Creating new account");
			Random random = new Random();
			var randomusernamenumber = random.Next();
			var randomusername = randomusernamenumber.ToString();

			var user = new AccountItem { password = Values.DEFAULTPASSWORD, username = randomusername };
			await SaveAccountTaskAsync(user);

			Console.WriteLine("New account created");
			return (await GetUserID(user, randomusername));
		}
		public async Task<String> GetUserID(AccountItem item, string randomusername)
		{
			try
			{
				if (Reachability.HasInternetConnection()) { 
					var accounts = (await GetAccountsAsync()).ToArray();
					for (int i = 0; i < accounts.Length; i++)
					{
						if (string.Equals(randomusername, accounts[i].username))
						{
							return accounts[i].ID;
						}
					}
				}
			}
			catch (WebException)
			{
				Console.WriteLine("UserDialog here, no net connection");//user dialog
																	  //UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (TaskCanceledException)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			return string.Empty;
		}
		public async Task<AccountItem> GetUserByID(string id)
		{
			try
			{
				List<AccountItem> accounts = (await GetAccountsAsync()).Where(user => user.ID == id).ToList();
				return accounts.FirstOrDefault();

			}
			catch (WebException)
			{
				Console.WriteLine("UserDialog here, no net connection");//user dialog
																	  //UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (TaskCanceledException)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (Exception e)
			{
				Console.WriteLine("GetUserByID error: " + e.Message);
			}
			return null;
		}
		public async Task<List<PostItem>> GetPostItemsByGroupOrderByPopularAsync(string groupid, bool syncItems = false)
		{
			Console.WriteLine("In GetPostItemsByGroupAsync");
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.GroupID == groupid).OrderByDescending(PostItem => PostItem.reactionCount)
					.ToListAsync();

				Console.WriteLine("Done w GetPostItemsByGroupAsync");
				//items.Reverse ();
				//items.OrderBy (PostItem => PostItem.reactionCount);
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (WebException we)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (TaskCanceledException)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);

			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<List<PostItem>> GetPostItemsByGroupID(string groupid, bool orderByMostRecent = false) { 
			Console.WriteLine("In GetPostItemsByGroupAsync");
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.GroupID == groupid)
					.ToListAsync();

				Console.WriteLine("Done w GetPostItemsByGroupAsync");
				if(orderByMostRecent) items.Reverse();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (WebException we)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (TaskCanceledException)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);

			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		/*public async Task<List<PostItem>> GetPostItemsByGroupOrderByRecentAsync(string groupid, bool syncItems = false)
		{
			Console.WriteLine("In GetPostItemsByGroupAsync");
			try
			{
				List<PostItem> items = await postsTable
					.Where(PostItem => PostItem.GroupID == groupid)//.OrderByDescending (PostItem => PostItem.reactionCount )
					.ToListAsync();

				Console.WriteLine("Done w GetPostItemsByGroupAsync");
				items.Reverse();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (WebException we)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (TaskCanceledException)
			{
				//UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);

			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}*/

		public async Task SavePostItemsTaskAsync(PostItem item)
		{
			try
			{
				if (item.ID == null)
				{
					await postsTable.InsertAsync(item);
				}
				else
				{
					await postsTable.UpdateAsync(item);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("SavePostItemsTaskAsync error: " + e.Message);
			}
		}

		public async Task<List<GroupItem>> GetGroupItemsAsync()
		{
			Console.WriteLine("GetGroupItemsAsync started");
			try
			{
				List<GroupItem> items = await groupsTable
					.Where(Item => Item.groupName != null)
					.ToListAsync();

				Console.WriteLine("GetGroupItemsAsync: {0} items fetched", items.Count);
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Console.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (WebException)
			{
				Console.WriteLine("UserDialog here, no net connection");//user dialog
				UserDialogs.Instance.Alert("Your data connection is a bit slow right now");
			}
			catch (TaskCanceledException)
			{
				//	UserDialogs.Instance.WarnToast ("Your data connection is a bit slow right now", null, 2000);
			}
			catch (Exception e)
			{
				Console.WriteLine(@"Sync error: {0} - {1}", e.ToString(), e.Message);
			}
			return null;
		}
		public async Task<bool> SaveGroupsItemsTaskAsync(GroupItem item)
		{
			try
			{
				if (Reachability.HasInternetConnection())
				{
					if (item.ID == null)
					{
						await groupsTable.InsertAsync(item);
					}
					else
					{
						await groupsTable.UpdateAsync(item);
					}
					return true;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("GroupItem Save error: " + e.Message);
			}
			return false;
		}
		public async Task SaveCommentItemsTaskAsync(CommentItem item)
		{
			try
			{
				if (item.ID == null)
				{
					await commentTable.InsertAsync(item);
				}
				else
				{
					await commentTable.UpdateAsync(item);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("CommentItem Save error: " + e.Message);
			}
		}
	}
}
