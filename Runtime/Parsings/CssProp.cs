namespace Parsings
{
    public struct CssProp
    {
        public string name;
        public string value;

        public override string ToString()
        {
            return $"{name}:{value}";
        }
    }
}