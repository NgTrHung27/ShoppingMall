using QLTTTM.models;

namespace DAPM.StatePattern
{
    public class UnapprovedState : IEventState
    {
        public void Handle(SuKien suKien)
        {
            // Logic for handling unapproved state
            suKien.TRANGTHAI = false;
        }
    }
}
