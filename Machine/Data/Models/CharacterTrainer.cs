
namespace Bobo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading;
    using Microsoft.ML;
    using Microsoft.ML.Data;
    using OpenCvSharp;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public static class CharacterTrainer
    {
        private static Dictionary<int, char> GetCharDictionary(string model)
        {
            var _dict = new Dictionary<int, char>( );
            using var _zip = ZipFile.Open($"{model}.onnx", ZipArchiveMode.Read);
            foreach (var _entry in _zip.Entries)
            {
                if (_entry.Name == "Terms.txt")
                {
                    using var _stream = _entry.Open();
                    using var _reader = new StreamReader( _stream );
                    foreach (var _pair in GetLines(_reader).Skip(1).Select(x => x.Split('\t').Select(n => int.Parse(n.Trim())).Take(2).ToArray()))
                    {
                        _dict[_pair[0]] = (char)(_pair[1] + '0');
                    } 

                    static IEnumerable<string> GetLines(StreamReader reader)
                    {
                        string _line;
                        while (( _line = reader.ReadLine()) != null)
                        {
                            yield return _line;
                        }
                    }
                }
            }

            return _dict;
        }
        
        public static void TrainImageClassificationData(string folder, string modelName, int[] rows, bool test = true)
        {
            var _context = new MLContext( );
            var _traindata = _context.Data.LoadFromEnumerable(ImageDataBuilder.GetImageData(folder, rows));
            var _partition = _context.Data.TrainTestSplit(_traindata, 0.2);
            var _pipeline = _context.Transforms.Conversion?.MapValueToKey(inputColumnName: "Character", outputColumnName: "Label")
                    .Append(_context.Transforms.Concatenate("Features", "PixelValues"))
                    .AppendCacheCheckpoint(_context)
                    .Append(_context.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features"))
                    .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Training Model");
            var _model = _pipeline.Fit(test ? _partition.TrainSet : _traindata);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Training Completed");

            if (test == true)
            {
                var _metric = _context.MulticlassClassification.Evaluate(_model.Transform(_partition.TestSet));
                Console.ForegroundColor = ConsoleColor.Green;


                Console.WriteLine($"Micro Accuracy:         {_metric.MicroAccuracy:0.###}");
                Console.WriteLine($"Macro Accuracy:         {_metric.MacroAccuracy:0.###}");
                Console.WriteLine();

                Console.WriteLine($"Log Loss:               {_metric.LogLoss:#.###}");
                Console.WriteLine($"Log Loss Reduction:     {_metric.LogLossReduction:#.###}");
                Console.WriteLine();

                Console.WriteLine($"Peak Accuracy:          {_metric.TopKAccuracy:###.###}");
                Console.WriteLine($"Peak Count:             {_metric.TopKPredictionCount}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                _context.Model.Save(_model, _traindata.Schema, $"{modelName}.onnx");
            }

        }
        public static void InterractiveTesting(string csvPath, string modelName)
        {
            var _context = new MLContext( );
            var _model = _context.Model.Load($"{modelName}.onnx", out var _schema);
            
            

            var _engine = _context.Model.CreatePredictionEngine<CharacterData, CharacterPrediction>(_model);

            var _dictionary = GetCharDictionary(modelName);
            var _reverse = _dictionary.ToDictionary(x => x.Value, y => y.Key);

            
            Console.WriteLine("Intializing Data...");
            
            var _data = _context.Data.LoadFromTextFile<CharacterData>(csvPath, hasHeader:false, separatorChar: ',');
            var _skips = 806 * 8;
            var _chars = _data.GetColumn<float>("Character").Skip(_skips).Take(806).ToArray();
            var _pixels = _data.GetColumn<float[]>("PixelValues").Skip(_skips).Take(806).ToList();
            for (var _i = 0; _i < _chars.Length; _i++)
            {
                var _testdata = new CharacterData
                {
                    CharacterId = (int)_chars[_i],
                    Pixels = _pixels[_i]
                };
                
                var _predictionData = _engine.Predict(_testdata);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                var _confidence = _predictionData.Score[_reverse[(char)(_predictionData.Character  + '0')]];
                Console.WriteLine($"Actual | Predicted | Confidence : {FloatToChar(_testdata.CharacterId)} {FloatToChar(_predictionData.Character)} : {_confidence}");

                Console.ForegroundColor = _predictionData.Character == _testdata.CharacterId ? ConsoleColor.DarkGreen : ConsoleColor.Red;

                for (var _scoreIndex = 0; _scoreIndex < _predictionData.Score.Length; _scoreIndex++)
                {
                    var _score = $"{_predictionData.Score[_scoreIndex]:0.#####}";
                    if (string.IsNullOrWhiteSpace(_score) == false && _predictionData.Score[_scoreIndex] > 0.1)
                    {
                        Console.WriteLine($"Character : {_dictionary[_scoreIndex]} : {_score}");
                    }
                }
                Thread.Sleep(100);
                using var _pixel = new Mat( 1, 1, MatType.CV_8UC1, new Scalar( 0 ) );
                using var _mat = new Mat( 33, 33, MatType.CV_8UC1, new Scalar( 255 ) );
                for (var _index = 0; _index < _testdata.Pixels.Length; _index++)
                {
                    if (_testdata.Pixels[_index] == 1)
                    {
                        var _xCord = _index % 32;
                        var _yCord = _index / 32;
                        _pixel.CopyTo(_mat[_yCord, _yCord + 1, _xCord, _xCord + 1]);
                    }

                }
                Cv2.ImShow($"Char = {_testdata.CharacterId}", _mat);
                Cv2.WaitKey();
            }
        }

        /// <summary>
        /// Floats to character.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// char
        /// </returns>
        public static char FloatToChar(float value)
        {
            return ( char )( ( int )( value + '0' ) );
        }

        public static void TrainAndSave(string csvPath, string outputModelName, bool test = true)
        {
            var _context = new MLContext( );
            var _traindata = _context.Data.LoadFromTextFile<CharacterData>(path: csvPath, hasHeader: false, separatorChar: ',');
            var _pipeline = _context.Transforms.Conversion.MapValueToKey(inputColumnName: "Character", outputColumnName: "Label")
                .Append(_context.Transforms.Concatenate("Features", "PixelValues"))
                .AppendCacheCheckpoint(_context)
                .Append(_context.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(_context.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            var _partition = _context.Data.TrainTestSplit(_traindata, test ? 0.2d : double.Epsilon);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Training Model");
            var _model = _pipeline.Fit(_partition.TrainSet);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Training Completed");
            if (test == true)
            {
                var _metric = _context.MulticlassClassification.Evaluate(_model.Transform(_partition.TestSet));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Micro Accuracy:         {_metric.MicroAccuracy:0.###}");
                Console.WriteLine($"Macro Accuracy:         {_metric.MacroAccuracy:0.###}");
                Console.WriteLine();
                Console.WriteLine($"Log Loss:               {_metric.LogLoss:#.###}");
                Console.WriteLine($"Log Loss Reduction:     {_metric.LogLossReduction:#.###}");
                Console.WriteLine();
                Console.WriteLine($"Peak Accuracy:          {_metric.TopKAccuracy:###.###}");
                Console.WriteLine($"Peak Count:             {_metric.TopKPredictionCount}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                _context.Model.Save(_model, _traindata.Schema, $"{outputModelName}.onnx");
            }
        }

        public static void ImageClassificationInterractiveTesting(string folder, string modelName,  int[] rows)
        {
            var _context = new MLContext( seed: 0 );
            var _model = _context.Model.Load($"{modelName}.onnx", out var _columns);
            var _engine = _context.Model.CreatePredictionEngine<OpticalCharacterData, CharacterPrediction>(_model);
            var _dictionary = GetCharDictionary(modelName);
            var _reverse = _dictionary.ToDictionary(x => x.Value, y => y.Key);
            foreach (var _data in ImageDataBuilder.GetImageData(folder, rows))
            {
                var _predictionData = _engine.Predict(_data);
                var _confidence = _predictionData.Score[_reverse[(char)(_predictionData.Character + '0')]];
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Actual | Predicted | Confidence : {FloatToChar(_data.Character)} | {FloatToChar(_predictionData.Character)} | {_confidence}");
                if (_predictionData.Character != _data.Character)
                {
                    Console.ReadKey();
                }
            }
        }
    }
}
