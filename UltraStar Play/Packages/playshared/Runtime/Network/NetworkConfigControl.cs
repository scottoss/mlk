using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniInject;
using UniRx;
using UnityEngine.UIElements;

// Disable warning about fields that are never assigned, their values are injected.
#pragma warning disable CS0649

public class NetworkConfigControl : INeedInjection, IInjectionFinishedListener
{
    [Inject(UxmlName = "networkConfigContainer")]
    protected VisualElement networkConfigContainer;

    [Inject(UxmlName = "udpPortOnClientTextField")]
    protected TextField udpPortOnClientTextField;

    [Inject(UxmlName = "udpPortOnServerTextField")]
    protected TextField udpPortOnServerTextField;

    [Inject(UxmlName = "ownHostTextField")]
    protected TextField ownHostTextField;

    [Inject]
    protected ISettings settings;

    [Inject]
    protected GameObject gameObject;

    public virtual void OnInjectionFinished()
    {
        // Update value when TextField changes
        BindTextField(udpPortOnServerTextField,
            () => settings.UdpPortOnServer,
            newStringValue => PropertyUtils.TrySetIntFromString(newStringValue, newIntValue => settings.UdpPortOnServer = newIntValue));

        BindTextField(udpPortOnClientTextField,
            () => settings.UdpPortOnClient,
            newStringValue => PropertyUtils.TrySetIntFromString(newStringValue, newIntValue => settings.UdpPortOnClient = newIntValue));

        BindTextField(ownHostTextField,
            () => settings.OwnHost,
            newStringValue => settings.OwnHost = newStringValue);
    }

    private void BindTextField(TextField textField, Func<object> valueGetter, Action<string> valueSetter)
    {
        object initialValue = valueGetter();
        textField.value = initialValue != null
            ? initialValue.ToString()
            : "";

        textField.RegisterValueChangedCallback(evt => valueSetter(evt.newValue));
    }
}
