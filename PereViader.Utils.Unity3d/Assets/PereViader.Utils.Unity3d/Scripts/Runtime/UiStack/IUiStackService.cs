using PereViader.Utils.Common.ActiveStatuses;
using PereViader.Utils.Common.Attributes;

namespace PereViader.Utils.Unity3d.UiStack
{
    [Experimental]
    public interface IUiStackService
    {
        ActiveStatus InteractableActiveStatus { get; }
        
        public void Register(UiStackElement uiStackElement);
        public void Unregister(UiStackElement uiStackElement);
    }
}