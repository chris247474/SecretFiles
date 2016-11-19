using Foundation;
using System;
using UIKit;
using Acr.UserDialogs;
using CoreGraphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretFiles
{
	public partial class SecretFileChatController : UIViewController
	{
		GroupItem SecretFile;
		List<PostItem> AllPosts;
		NSObject keyboardShownNotif, keyboardHiddenNotif;
		const string placeholder = "Type a message here...";
		CGRect OriginalViewFrame, OriginalScrollViewFrame, OriginalImageViewFrame, OriginalTextViewFrame;
		TextViewDelegate TextView_Delegate;
		nfloat animationDuration = 0.25f, heightDiff, labelWidth, labelHeight, ChatBubbleVerticalSpacing = 10f, ChatBubbleY = 0f;
		bool AlreadySaved;

		List<PostItem> PendingPosts = new List<PostItem>();
		List<ChatBubbleViewExtension> PendingPostBubbles = new List<ChatBubbleViewExtension>();

		public SecretFileChatController(IntPtr handle) : base(handle)
		{
		}

		public void SetSecretFileContent(GroupItem content)
		{
			this.SecretFile = content;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			GlobalVars.CurrentChat = this;
			SetupUI();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			SetPlaceholderTextIfTextViewIsEmpty();

			TextField.Layer.BorderWidth = 0.5f;
			TextField.Layer.BorderColor = UIColor.LightGray.CGColor;

			View.BringSubviewToFront(ChatScrollView);

			LoadAllMessages();
		}

		async Task LoadAllMessages() {
			LoadingIndicatorView.StartAnimating();

			AllPosts = await GlobalVars.CloudDB.GetPostItemsByGroupID(SecretFile.ID);
			AddMessagesToView(AllPosts);

			LoadingIndicatorView.StopAnimating();
		}
		void AddMessagesToView(List<PostItem> posts) { 
			for (int c = 0; c < posts.Count; c++)
			{
				if (string.Equals(Settings.UserId, posts[c].UserId))
				{
					AddLocalClientPostToUI(posts[c], PostStatusType.PreviousPost);
				}
				else { 
					AddLocalClientPostToUI(posts[c], PostStatusType.Received2ndPartyPostFromServer);
				}
			}
		}
		void SetupUI() { 
			RegisterKeyboardEvents();

			//hide keyboard on tap
			ChatScrollView.AddGestureRecognizer(new UITapGestureRecognizer(() =>
			{
				TextField.ResignFirstResponder();
			}));
			ChatScrollView.ContentSize = new CGSize(View.Frame.Width, View.Frame.Height);

			//set placeholder text if textfield is blank
			TextView_Delegate = new TextViewDelegate(ChatScrollView, ChatBackground);
			TextField.Delegate = TextView_Delegate;
		}

		ChatBubbleViewExtension CreateChatBubble(PostItem post, PostStatusType postStatus) 
		{
			bool IsLocal = false;
			if (postStatus == PostStatusType.SendingPost || postStatus == PostStatusType.PreviousPost) IsLocal = true;

			Console.WriteLine("postStatus is {0}, IsLocal is {1}", postStatus.ToString(), IsLocal);

			//base measurements
			var verticalImageAdjustment = 5f;
			var ChatAreaSpacing = 10f;
			var ImageDimension = 22f;
			nfloat labelSpacing = 5;
			var ChatBubbleWidth = 200;//use DecideChatBubbleWidth(
			var ChatBubbleHeight = 100;//heightDiff *1.5f;

			//dependent measurements
			ChatBubbleY += ChatBubbleHeight + ChatAreaSpacing;
			labelWidth = ChatBubbleWidth - (ChatBubbleWidth - (labelSpacing * 2));
			labelHeight = ChatBubbleHeight - (ChatBubbleHeight - (labelSpacing * 2));

			var ImageAndBubbleFrame = new CGRect(0, ChatBubbleY, ChatScrollView.Frame.Width, ChatBubbleHeight);
			var ImageX = IsLocal ? ImageAndBubbleFrame.Width - ChatAreaSpacing - ImageDimension : ChatAreaSpacing;
			var ChatBubbleX = IsLocal ? ImageX - ChatBubbleWidth - ChatAreaSpacing : ImageDimension + (ChatAreaSpacing * 2);

			//profile pic
			var ProfileImageView = new UIImageView();
			ProfileImageView.Image = UIImage.FromFile("profilepics/people.png");
			ProfileImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
			ProfileImageView.Frame = new CGRect(ImageX, 
			                                    verticalImageAdjustment,
			                                    ImageDimension, 
			                                    ImageDimension);

			//chat bubble without the profile pic
			var ChatFrame = new CGRect(ChatBubbleX, 
			                           0,
			                           ChatBubbleWidth, 
			                           ChatBubbleHeight);
			var ChatMessage = new UIView();
			ChatMessage.BackgroundColor = UIColor.White;
			ChatMessage.Layer.CornerRadius = 12;
			ChatMessage.Frame = ChatFrame;

			//this is the username
			var TitleLabel = new UILabel(new CGRect(labelSpacing, labelSpacing, labelWidth, labelHeight));
			TitleLabel.TextColor = UIColor.LightGray;
			TitleLabel.TextAlignment = UITextAlignment.Left;
			TitleLabel.Font = UIFont.SystemFontOfSize(14);
			TitleLabel.Text = post.Title;
			TitleLabel.SizeToFit();

			//message content
			var BodyLabel = new UILabel(new CGRect(labelSpacing, TitleLabel.Frame.Height + (labelSpacing * 2), labelWidth, labelHeight));
			BodyLabel.TextColor = UIColor.Black;
			BodyLabel.Text = post.Body;
			BodyLabel.Font = UIFont.SystemFontOfSize(15);
			BodyLabel.SizeToFit();
			BodyLabel.Lines = 50;
			BodyLabel.LineBreakMode = UILineBreakMode.WordWrap;

			//time message was sent
			var TimeStampLabel = new UILabel(new CGRect(ChatBubbleWidth - (labelWidth * 4), ChatBubbleHeight - labelHeight - (labelSpacing * 2)
														, labelWidth, labelHeight));
			TimeStampLabel.TextAlignment = UITextAlignment.Left;
			TimeStampLabel.Font = UIFont.SystemFontOfSize(12);
			TimeStampLabel.Text = string.Format("{0}:{1}", DateTime.Now.TimeOfDay.Hours, DateTime.Now.TimeOfDay.Minutes);
			TimeStampLabel.TextColor = UIColor.LightGray;
			TimeStampLabel.SizeToFit();

			//add message text into one bubble view
			ChatMessage.AddSubview(TitleLabel);
			ChatMessage.BringSubviewToFront(TitleLabel);
			ChatMessage.AddSubview(BodyLabel);
			ChatMessage.BringSubviewToFront(BodyLabel);
			ChatMessage.AddSubview(TimeStampLabel);
			ChatMessage.BringSubviewToFront(TimeStampLabel);
			ChatMessage.SizeToFit();

			//add image and text bubble together
			//this is the chat bubble containing text and the profile pic
			var ImageAndBubbleViewContainer = new ChatBubbleViewExtension();
			ImageAndBubbleViewContainer.BackgroundColor = UIColor.Clear;
			ImageAndBubbleViewContainer.Frame = ImageAndBubbleFrame;
			ImageAndBubbleViewContainer.AddSubview(ProfileImageView);
			ImageAndBubbleViewContainer.BringSubviewToFront(ProfileImageView);
			ImageAndBubbleViewContainer.AddSubview(ChatMessage);
			ImageAndBubbleViewContainer.BringSubviewToFront(ChatMessage);

			//add image and text bubble to chat window
			ChatScrollView.AddSubview(ImageAndBubbleViewContainer);
			ChatScrollView.BringSubviewToFront(ImageAndBubbleViewContainer);

			return ImageAndBubbleViewContainer;
		}

		nfloat CalculateTimeStampLabelYPosition(UILabel titleLabel, UILabel bodyLabel, int labelSpacing = 5)
		{
			var lines = RecommendNumberOfLines(bodyLabel);
			Console.WriteLine("{0} lines in post", lines);
			if (lines > 1)
			{
				return titleLabel.Frame.Height + bodyLabel.Frame.Height + (labelSpacing * 2.8f);
			}
			else {
				return titleLabel.Frame.Height + (labelSpacing * 2.8f);
			}
		}

		int RecommendNumberOfLines(UILabel label)
		{
			var lines = label.Text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
			return lines;
		}

		void SetPlaceholderTextIfTextViewIsEmpty()
		{
			if (string.IsNullOrWhiteSpace(TextField.Text))
			{
				TextField.TextColor = UIColor.LightGray;
				TextField.Text = placeholder;
			}
		}
		void ClearTextViewWhenStartingToType()
		{

			if (string.Equals(TextField.Text, placeholder))
			{
				TextField.Text = string.Empty;
				TextField.TextColor = UIColor.Black;
			}
		}
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			if (!AlreadySaved)
			{
				SaveStartingFrames();
				AlreadySaved = true;
			}
		}

		void SaveStartingFrames()
		{
			OriginalViewFrame = View.Frame;
			OriginalScrollViewFrame = ChatScrollView.Frame;
			OriginalImageViewFrame = ChatBackground.Frame;
			OriginalTextViewFrame = TextField.Frame;
		}

		void RestoreStartingFrames_AdjustForChatTextContent()
		{
			heightDiff = OriginalTextViewFrame.Y - TextField.Frame.Y;
			Console.WriteLine("Chat window content height displacement is {0}", heightDiff);
			if (heightDiff <= 0) heightDiff = 0;

			this.View.Frame = OriginalViewFrame;

			ChatScrollView.Frame = new CGRect(OriginalScrollViewFrame.X, OriginalScrollViewFrame.Y - heightDiff,
											  OriginalScrollViewFrame.Width, OriginalScrollViewFrame.Height);

			ChatBackground.Frame = new CGRect(OriginalImageViewFrame.X, OriginalImageViewFrame.Y - heightDiff,
											  OriginalImageViewFrame.Width, OriginalImageViewFrame.Height);
		}

		public override void ViewWillUnload()
		{
			base.ViewWillUnload();
			keyboardShownNotif.Dispose();
			keyboardHiddenNotif.Dispose();
		}

		partial void GalleryButton_TouchUpInside(UIButton sender)
		{
			PhotoPickerService.ChoosePicture();
		}

		partial void SendButton_TouchUpInside(UIButton sender)
		{
			var newPost = new PostItem
			{
				Title = "Chris Verceles",
				Body = TextField.Text,
				UserId = Settings.UserId,
				GroupID = this.SecretFile.ID
			};

			//Add chat bubble to UI
			AddLocalClientPostToUI(newPost, PostStatusType.SendingPost);

			//send message to chat serv
			Console.WriteLine("Sending post to server");
			ChatMessageService.PostToThread(newPost);

			ResetTextField();
		}

		//called by ChatMessageService when receiving a post from server
		public void AddReceivedPostUI(PostItem item)
		{
			InvokeOnMainThread(() => { 
				ChatBubbleViewExtension postBubble = null;

				//if posted by this account, then find pending post then undo transparency
				if (string.Equals(item.UserId, Settings.UserId))
				{
					var bubbleArr = PendingPostBubbles.ToArray();
					for (int c = 0; c < bubbleArr.Length; c++)
					{
						Console.WriteLine("Searching for matching post, iteration {0}", c);
						if (IsSamePostItem(bubbleArr[c].PostData, item))
						{
							Console.WriteLine("Found possible matching post w same title and body, iteration {0} with body {1} and alpha {2}",
											  c, bubbleArr[c].PostData.Body, bubbleArr[c].Alpha);
							if (IsPending(bubbleArr[c]))
							{
								Console.WriteLine("Found pending post, iteration {0} with body {1} and alpha {2}",
												  c, bubbleArr[c].PostData.Body, bubbleArr[c].Alpha);
								bubbleArr[c].Alpha = 1.0f;
								Console.WriteLine("Set alpha to {0}", bubbleArr[c].Alpha);
							}
							//else?
						}
						else Console.WriteLine("iteration {0}, not a match", c);
					}
				}
				else {
					postBubble = CreateChatBubble(item, PostStatusType.Received2ndPartyPostFromServer);
				}

				ChatScrollView.ContentSize = new CGSize(ChatScrollView.ContentSize.Width,
														ChatBubbleY + TextField.Frame.Height + SmileyButton.Frame.Height + 100);
			});
		}

		bool IsLocalPost(PostItem item) {
			foreach (var post in PendingPosts) {
				if (IsSamePostItem(post, item)) {
					return true;
				}
			}
			return false;
		}
		bool IsSamePostItem(PostItem post1, PostItem post2, bool testMode_DoNotCompareUserID = false) {
			var IsSame_TestMode = string.Equals(post1.Title, post2.Title) && string.Equals(post1.Body, post2.Body);
			var IsSame =  string.Equals(post1.Title, post2.Title) && string.Equals(post1.Body, post2.Body) && string.Equals(post1.UserId, post2.UserId);
			Console.WriteLine("IsSsamePostItem: Comparing: testmode - {0}, actual - {1}", IsSame_TestMode, IsSame);
			return testMode_DoNotCompareUserID ? IsSame_TestMode : IsSame;
		}
		bool IsPending(ChatBubbleViewExtension postBubble) {
			return postBubble.Alpha < 1.0f;
		}

		//called by client app when user composes message
		public void AddLocalClientPostToUI(PostItem item, PostStatusType postStatus)
		{
			InvokeOnMainThread(() => { 
				Console.WriteLine("Adding post, ChatBubbleY is {0}", ChatBubbleY);

				ChatBubbleViewExtension postBubble =
					CreateChatBubble(item, postStatus);

				if (postStatus == PostStatusType.SendingPost)
				{
					Console.WriteLine("Adding post w body {0} to pending list", item.Body);
					PendingPosts.Add(item);
					postBubble.Alpha = 0.5f;
					postBubble.PostData = item;
					PendingPostBubbles.Add(postBubble);
				}

				ChatScrollView.ContentSize = new CGSize(ChatScrollView.ContentSize.Width,
														ChatBubbleY + TextField.Frame.Height + SmileyButton.Frame.Height + 100);
				Console.WriteLine("Added post, ChatBubbleY is {0}", ChatBubbleY);
			});
		}

		void ResetTextField()
		{
			TextField.Text = string.Empty;
			SetPlaceholderTextIfTextViewIsEmpty();
			TextView_Delegate.AdjustChatUIAsUserTypes(TextField);
			TextField.ResignFirstResponder();
		}

		void RegisterKeyboardEvents()
		{
			Console.WriteLine("Registering");

			keyboardShownNotif = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
			{
				Console.WriteLine("Notification: {0}", args.Notification);
				Console.WriteLine("FrameBegin {0}, {1}, {2}, {3}", args.FrameBegin.X, args.FrameBegin.Y, args.FrameBegin.Width, args.FrameBegin.Height);
				Console.WriteLine("FrameEnd {0}, {1}, {2}, {3}", args.FrameEnd.X, args.FrameEnd.Y, args.FrameEnd.Width, args.FrameEnd.Height);
				Console.WriteLine("AnimationDuration {0}", args.AnimationDuration);
				Console.WriteLine("AnimationCurve {0}", args.AnimationCurve);

				UIView.Animate(animationDuration, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
				{
					ClearTextViewWhenStartingToType();
					View.Frame = new CGRect(View.Frame.X, View.Frame.Y - (args.FrameEnd.Height),
											View.Frame.Width, View.Frame.Height);

				}, null);
			});

			keyboardHiddenNotif = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
			{
				Console.WriteLine("keyboard hidden");
				UIView.Animate(animationDuration, 0, UIViewAnimationOptions.CurveEaseInOut, () =>
				{
					SetPlaceholderTextIfTextViewIsEmpty();
					RestoreStartingFrames_AdjustForChatTextContent();
				}, null);
			});
		}
	}
}
