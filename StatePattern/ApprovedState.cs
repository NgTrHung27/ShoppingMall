using QLTTTM.models;

namespace DAPM.StatePattern
{
    public class ApprovedState : IEventState
    {
        public void Handle(SuKien suKien)
        {
            // Logic for handling approved state
            suKien.TRANGTHAI = true;
        }
    }
}
