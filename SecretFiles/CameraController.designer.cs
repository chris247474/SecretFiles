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
    [Register ("ViewController")]
    partial class CameraController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AudioButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton flashButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GalleryButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView liveCameraStreamView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SwitchCameraButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TakePhotoButton { get; set; }

        [Action ("FlashButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void FlashButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("SwitchCameraButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SwitchCameraButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("TakePhotoButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void TakePhotoButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AudioButton != null) {
                AudioButton.Dispose ();
                AudioButton = null;
            }

            if (flashButton != null) {
                flashButton.Dispose ();
                flashButton = null;
            }

            if (GalleryButton != null) {
                GalleryButton.Dispose ();
                GalleryButton = null;
            }

            if (liveCameraStreamView != null) {
                liveCameraStreamView.Dispose ();
                liveCameraStreamView = null;
            }

            if (SwitchCameraButton != null) {
                SwitchCameraButton.Dispose ();
                SwitchCameraButton = null;
            }

            if (TakePhotoButton != null) {
                TakePhotoButton.Dispose ();
                TakePhotoButton = null;
            }
        }
    }
}