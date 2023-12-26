using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PereViader.Utils.Common.ActiveStatuses;
using PereViader.Utils.Common.Attributes;

namespace PereViader.Utils.Unity3d.UiStack
{
    [Experimental]
    public interface IUiStackService
    {
        ActiveStatus InteractableActiveStatus { get; }
        IReadOnlyDictionary<UiStackLayer, ActiveStatus> LayerInteractableActiveStatus { get; }
        IReadOnlyDictionary<UiStackLayer, IReadOnlyCollection<UiStackElement>> UiStackElements { get; }

        void Register(UiStackElement uiStackElement);
        void Unregister(UiStackElement uiStackElement);
        
        Task Show(UiStackElement uiStackElement, bool instantly, CancellationToken cancellationToken);
        Task Hide(UiStackElement uiStackElement, bool instantly, CancellationToken cancellationToken);
        Task SetVisible(UiStackElement uiStackElement, bool visible, bool instantly, CancellationToken cancellationToken); 
    }
}