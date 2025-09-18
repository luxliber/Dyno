using System;
using System.IO;

namespace Dyno.Views.FormControls
{
   
    [Serializable]
    public class FormImageSource
    {
        public string Name => new FileInfo(Path).Name;

        public string Path { set; get; }

        public FormImageSource(){}
    }

   
}
