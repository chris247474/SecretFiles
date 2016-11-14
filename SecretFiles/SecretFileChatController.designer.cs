// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SecretFiles
{
    [Register ("SecretFileChatController")]
    partial class SecretFileChatController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CameraButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ChatBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView ChatScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GalleryButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SendButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SmileyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView TextField { get; set; }

        [Action ("GalleryButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void GalleryButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("SendButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SendButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CameraButton != null) {
                CameraButton.Dispose ();
                CameraButton = null;
            }

            if (ChatBackground != null) {
                ChatBackground.Dispose ();
                ChatBackground = null;
            }

            if (ChatScrollView != null) {
                ChatScrollView.Dispose ();
                ChatScrollView = null;
            }

            if (GalleryButton != null) {
                GalleryButton.Dispose ();
                GalleryButton = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (SmileyButton != null) {
                SmileyButton.Dispose ();
                SmileyButton = null;
            }

            if (TextField != null) {
                TextField.Dispose ();
                TextField = null;
            }
        }
    }
}