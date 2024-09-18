namespace Bobo
{
    using System.Collections.Generic;

    public interface ITrainer 
    {
        public void Train(IEnumerable<FontSetting> settings, EngineType engine, ITrainingProgress progress);

        public void Evaluate(FontSetting setting, EngineType engine, ITestProgress progress);

        public void EvaluateAccuracy(FontSetting setting, EngineType engine, IAccuracyProgress progress);

        public void SaveModel(string path, EngineType type);

        public void LoadModel(string path, EngineType type);

        public void Delete(EngineType engine);
    }
}
