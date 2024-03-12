using QLTTTM.models;

public class XoaPhanQuyenCommand : ICommand
{
    private readonly int _ID;
    private readonly DataSQLContext _data;

    public XoaPhanQuyenCommand(int ID, DataSQLContext data)
    {
        _ID = ID;
        _data = data;
    }

    public async Task ExecuteAsync()
    {
        if (_ID != 0)
        {
            PhanQuyen phanQuyen = await _data.PhanQuyens.FindAsync(_ID);
            if (phanQuyen != null)
            {
                _data.PhanQuyens.Remove(phanQuyen);
                await _data.SaveChangesAsync();
            }
        }
    }
}
