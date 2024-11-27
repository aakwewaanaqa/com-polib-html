using System.Collections.Generic;

namespace Parsings
{
    public struct CssClass
    {
        public string               name;
        public IEnumerable<CssProp> props;
    }
}