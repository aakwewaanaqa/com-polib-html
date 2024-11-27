namespace Parsings
{
    public struct Output<T>
    {
        public bool   isRejected;
        public T      value;
        public string remaining;

        public override string ToString()
        {
            return remaining;
        }

        public static implicit operator string(Output<T> opt)
        {
            return opt.ToString();
        }

        public static Output<T> Fail(string src)
        {
            return new Output<T>
            {
                isRejected = true,
                value      = default,
                remaining  = src,
            };
        }
    }
}