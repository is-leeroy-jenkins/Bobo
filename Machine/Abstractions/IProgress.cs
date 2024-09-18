namespace Bobo
{
    public interface IProgress
    {
        public int Maximum { get; set; }

        public int Current { get; set; }
    }
}
