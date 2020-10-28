using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public interface IPatternService
    {
        string GetPatternDefinition(EWPattern pattern, Dictionary<string, object> cache = null);
        EWPattern GetPattern(string definition, Dictionary<string, object> cache = null);
    }
}