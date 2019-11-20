using System;

namespace GoogleTaskDesktop.ViewModel
{
    public interface IReqeustItemEditable
	{
        event EventHandler UpdatedRequested;
        event EventHandler RemoveRequested;
    }
}