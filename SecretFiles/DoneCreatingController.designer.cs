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
    [Register ("DoneCreatingController")]
    partial class DoneCreatingController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CopyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DoneLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShareButton { get; set; }

        [Action ("CopyButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CopyButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("ShareButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void ShareButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CopyButton != null) {
                CopyButton.Dispose ();
                CopyButton = null;
            }

            if (DoneLabel != null) {
                DoneLabel.Dispose ();
                DoneLabel = null;
            }

            if (ShareButton != null) {
                ShareButton.Dispose ();
                ShareButton = null;
            }
        }
    }
}