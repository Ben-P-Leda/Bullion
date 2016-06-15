using System;
using System.Collections.Generic;


namespace Assets.Scripts.Generic.ParameterManagement
{
    public static class ParameterRepository
    {
        private static Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public static void SetItem(string key, object value)
        {
            if (!_parameters.ContainsKey(key))
            {
                _parameters.Add(key, null);
            }
            _parameters[key] = value;
        }

        public static T GetItem<T>(string key)
        {
            T value = default(T);

            if ((_parameters.ContainsKey(key)) && (_parameters[key] != null))
            {
                value = (T)Convert.ChangeType(_parameters[key], typeof(T));
            }

            return value;
        }
    }
}