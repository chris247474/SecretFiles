using System;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace SecretFiles
{
	public class TextViewDelegate: UITextViewDelegate
	{
		UIView[] Views;
		nfloat animationDuration = 0.125f;

		public TextViewDelegate(params UIView[] viewsToAdjust) {
			this.Views = viewsToAdjust;
		}
		public override void Changed(UITextView textView)
		{
			Console.WriteLine("In TextViewDelegate.Changed");
			Console.WriteLine("TextView height is {0}, y is {1}", textView.Frame.Height, textView.Frame.Y);

			AdjustChatUIAsUserTypes(textView);
		}
		public void AdjustChatUIAsUserTypes(UITextView textView) {
			if (textView == null) throw new NullReferenceException("TextViewDelegate.AdjsutChatUIAsUserTypes method received null param");

			var maxHeight = UIScreen.MainScreen.ApplicationFrame.Height * 0.4f;
			var width = textView.Frame.Size.Width;
			var newSize = textView.SizeThatFits(new CGSize(width, maxHeight));
			var newHeight = Math.Min(newSize.Height, maxHeight);
			var heightAdjustDiff = (newHeight - textView.Frame.Height);
			var newFrame = new CGRect(textView.Frame.X, textView.Frame.Y - heightAdjustDiff,
									  Math.Max(newSize.Width, width), newHeight);
			UIView.Animate(animationDuration, () =>
			{
				textView.Frame = newFrame;
				foreach (var view in Views)
				{
					view.Frame = new CGRect(view.Frame.X, view.Frame.Y - heightAdjustDiff,
													  view.Frame.Width, view.Frame.Height);
				}
			}, () =>
			{
				Console.WriteLine("New TextView height is {0}, y is {1}", textView.Frame.Height, textView.Frame.Y);
			});
		}
	}
}
