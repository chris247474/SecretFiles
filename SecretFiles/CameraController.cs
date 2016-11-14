using System;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using UIKit;

namespace SecretFiles
{
	public partial class CameraController : UIViewController
	{
		bool flashOn = false;

		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;
		AVCaptureVideoPreviewLayer videoPreviewLayer;

		public CameraController(IntPtr handle) : base(handle)
		{
		}

		public override async void ViewDidLoad()
		{
			base.ViewDidLoad();

			await AuthorizeCameraUse();
			SetupLiveCameraStream();

			AddSwipeNavigationGestures();
		}

		void AddSwipeNavigationGestures() { 
			var SwipeToOverviewGesture = new UISwipeGestureRecognizer(() =>
		   {
				//slide overview screen from right side
				var topVC = iOSNavigationHelper.GetTopUIViewController();
			    var OverviewController = topVC.Storyboard.InstantiateViewController("OverviewController") as OverviewController;
			   //OverviewController.View.BackgroundColor = UIColor.Clear;
				this.PresentViewController(OverviewController, true, () => { });
			});
			SwipeToOverviewGesture.Direction = UISwipeGestureRecognizerDirection.Left;
			View.AddGestureRecognizer(SwipeToOverviewGesture);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
		}

		partial void GalleryButton_TouchUpInside(UIButton sender)
		{
			PhotoPickerService.ChoosePicture();
		}

		async partial void TakePhotoButton_TouchUpInside(UIButton sender)
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
			var jpegAsByteArray = jpegImageAsNsData.ToArray();
		}

		partial void SwitchCameraButton_TouchUpInside(UIButton sender)
		{
			var devicePosition = captureDeviceInput.Device.Position;
			if (devicePosition == AVCaptureDevicePosition.Front)
			{
				devicePosition = AVCaptureDevicePosition.Back;
			}
			else {
				devicePosition = AVCaptureDevicePosition.Front;
			}

			var device = GetCameraForOrientation(devicePosition);
			ConfigureCameraForDevice(device);

			captureSession.BeginConfiguration();
			captureSession.RemoveInput(captureDeviceInput);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
			captureSession.AddInput(captureDeviceInput);
			captureSession.CommitConfiguration();
		}

		partial void FlashButton_TouchUpInside(UIButton sender)
		{
			var device = captureDeviceInput.Device;

			var error = new NSError();
			if (device.HasFlash)
			{
				if (device.FlashMode == AVCaptureFlashMode.On)
				{
					device.LockForConfiguration(out error);
					device.FlashMode = AVCaptureFlashMode.Off;
					device.UnlockForConfiguration();

					flashButton.SetBackgroundImage(UIImage.FromBundle("flashoff.png"), UIControlState.Normal);
				}
				else {
					device.LockForConfiguration(out error);
					device.FlashMode = AVCaptureFlashMode.On;
					device.UnlockForConfiguration();

					flashButton.SetBackgroundImage(UIImage.FromBundle("flashon.png"), UIControlState.Normal);
				}
			}

			flashOn = !flashOn;
		}

		public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}

			return null;
		}



		async Task AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}

		public void SetupLiveCameraStream()
		{
			captureSession = new AVCaptureSession();

			var viewLayer = liveCameraStreamView.Layer;
			videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
			{
				Frame = this.View.Frame
			};
			liveCameraStreamView.Layer.AddSublayer(videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			ConfigureCameraForDevice(captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
			captureSession.AddInput(captureDeviceInput);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};

			captureSession.AddOutput(stillImageOutput);
			captureSession.StartRunning();

			BringAllToFont();
		}
		void BringAllToFont() { 
			liveCameraStreamView.BringSubviewToFront(TakePhotoButton);
			liveCameraStreamView.BringSubviewToFront(flashButton);
			liveCameraStreamView.BringSubviewToFront(GalleryButton);
			liveCameraStreamView.BringSubviewToFront(SwitchCameraButton);
			liveCameraStreamView.BringSubviewToFront(AudioButton);
		}
		void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}
	}
}
