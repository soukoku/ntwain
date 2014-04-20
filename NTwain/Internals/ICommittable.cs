using System;

namespace NTwain.Internals
{
    interface ICommittable : IDisposable
    {
        void Commit();
    }
}
