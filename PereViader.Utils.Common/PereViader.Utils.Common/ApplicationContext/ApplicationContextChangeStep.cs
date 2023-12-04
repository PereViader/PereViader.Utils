namespace PereViader.Utils.Common.ApplicationContext
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