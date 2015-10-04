namespace ExploreFlicker.Helpers.Interfaces
{
    public interface IToastService
    {
        void ShowToastThreeLines ( string message );
        void ShowToastTwoLinesBody ( string title, string message );
    }
}
