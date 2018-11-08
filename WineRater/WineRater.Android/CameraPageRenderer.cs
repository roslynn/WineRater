using Android.Graphics;
using Android.Views;
using Android.Widget;
using System.Linq;
using System;
using Xamarin.Forms.Platform.Android;
using WineRater;
using WineRater.Droid;
using Camera = Android.Hardware.Camera;

///<summary>
/// Concept inspired by Antonio Feregrino from the post 
/// Full page camera in Xamarin.Forms 08/11/2016
/// post source:  https://thatcsharpguy.com/post/full-camera-page.1/
/// source code:  https://github.com/messier16/FullCameraPage
///</summary>
[assembly: Xamarin.Forms.ExportRenderer(typeof(ExamineWinePage), typeof(CameraPageRenderer))]
namespace WineRater.Droid
{
  public class CameraPageRenderer : PageRenderer, TextureView.ISurfaceTextureListener
  {
    private Camera _camera = null;
    private TextureView _liveView;
    RelativeLayout _mainLayout;
    Button _capturePhotoButton;

    public CameraPageRenderer() { }

    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e)
    {
      base.OnElementChanged(e);
      SetupUserInterface();
      SetupEventHandlers();
    }

    private void SetupUserInterface()
    {
      _mainLayout = new RelativeLayout(Context);
      RelativeLayout.LayoutParams mainLayoutParams = new RelativeLayout.LayoutParams(
          RelativeLayout.LayoutParams.MatchParent,
          RelativeLayout.LayoutParams.MatchParent);
      _mainLayout.LayoutParameters = mainLayoutParams;

      _liveView = new TextureView(Context);
      RelativeLayout.LayoutParams liveViewParams = new RelativeLayout.LayoutParams(
          RelativeLayout.LayoutParams.MatchParent,
          RelativeLayout.LayoutParams.MatchParent);
      _liveView.LayoutParameters = liveViewParams;
      _mainLayout.AddView(_liveView);

      _capturePhotoButton = new Button(Context);
      _capturePhotoButton.SetBackgroundResource(Resource.Drawable.camera_round_btn_bg);
      _mainLayout.AddView(_capturePhotoButton);

      AddView(_mainLayout);
    }
    private void SetupEventHandlers()
    {
      _capturePhotoButton.Click += async (sender, e) =>
      {
        var bytes = await TakePhotoAsync();
        (Element as ExamineWinePage).SetPhotoResult(bytes, _liveView.Bitmap.Width, _liveView.Bitmap.Height);
      };
      _liveView.SurfaceTextureListener = this;
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
      base.OnLayout(changed, l, t, r, b);
      if (!changed)
        return;

      var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
      var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);
      _mainLayout.Measure(msw, msh);
      _mainLayout.Layout(0, 0, r - l, b - t);

      _capturePhotoButton.SetX(_mainLayout.Width / 2 - _capturePhotoButton.Width / 2);
      _capturePhotoButton.SetY(_mainLayout.Height - _capturePhotoButton.Height - 200/*offset*/);
    }

    public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
    {
      if (keyCode == Keycode.Back)
      {
        (Element as ExamineWinePage).Cancel();
        return false;
      }
      return base.OnKeyDown(keyCode, e);
    }

    public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
    {
      StopCamera();
      return true;
    }

    public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
    {
    }

    public void OnSurfaceTextureUpdated(SurfaceTexture surface)
    {
    }

    public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
    {
      _camera = Camera.Open(FindBackFacingCamera());

      //_textureView.LayoutParameters = new FrameLayout.LayoutParams(width, WallpaperDesiredMinimumHeight);
      var parameters = _camera.GetParameters();
      try
      {
        var aspect = ((decimal)height) / ((decimal)width);

        var previewSize = parameters.SupportedPreviewSizes
                                    .OrderBy(s => Math.Abs(s.Width / (decimal)s.Height - aspect))
                                    .First();

        parameters.SetPreviewSize(previewSize.Width, previewSize.Height);
        parameters.FocusMode = Camera.Parameters.FocusModeContinuousPicture;
        _camera.SetParameters(parameters);

        _camera.SetPreviewTexture(surface);
        StartCamera();
      }
      catch (Java.IO.IOException ex)
      {
        // todo handle error here
        System.Diagnostics.Debug.WriteLine(ex);
      }
    }

    private int FindBackFacingCamera()
    {
      var cameraId = 0;
      // Search for the front facing camera
      int numberOfCameras = Camera.NumberOfCameras;
      for (int i = 0; i < numberOfCameras; i++)
      {
        var info = new Camera.CameraInfo();
        Camera.GetCameraInfo(i, info);
        if (info.Facing == Camera.CameraInfo.CameraFacingBack)
        {
          cameraId = i;
          break;
        }
      }
      return cameraId;
    }

    private void StartCamera()
    {
      _camera?.SetDisplayOrientation(90);
      _camera?.StartPreview();
    }

    private void StopCamera()
    {
      if (_camera != null)
      {
        _camera.StopPreview();
        _camera.Release();
        _camera = null;
      }
    }

    private async System.Threading.Tasks.Task<byte[]> TakePhotoAsync()
    {
      _camera.StopPreview();
      var ratio = ((decimal)Height) / Width;
      var image = Bitmap.CreateBitmap(_liveView.Bitmap, 0, 0, _liveView.Bitmap.Width, (int)(_liveView.Bitmap.Width * ratio));
      byte[] imageBytes = null;
      using (var imageStream = new System.IO.MemoryStream())
      {
        await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, imageStream);
        image.Recycle();
        imageBytes = imageStream.ToArray();
      }
      _camera.StartPreview();
      return imageBytes;
    }

  }
}