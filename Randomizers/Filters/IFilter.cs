using System.Xml.Linq;

namespace DanielleDotnet.Randomizers.Filters;

public interface IFilter
{
    /**
     * Applies desired changes to the given element.
     */
    public void Apply(XElement e, Random r, string levelFile);
}