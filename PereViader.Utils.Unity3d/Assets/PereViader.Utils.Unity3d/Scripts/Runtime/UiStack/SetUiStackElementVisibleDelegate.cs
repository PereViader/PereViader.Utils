using System.Threading;
using System.Threading.Tasks;

namespace PereViader.Utils.Unity3d.UiStack
{
    public delegate Task SetUiStackElementVisibleDelegate(bool visible, bool instantly, CancellationToken ct);
}