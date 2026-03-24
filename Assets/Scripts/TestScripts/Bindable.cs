using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bindable<T>
{
    private T _value;

    public T Value
    {
        get { return _value; }
        set {
            if (!EqualityComparer<T>.Default.Equals(_value, value)) {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public Action<T> OnValueChanged;

    public Bindable(T initialValue) {
        _value = initialValue;
    }
}
