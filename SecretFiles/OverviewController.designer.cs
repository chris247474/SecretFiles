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
    [Register ("OverviewController")]
    partial class OverviewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIScrollView CurrentSecretsScrollView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel InvitedLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView LoadingIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NewSecretButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView Overview { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchDisplayController searchDisplayController { get; set; }

        [Action ("NewSecretButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void NewSecretButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CurrentSecretsScrollView != null) {
                CurrentSecretsScrollView.Dispose ();
                CurrentSecretsScrollView = null;
            }

            if (InvitedLabel != null) {
                InvitedLabel.Dispose ();
                InvitedLabel = null;
            }

            if (LoadingIndicatorView != null) {
                LoadingIndicatorView.Dispose ();
                LoadingIndicatorView = null;
            }

            if (NewSecretButton != null) {
                NewSecretButton.Dispose ();
                NewSecretButton = null;
            }

            if (Overview != null) {
                Overview.Dispose ();
                Overview = null;
            }

            if (searchDisplayController != null) {
                searchDisplayController.Dispose ();
                searchDisplayController = null;
            }
        }
    }
}