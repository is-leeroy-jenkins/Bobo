

namespace Bobo
{
    using Microsoft.ML.Data;
    using OpenCvSharp;
    using System.Drawing;
    using Tensorflow;
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class CharacterData
    {
        [LoadColumn(0)]
        public int CharacterId { get; set; }

        [LoadColumn(1, 64), VectorType(64)]
        public float[] Pixels { get; set; }

        public static float[] GetData(byte[] data)
        {
            var _result = new float[64];
            var _total = 0;
            for (var _x = 0; _x < 21; _x++)
            {
                for (var _y = 0; _y < 21; _y++)
                {
                    if (data[(_y * 21) + _x] < 240)
                    {
                        _total++;
                        var _x1 = _x / 3;
                        var _y1 = _y / 3;
                        _result[(_y1 * 7) + _x1] += 1;
                        var _x2 = _x / 7;
                        var _y2 = _y / 7;
                        _result[49 + (_y2 * 3) + _x2] += 1;
                        var _x3 = _x > 10 ? 1 : 0;
                        var _y3 = _y > 10 ? 1 : 0;
                        if(_x != 10 && _y != 10)
                        {
                            _result[58 + (_y3 * 2) + _x3] += 1;
                        }
                    }
                }
            }

            for(var _i = 0; _i < 49; _i++)
            {
                var _val = (int)(_result[_i] + 1) / 3;
                _result[_i] = MathF.Ceiling(_val);
            }

            for(var _i = 49; _i < 58; _i++)
            {
                var _val = (int)_result[_i] / 5;
                _result[_i] = MathF.Ceiling(_val);
            }

            for(var _i = 58; _i < 62; _i++)
            {
                _result[_i] = MathF.Ceiling(100 * _result[_i] / _total);
            }

            _result[62] = _total;
            _result[63] = 441 - _total;
            return _result;
        }
    }
}

