using Foundation;
using System;
using UIKit;
using ObjCRuntime;

namespace SecretFiles
{
    public partial class SecretFileView : UIView
    {
		SecretFileItem SecretFile;

        public SecretFileView (IntPtr handle) : base (handle)
        {
        }

		public void SetSecretFileInfo(SecretFileItem secretFile)
		{
			SecretFile = secretFile;
			BackgroundImageView.BackgroundColor = UIColor.Clear;
			this.BackgroundColor = UIColor.Green;
		}

		public static SecretFileView Create()
		{
			var arr = NSBundle.MainBundle.LoadNib("SecretFileView", null, null);
			var v = Runtime.GetNSObject<SecretFileView>(arr.ValueAt(0));
			return v;
		}

		public override void AwakeFromNib()
		{
			Console.WriteLine("AwakeFromNib");
			if (SecretFile != null)
			{
				TitleLabel.Text = SecretFile.Title;
				DescriptionLabel.Text = SecretFile.Description;
			}
			else Console.WriteLine("SecretFileItem still null");
		}
    }
}