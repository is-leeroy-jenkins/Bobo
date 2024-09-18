

namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using Microsoft.ML;
    using Microsoft.ML.Data;
    using Microsoft.ML.Transforms;
    using Microsoft.ML.Transforms.Onnx;
    using OpenCvSharp;
    using Tensorflow;
    using Microsoft.ML.Trainers.LightGbm;
    using Microsoft.ML.Vision;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class Trainer : ITrainer
    {
        private const int RETRAIN_FACTOR = 1;

        private static readonly Dictionary<char, int> _minPixelsDictionary =
            new Dictionary<char, int>( );

        private static readonly Dictionary<char, int> _maxPixelsDictionary =
            new Dictionary<char, int>( );

        public void LoadModel(string path, EngineType engine)
        {
            Directory.CreateDirectory("models");
            File.Copy(path, $"models\\{engine}.onnx", true);
        }

        public void SaveModel(string path, EngineType engine)
        {
            Directory.CreateDirectory(Path.GetFullPath(path));
            File.Copy($"models\\{engine}.onnx", path);
        }

        public void Delete(EngineType engine)
        {
            File.Delete( $"models\\{engine}.onnx" );
        }

        public void Train(IEnumerable<FontSetting> settings, EngineType engine, ITrainingProgress progress)
        {
            progress.Current = 0;
            progress.Maximum = RETRAIN_FACTOR * settings.Sum(x => x.CharCount);

            var _context = new MLContext( seed: null );

            var _data = engine switch
            {
                EngineType.ImageClassification => _context.Data.LoadFromEnumerable(GetOpticalTrainData(settings, progress)),
                _ => _context.Data.LoadFromEnumerable(GetTrainData(settings, progress)),
            };

            var _mapingPipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "CharacterId", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", nameof(CharacterData.Pixels)))
                .AppendCacheCheckpoint(_context);

            var _options = new LightGbmMulticlassTrainer.Options
            {
                LearningRate = 0.45,
                UseSoftmax = true,
                UseZeroAsMissingValue = true,
            };


            var _pipeline = engine switch
            {
                EngineType.SdcaNonCalibrated => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.SdcaNonCalibrated("Label", "Features")).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                EngineType.SdcaMaximumEntropy => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features")).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                EngineType.LbfgsMaximumEntropy => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features")).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                EngineType.NaiveBayes => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.NaiveBayes("Label", "Features")).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                EngineType.ImageClassification => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.ImageClassification("Label", "Features"))
                    .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                EngineType.LightGbm => _mapingPipeline.Append(_context.MulticlassClassification.Trainers.LightGbm()).Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel")),
                _ => throw new NotImplementedException(),
            }; 
            var _model = _pipeline.Fit(_data);
            Directory.CreateDirectory("models");
            _context.Model.Save(_model, _data.Schema, $"models\\{engine}.onnx");

            var _mapping = new StringBuilder( );
            foreach (var _key in _minPixelsDictionary.Keys)
            {
                _mapping.Append(_key);
                _mapping.Append('\t');
                _mapping.Append(_minPixelsDictionary[_key]);
                _mapping.Append('\t');
                _mapping.Append(_maxPixelsDictionary.TryGetValue(_key, out var _value) ? _value : '-');
                _mapping.AppendLine();
            }
            File.WriteAllText($"models\\{engine}.txt", _mapping.ToString());
        }

        public void EvaluateAccuracy(FontSetting setting, EngineType engine, IAccuracyProgress progress)
        {
            progress.Current = 0;
            progress.Maximum = setting.CharCount;

            var _context = new MLContext( seed: null );

            var _model = _context.Model.Load($"models\\{engine}.onnx", out var _columns);
            var _characterDictionary = GetCharDictionary($"models\\{engine}.onnx");
            var _correct = 0;
            var _total = 0;
            var _correctness = 0f;
            if (engine == EngineType.ImageClassification)
            {
                using var _predictionEngine = _context.Model.CreatePredictionEngine<CharacterOpticalData, PredictionData>(_model, _columns);

                foreach (var _character in GetOpticalTrainData(setting, progress))
                {
                    _total++;
                    var _prediction = _predictionEngine.Predict(_character);
                    UpdateCorrectness(_prediction.Score, _character.CharacterId);
                }
            }
            else
            {
                using var _predictionEngine = _context.Model.CreatePredictionEngine<CharacterData, PredictionData>(_model, _columns);
                foreach (var _character in GetTrainData(setting, progress))
                {
                    _total++;
                    var _prediction = _predictionEngine.Predict(_character);
                    UpdateCorrectness(_prediction.Score, _character.CharacterId);
                }
            }

            progress.Accuracy = Convert.ToSingle(_correct) / Convert.ToSingle(_total);
            progress.AverageScore = _correctness / Convert.ToSingle(_total);

            void UpdateCorrectness(float[] scores, int actual)
            {
                var _scoreDictionary = new Dictionary<int, float>( );
                for (var _i = 0; _i < scores.Length; _i++)
                {
                    var _charId = _characterDictionary[_i];
                    if (_scoreDictionary.ContainsKey(_charId) == false)
                    {
                        _scoreDictionary[_charId] = 0;
                    }

                    _scoreDictionary[_charId] += scores[_i];
                }

                var _id = actual;
                var _toatalscore = scores.Where(x => x > 0f).Sum();
                if (_scoreDictionary.ContainsKey(_id))
                {
                    if (_scoreDictionary[_id] > 0.25 * _toatalscore)
                    {
                        _correct++;
                        _correctness += _scoreDictionary[_id] / _toatalscore;
                    }
                }
            }
        }

        public void Evaluate(FontSetting setting, EngineType engine, ITestProgress progress)
        {
            progress.Current = 0;
            progress.Maximum = setting.CharCount;
            var _context = new MLContext( seed: null );
            var _data = engine switch
            {
                EngineType.ImageClassification => _context.Data.LoadFromEnumerable(GetOpticalTrainData(setting, progress)),
                _ => _context.Data.LoadFromEnumerable(GetTrainData(setting, progress)),
            };

            var _model = _context.Model.Load($"models\\{engine}.onnx", out var _columns);
            if (engine == EngineType.ImageClassification)
            {
                using var _predictionEngine = _context.Model.CreatePredictionEngine<CharacterOpticalData, PredictionData>(_model, _columns);
                var _metric = _context.MulticlassClassification.Evaluate(_model.Transform(_data));
                progress.IsEvaluated = true;
                progress.LogLoss = _metric.LogLoss;
                progress.LogLossReduction = _metric.LogLossReduction;
                progress.MicroAccuracy = _metric.MicroAccuracy;
                progress.MacroAccuracy = _metric.MacroAccuracy;
            }
            else
            {
                using var _predictionEngine = _context.Model.CreatePredictionEngine<CharacterData, PredictionData>(_model, _columns);
                var _metric = _context.MulticlassClassification.Evaluate(_model.Transform(_data));
                progress.IsEvaluated = true;
                progress.LogLoss =_metric.LogLoss;
                progress.LogLossReduction = _metric.LogLossReduction;
                progress.MicroAccuracy = _metric.MicroAccuracy;
                progress.MacroAccuracy = _metric.MacroAccuracy;
            }
        }

        public static Dictionary<int, int> GetCharDictionary(string path)
        {
            var _dict = new Dictionary<int, int>( );
            using var _zip = ZipFile.Open(path, ZipArchiveMode.Read);
            foreach (var _entry in _zip.Entries)
            {
                if (_entry.Name == "Terms.txt")
                {
                    using var _stream = _entry.Open();
                    using var _reader = new StreamReader( _stream );
                    foreach (var _pair in GetLines(_reader).Skip(1).Select(x => x.Split('\t').Select(n => int.Parse(n.Trim())).Take(2).ToArray()))
                    {
                        _dict[_pair[0]] = _pair[1];
                    }

                    static IEnumerable<string> GetLines(StreamReader reader)
                    {
                        string _line;
                        while ((_line = reader.ReadLine()) != null)
                        {
                            yield return _line;
                        }
                    }
                }
            }

            return _dict;
        }

        private static IEnumerable<CharacterData> GetTrainData(FontSetting setting, IProgress progress)
        {
            foreach (var _alphabet in setting.GetAlphabets())
            {
                using var _mat = _alphabet.Print();
                _mat.GetArray(out byte[] _data);
                progress.Current++;
                var _pixels = CharacterData.GetData(_data);
                if (_pixels[62] > 10 || _alphabet.Character == ' ')
                {
                    if (_minPixelsDictionary.TryGetValue(_alphabet.Character, out var _value1))
                    {
                        _minPixelsDictionary[_alphabet.Character] = Math.Min(_value1, (int)_pixels[62]);
                    }
                    else
                    {
                        _minPixelsDictionary[_alphabet.Character] = (int)_pixels[62];
                    }

                    if (_maxPixelsDictionary.TryGetValue(_alphabet.Character, out var _value2))
                    {
                        _maxPixelsDictionary[_alphabet.Character] = Math.Max(_value2, (int)_pixels[62]);
                    }
                    else
                    {
                        _maxPixelsDictionary[_alphabet.Character] = (int)_pixels[62];
                    }
                
                    yield return new CharacterData
                    {
                        CharacterId = _alphabet.CharacterId,
                        Pixels = _pixels,
                    };
                }
            }
        }

        private static IEnumerable<CharacterData> GetTrainData(IEnumerable<FontSetting> settings, IProgress progress)
        {
            for (var _i = 0; _i < RETRAIN_FACTOR; _i++)
            {
                foreach (var _setting in settings.OrderBy(x => Random.Shared.Next()))
                {
                    foreach (var _data in GetTrainData(_setting, progress))
                    {
                        yield return _data;
                    }
                }
            }
        }

        private static IEnumerable<CharacterOpticalData> GetOpticalTrainData(FontSetting setting, IProgress progress)
        {
            foreach (var _alphabet in setting.GetAlphabets().OrderBy(x => Random.Shared.Next()))
            {
                using var _mat = _alphabet.Print();
                progress.Current++;
                yield return new CharacterOpticalData
                {
                    CharacterId = _alphabet.CharacterId,
                    Pixels = _mat.ToBytes(".jpg")
                };
            }
        }

        private static IEnumerable<CharacterOpticalData> GetOpticalTrainData(IEnumerable<FontSetting> settings, IProgress progress)
        {
            for (var _i = 0; _i < RETRAIN_FACTOR; _i++)
            {
                foreach (var _setting in settings)
                {
                    foreach (var _data in GetOpticalTrainData(_setting, progress))
                    {
                        yield return _data;
                    }
                }
            }
        }

    }
}
