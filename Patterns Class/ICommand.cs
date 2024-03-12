using System.Runtime.InteropServices;
using System;

public interface ICommand
{
    Task ExecuteAsync();
}

