using Android.Content;
using graphics = Android.Graphics;
using Chroomsoft.Top2000.Apps.TrackInformation;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;

namespace Top2000.Platforms.Android.TrackInformation
{
    public class ProgressRingRenderer : ViewRenderer
    {
        private graphics.Paint? _paint;
        private graphics.RectF? _ringDrawArea;
        private bool _sizeChanged;

        public ProgressRingRenderer(Context context) : base(context)
        {
            SetWillNotDraw(false);
        }

        protected override void OnDraw(graphics.Canvas? canvas)
        {
            if (canvas is null) return;

            var progressRing = (ProgressRing)Element;

            if (_paint == null)
            {
                var displayDensity = Context?.Resources?.DisplayMetrics?.Density;

                if (displayDensity != null)
                {
                    var strokeWidth = (float)Math.Ceiling(progressRing.RingThickness * displayDensity.Value);

                    _paint = new graphics.Paint();
                    _paint.StrokeWidth = strokeWidth;
                    _paint.SetStyle(graphics.Paint.Style.Stroke);
                    _paint.Flags = graphics.PaintFlags.AntiAlias;
                }
            }

            if ((_ringDrawArea == null || _sizeChanged) && _paint != null)
            {
                _sizeChanged = false;

                var ringAreaSize = Math.Min(canvas.ClipBounds.Width(), canvas.ClipBounds.Height());

                var ringDiameter = ringAreaSize - _paint.StrokeWidth;

                var left = canvas.ClipBounds.CenterX() - ringDiameter / 2;
                var top = canvas.ClipBounds.CenterY() - ringDiameter / 2;

                _ringDrawArea = new RectF(left, top, left + ringDiameter, top + ringDiameter);
            }

            var backColor = progressRing.RingBaseColor;
            var frontColor = progressRing.RingProgressColor;
            var progress = (float)progressRing.Progress;
            DrawProgressRing(canvas, progress, backColor, frontColor);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName ||
                e.PropertyName == ProgressRing.RingThicknessProperty.PropertyName ||
                e.PropertyName == ProgressRing.RingBaseColorProperty.PropertyName ||
                e.PropertyName == ProgressRing.RingProgressColorProperty.PropertyName)
            {
                Invalidate();
            }

            if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                e.PropertyName == VisualElement.HeightProperty.PropertyName)
            {
                _sizeChanged = true;
                Invalidate();
            }
        }

        private void DrawProgressRing(graphics.Canvas canvas, float progress,
                                      Color ringBaseColor,
                                      Color ringProgressColor)
        {
            if (_paint != null && _ringDrawArea != null)
            {
                _paint.Color = ringBaseColor.ToAndroid();
                canvas.DrawArc(_ringDrawArea, 270, 360, false, _paint);

                _paint.Color = ringProgressColor.ToAndroid();
                canvas.DrawArc(_ringDrawArea, 270, 360 * progress, false, _paint);
            }
        }
    }

}
