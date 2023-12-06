namespace PereViader.Utils.Common.ApplicationContexts
{
    public enum ApplicationContextChangeStep
    {
        WaitingToStart,
        ProcessingPrevious,
        AwaitingPermissionForFinal,
        StartingFinal,
        Complete
    }
}