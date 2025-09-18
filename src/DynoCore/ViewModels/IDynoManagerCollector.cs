using System.Collections.Generic;
using Dyno.Models;
using Dyno.Models.Parameters;

namespace Dyno.ViewModels
{
    public interface IDynoManagerCollector
    {
        object SelectObject();

        List<object> SelectObjectsInOrder();

        object SelectFace();
        List<object> SelectFaces();

        object SelectEdge();
        List<object> SelectEdges();

        object SelectPointOnFace();

        List<object> SelectObjectsByRectangle();

        List<object> SelectObjects();

        DynoSettingsBase GetSettingsBase();

        string GetElementId(object element);

        void SelectElements(SelectElementParameter spar);
        void SelectReference(SelectReferenceParameter spar);
        void SelectFile(PathParameter spar);
    }
}