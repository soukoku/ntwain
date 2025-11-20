using System.Collections.Generic;

namespace NTwain.Data;

// dynamic is a cheap hack to sidestep the compiler restrictions if I know TValue is numeric
class DynamicEnumerator<TValue> : IEnumerator<TValue> where TValue : struct
{
    private readonly TValue _min;
    private readonly TValue _max;
    private readonly TValue _step;
    private TValue _cur;
    bool started = false;

    public DynamicEnumerator(TValue min, TValue max, TValue step)
    {
        _min = min;
        _max = max;
        _step = step;
        _cur = min;
    }

    public TValue Current => _cur;

    object System.Collections.IEnumerator.Current => this.Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        if (!started)
        {
            started = true;
            return true;
        }

        var next = _cur + (dynamic)_step;
        if (next == _cur || next < _min || next > _max) return false;

        _cur = next;
        return true;
    }

    public void Reset()
    {
        _cur = _min;
        started = false;
    }
}
