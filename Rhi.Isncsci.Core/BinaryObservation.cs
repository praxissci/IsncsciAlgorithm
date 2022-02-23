using System;

namespace Rhi.Isncsci.Core
{
	[Flags]
    public enum BinaryObservation
    {
        None = 0x0,
        Yes = 0x1,
        No = 0x2,
        NT = 0x4
    }
}
