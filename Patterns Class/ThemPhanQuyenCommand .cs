using QLTTTM.models;

public class ThemPhanQuyenCommand : ICommand
{
    private readonly int _macv;
    private readonly int _macn;
    private readonly string _noidung;
    private readonly DataSQLContext _data;

    public ThemPhanQuyenCommand(int macv, int macn, string noidung, DataSQLContext data)
    {
        _macv = macv;
        _macn = macn;
        _noidung = noidung;
        _data = data;
    }

    public async Task ExecuteAsync()
    {
        if (_macv != 0 && _macn != 0)
        {
            PhanQuyen phanQuyen = new PhanQuyen
            {
                MACV = _macv,
                MACN = _macn,
                GHICHU = string.IsNullOrEmpty(_noidung) ? "Empty" : _noidung
            };
            await _data.PhanQuyens.AddAsync(phanQuyen);
            await _data.SaveChangesAsync();
        }
    }
}
