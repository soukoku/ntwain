namespace NTwain.Internals
{
    class TentativeStateCommitable : ICommittable
    {
        bool _commit;
        ITwainSessionInternal _session;
        int _origState;
        int _newState;
        public TentativeStateCommitable(ITwainSessionInternal session, int newState)
        {
            _session = session;
            _origState = session.State;
            _newState = newState;
            _session.ChangeState(newState, false);
        }

        #region ICommitable Members

        public void Commit()
        {
            if (_session.State == _newState)
            {
                _session.ChangeState(_newState, true);
            }
            _commit = true;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (!_commit && _session.State == _newState)
            {
                _session.ChangeState(_origState, false);
            }
        }

        #endregion
    }
}
