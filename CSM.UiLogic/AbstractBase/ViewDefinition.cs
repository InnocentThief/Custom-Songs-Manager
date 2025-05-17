using System.IO;

namespace CSM.UiLogic.AbstractBase
{
    internal class ViewDefinition
    {
        public string Name { get; set; } = string.Empty;

        public Stream? Stream { get; set; }
    }
}
