using NTwain;
using NTwain.Data;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps a capability as a view model.
    /// </summary>
    class CapVM
    {
        DataSource _ds;
        object _wrapper;
        MethodInfo _getMethod;
        MethodInfo _getCurrentMethod;
        MethodInfo _setMethod;

        public CapVM(DSVM ds, CapabilityId cap)
        {
            _ds = ds.DS;
            Cap = cap;
            Supports = ds.DS.CapQuerySupport(cap);

            var capName = cap.ToString();
            var wrapProperty = ds.DS.GetType().GetProperty(capName);
            if (wrapProperty != null)
            {
                _wrapper = wrapProperty.GetGetMethod().Invoke(ds.DS, null);
                var wrapperType = _wrapper.GetType();
                _getMethod = wrapperType.GetMethod("Get");
                _getCurrentMethod = wrapperType.GetMethod("GetCurrent");
                _setMethod = wrapperType.GetMethods().FirstOrDefault(m => m.Name == "Set");
            }
        }

        public IEnumerable Get()
        {
            if (_getMethod == null)
            {
                return _ds.CapGet(Cap);
            }
            return _getMethod.Invoke(_wrapper, null) as IEnumerable;
        }
        public object GetCurrent()
        {
            if (_getMethod == null)
            {
                return _ds.CapGetCurrent(Cap);
            }
            return _getCurrentMethod.Invoke(_wrapper, null);
        }
        public void Set(object value)
        {
            if (_setMethod != null && value != null)
            {
                _setMethod.Invoke(_wrapper, new object[] { value });
            }
        }

        public object MyProperty { get; set; }

        public CapabilityId Cap { get; private set; }

        public string Name
        {
            get
            {
                if (Cap > CapabilityId.CustomBase)
                {
                    return "[Custom] " + ((int)Cap - (int)CapabilityId.CustomBase);
                }
                return Cap.ToString();
            }
        }

        public QuerySupports Supports { get; private set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
