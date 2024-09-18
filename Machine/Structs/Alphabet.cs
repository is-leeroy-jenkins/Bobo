

namespace Bobo
{
    using OpenCvSharp;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    public readonly record struct Alphabet(string FontName, char Character, DrawStyle DrawStyle, double Rotation)
    {
        public const int RENDER_SIZE = 21;
        public const int CHAR_OFF_SET = 100;
        public int CharacterId
        {
            get
            {
                return CharacterMapper.GetCharacterId( Character );
            }
        }

        public Mat Print()
        {
            if (Character == ' ')
            {
                return new Mat(new Size(21, 21), MatType.CV_8UC1, new Scalar(255, 255, 255, 255));
            }

            try
            {
                var _data = DrawImage();

                using var _gray = Cv2.ImDecode(_data, ImreadModes.Grayscale);
                using var _threshold = new Mat( );

                Cv2.Threshold(_gray, _threshold, 200, 255, ThresholdTypes.Binary);

                var _rect = GetBoundingBox(_threshold);

                using var _cropped = GetCropped(_threshold, _rect);

                var _final = new Mat( RENDER_SIZE, RENDER_SIZE, MatType.CV_8UC1, new Scalar( 255 ) );

                var _xoffset = (RENDER_SIZE - _cropped.Width) / 2;
                var _yoffset = (RENDER_SIZE - _cropped.Height) / 2;

                var _roi = new Rect( _xoffset, _yoffset, _cropped.Width, _cropped.Height );
                using var _region = _final[_roi];
                _cropped.CopyTo(_region);

                return _final;
            }
            catch (Exception)
            {
                return new Alphabet("Arial", Character, DrawStyle, Rotation).Print();
            }
        }

        private static Rect GetBoundingBox(Mat threshold)
        {
            Cv2.FindContours(threshold, out var _contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxNone);
            return Cv2.BoundingRect(_contours.OrderByDescending(x => GetArea(x)).Skip(1).First());
        
            static double GetArea(Point[] contour)
            {
                return Cv2.ContourArea( contour );
            }
        }

        private static Mat GetCropped(Mat threshold, Rect rect)
        {
            using var _cropped = new Mat( threshold, rect );
            var _ratio = RENDER_SIZE / (double)Math.Max(rect.Width, rect.Height);
            var _output = new Mat( );
            Cv2.Resize(_cropped, _output, new Size(_cropped.Width * _ratio, _cropped.Height * _ratio), _ratio, _ratio, InterpolationFlags.Cubic);
            return _output;
        }

        private byte[] DrawImage()
        {

            var _visual = new DrawingVisual( );
            using var _drawing = _visual.RenderOpen();

            var _style = DrawStyle switch
            {
                DrawStyle.Normal or DrawStyle.Bold => System.Windows.FontStyles.Normal,
                _ => System.Windows.FontStyles.Italic,
            };
            var _weight = DrawStyle switch
            {
                DrawStyle.Normal or DrawStyle.Italic => System.Windows.FontWeights.Normal,
                _ => System.Windows.FontWeights.Bold,
            };

            var _text = new FormattedText( textToFormat: $"{Character}",
                culture: CultureInfo.GetCultureInfo( "en-us" ),
                flowDirection: System.Windows.FlowDirection.LeftToRight,
                typeface: new Typeface( new FontFamily( FontName ), _style, _weight, default ), 48,
                Brushes.Black, 96 )
            {
                TextAlignment = System.Windows.TextAlignment.Center,
            };

            _drawing.PushTransform(new RotateTransform(Rotation, 48, 48));
            _drawing.DrawRectangle(Brushes.White, null, new System.Windows.Rect(0, 0, 96, 96));
        
            _drawing.DrawText(_text, new System.Windows.Point(48, 24));
            _drawing.Close();


            var _renderTargetBitmap = new RenderTargetBitmap( 96, 96, 96, 96,
                PixelFormats.Pbgra32 );
            _renderTargetBitmap.Render(_visual);

            var _png = new BmpBitmapEncoder( );
            _png.Frames.Add(BitmapFrame.Create(_renderTargetBitmap));
            using var _stream = new MemoryStream( );
            _png.Save(_stream);

            return _stream.ToArray();
        }
    }
}

