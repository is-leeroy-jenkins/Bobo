
namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using OpenCvSharp;
    using System.Data;
    using System.IO;
    using System.Runtime.ConstrainedExecution;
    using System.Text;
    using System.Windows.Shapes;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Media;
    using System.Windows;
    using Color = System.Drawing.Color;
    using FontStyle = System.Windows.FontStyle;
    using Matrix = System.Drawing.Drawing2D.Matrix;
    using Point = System.Drawing.Point;
    using Rect = OpenCvSharp.Rect;

    /// <summary>
    /// 
    /// </summary>
    public static class ImageDataBuilder
    {
        /// <summary>
        /// The size
        /// </summary>
        public const int SIZE = 32;

        /// <summary>
        /// The alpha numeric
        /// </summary>
        public const string ALPHA_NUMERIC = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static IEnumerable<OpticalCharacterData> GetImageData(string path, int[] rows)
        {
            foreach (var _file in Directory.GetFiles(path, "*.jpg").OrderBy(x => Random.Shared.Next()))
            {
                using var _glyphs = Cv2.ImRead(_file, ImreadModes.Grayscale);
                using var _threshold = new Mat( );
                Cv2.Threshold(_glyphs, _threshold, 200, 255, ThresholdTypes.Binary);

                for (var _charIndex = 0; _charIndex < ALPHA_NUMERIC.Length; _charIndex++)
                {
                    var _cordX = _charIndex * SIZE;
                    var _character = ALPHA_NUMERIC[_charIndex];

                    for (var _rowIndex = 0; _rowIndex < rows.Length; _rowIndex++)
                    {
                        var _cordY = _rowIndex * SIZE;

                        var _roi = new Rect( _cordX, _cordY, SIZE, SIZE );
                        using var _charMat = _threshold[_roi];
                        using var _charMatResized = new Mat( );
                        Cv2.Resize(_charMat, _charMatResized, new OpenCvSharp.Size(16, 16), 0.5d, 0.5d, InterpolationFlags.LinearExact);
                        using var _charThreshold = new Mat( );
                        Cv2.Threshold(_charMatResized, _charThreshold, 200, 255, ThresholdTypes.Binary);

                        var _data = _charMat.ToBytes();

                        yield return new OpticalCharacterData
                        {
                            Character = (_character - '0'),
                            PixelValues = _data,
                        };
                    }
                }
            }
        }

        public static void WriteAsCsv(string path, StreamWriter writer, int[] rows)
        {
            foreach (var _file in Directory.GetFiles(path, "*.jpg"))
            {
                using var _glyphs = Cv2.ImRead(_file, ImreadModes.Grayscale);
                using var _threshold = new Mat( );
                Cv2.Threshold(_glyphs, _threshold, 200, 255, ThresholdTypes.Binary);

                for (var _charIndex = 0; _charIndex < ALPHA_NUMERIC.Length; _charIndex++)
                {
                    var _cordX = _charIndex * SIZE;
                    var _character = ALPHA_NUMERIC[_charIndex];

                    for (var _rowIndex = 0; _rowIndex < rows.Length; _rowIndex++)
                    {
                        var _cordY = _rowIndex * SIZE;

                        var _roi = new Rect( _cordX, _cordY, SIZE, SIZE );
                        using var _charMat = _threshold[_roi];
                        using var _charMatResized = new Mat( );
                        Cv2.Resize(_charMat, _charMatResized, new OpenCvSharp.Size(16, 16), 0.5d, 0.5d, InterpolationFlags.LinearExact);
                        using var _charThreshold = new Mat( );
                        Cv2.Threshold(_charMatResized, _charThreshold, 200, 255, ThresholdTypes.Binary);

                        var _success = _charMat.GetArray(out byte[] _charBytes);

                        if (_success == true)
                        {
                            writer.Write($"{(int)(_character - '0')},");

                            for (var _i = 0; _i < _charBytes.Length; _i++)
                            {
                                var _bit = _charBytes[_i];
                                if (_bit == 0 || _bit == 255)
                                {
                                    writer.Write(_bit == 255 ? "0" : "1");
                                    if (_i != _charBytes.Length - 1)
                                    {
                                        writer.Write(',');
                                    }
                                }
                            }

                            writer.WriteLine();
                        }
                    }
                }
            }
        }

        public static Mat GetFontMat(string fontname)
        {
            var _styles = new FontStyle[] { FontStyles.Normal, FontStyles.Italic, FontStyles.Oblique };
            var _angles = new int[] { -15, -10, -5, 0, 5, 10, 15 };
            var _chars = ALPHA_NUMERIC.ToCharArray( );
            var _width = _chars.Length * SIZE;
            var _height = _styles.Length * _angles.Length * SIZE;
            var _fontMat = new Mat( _height, _width, MatType.CV_8UC1, new Scalar( 255 ) );
            for (var _charIndex = 0; _charIndex < _chars.Length; _charIndex++)
            {
                for (var _styleIndex = 0; _styleIndex < _styles.Length; _styleIndex++)
                {
                    for (var _angleIndex = 0; _angleIndex < _angles.Length; _angleIndex++)
                    {
                        var _character = _chars[_charIndex];
                        var _style = _styles[_styleIndex];
                        var _angle = _angles[_angleIndex];
                        var _y = (_angles.Length * _styleIndex + _angleIndex) * SIZE;
                        var _x = _charIndex * SIZE;
                        var _roi = new Rect( _x, _y, SIZE, SIZE );
                        using var _region = _fontMat[_roi];
                        using var _charMat = GetCharactorMat(_character, fontname, _style, _angle);
                        _charMat.CopyTo(_region);
                    }
                }
            }

            return _fontMat;
        }

        public static Mat GetCharactorMat(char character, string fontName, FontStyle style, int rotation)
        {
            using var _gray = Mat.FromImageData(PrintCharactor(character, fontName, style, rotation), ImreadModes.Grayscale);
            using var _threshold = new Mat( );
            Cv2.Threshold(_gray, _threshold, 200, 255, ThresholdTypes.Binary);

            var _rect = GetBoundingBox(_threshold);

            using var _cropped = GetCropped(_threshold, _rect);

            var _final = new Mat( SIZE, SIZE, MatType.CV_8UC1, new Scalar( 255 ) );

            var _xoffset = (SIZE - _cropped.Width) / 2;
            var _yoffset = (SIZE - _cropped.Height) / 2;

            var _roi = new Rect( _xoffset, _yoffset, _cropped.Width, _cropped.Height );
            using var _region = _final[_roi];
            _cropped.CopyTo(_region);

            return _final;
        }

        private static Mat GetCropped(Mat threshold, Rect rect)
        {
            using var _cropped = new Mat( threshold, rect );
            var _ratio = SIZE / (double)Math.Max(rect.Width, rect.Height);
            var _output = new Mat( );
            Cv2.Resize(_cropped, _output, new OpenCvSharp.Size(_cropped.Width * _ratio, _cropped.Height * _ratio), _ratio, _ratio, InterpolationFlags.Cubic);
            return _output;
        }

        private static Rect GetBoundingBox(Mat threshold)
        {
            Cv2.FindContours(threshold, out var _contours, out _, RetrievalModes.List, ContourApproximationModes.ApproxTC89KCOS);
            return Cv2.BoundingRect(_contours.OrderByDescending(x => GetArea(x)).Skip(1).First());
            static double GetArea(OpenCvSharp.Point[] contour)
            {
                return Cv2.ContourArea( contour );
            }
        }

        public static byte[] PrintCharactor(char character, string fontName, FontStyle style, int rotation)
        {
            using Font _font = new Font( fontName, 48 );
            using var _characterBitmap = new Bitmap( 144, 144 );
            using var _characterGraphics = Graphics.FromImage(_characterBitmap);

            _characterGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            _characterGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _characterGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            _characterGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            _characterGraphics.RotateTransform(rotation);
            _characterGraphics.Clear(Color.White);

            var _offset = GetOffset(rotation);
            _characterGraphics.DrawString(character.ToString(), _font, new SolidBrush(Color.Black), _offset.X, _offset.Y);
            _characterGraphics.Flush();
            using var _stream = new MemoryStream( );

            _characterBitmap.Save(_stream, System.Drawing.Imaging.ImageFormat.Png);

            return _stream.ToArray();
        }

        private static System.Drawing.Point GetOffset(int rotation)
        {
            var _point = new Point( 72, 72 );
            var _matrix = new Matrix( );
            _matrix.Rotate(-rotation);
            var _offsets = new System.Drawing.Point[1] { _point };
            _matrix.TransformPoints(_offsets);
            return new System.Drawing.Point(_offsets[0].X - 36, _offsets[0].Y - 36);
        }

        public static IEnumerable<string> GetFontNames(string culture = "en-us")
        {
            foreach (var _fontfamily in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                foreach (var _font in _fontfamily.FamilyNames
                    .Where(x => x.Key.ToString().Trim().ToLower() == culture)
                    .Select(x => x.Value).Distinct())
                {
                    yield return _font;
                }
            }
        }


    }
}