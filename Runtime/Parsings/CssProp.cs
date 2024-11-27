namespace Parsings
{
    public struct CssProp
    {
        public string name;
        public object value;

        public override string ToString()
        {
            return $"{name}:{value}";
        }
    }
}