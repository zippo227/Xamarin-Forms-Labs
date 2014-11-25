using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if __UNIFIED__
using Foundation;
using UIKit;
using AVFoundation;
using CoreGraphics;
#elif __IOS__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.AVFoundation;
#endif
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Drawing;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class CameraViewRenderer : ViewRenderer<CameraView, CameraPreview>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            System.Diagnostics.Debug.WriteLine("Testing CameraView");
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                this.SetNativeControl(new CameraPreview());
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }
    }

    [Register("CameraPreview")]
    public class CameraPreview : UIView
    {
        private AVCaptureVideoPreviewLayer previewLayer;

        public CameraPreview()
        {
            Initialize();
        }

        public CameraPreview(RectangleF bounds)
            : base(bounds)
        {
            Initialize();
        }

#if __UNIFIED__
        public override void Draw(CGRect rect)
#elif __IOS__
        public override void Draw(RectangleF rect)
#endif
        {
            base.Draw(rect);
            this.previewLayer.Frame = rect;
        }

        private void Initialize()
        {
            var captureSession = new AVCaptureSession();
            previewLayer = new AVCaptureVideoPreviewLayer(captureSession)
                {
#if __UNIFIED__
                    VideoGravity = AVLayerVideoGravity.ResizeAspectFill,
#elif __IOS__
                    LayerVideoGravity = AVLayerVideoGravity.ResizeAspectFill,
#endif
                    Frame = this.Bounds
                };

            var device = AVCaptureDevice.DefaultDeviceWithMediaType(
                AVMediaType.Video);

            if (device == null)
            {
                System.Diagnostics.Debug.WriteLine("No device detected.");
                return;
            }

            NSError error;

            var input = new AVCaptureDeviceInput(device, out error);

            captureSession.AddInput(input);

            this.Layer.AddSublayer(previewLayer);

            captureSession.StartRunning();
        }
    }
}