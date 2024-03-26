using QLTTTM.models;

namespace DAPM.StatePattern
{
    public interface IEventState
    {
        void Handle(SuKien suKien);
    }
}
