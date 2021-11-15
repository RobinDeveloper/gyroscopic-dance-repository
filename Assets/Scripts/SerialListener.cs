using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SerialListener : MonoBehaviour
{
    [SerializeField] private SoundMaker m_SoundMaker;
    
    // Invoked when a line of data is received from the serial device.
    private void OnMessageArrived(string _message)
    {
        ParseMessage(_message);
    }

    private void ParseMessage(string _message)
    {
        string[] splitString = _message.Split(',');

        SendVectorsToSoundMaker(new Vector3(float.Parse(splitString[0], CultureInfo.InvariantCulture), float.Parse(splitString[1], CultureInfo.InvariantCulture), float.Parse(splitString[2], CultureInfo.InvariantCulture)),
                                new Vector3(float.Parse(splitString[3], CultureInfo.InvariantCulture), float.Parse(splitString[4], CultureInfo.InvariantCulture), float.Parse(splitString[5], CultureInfo.InvariantCulture)));
    }

    private void SendVectorsToSoundMaker(Vector3 _ypr, Vector3 _xyz)
    {
        Vector3 yawPitchRoll = _ypr;
        Vector3 AccelerationXYZ = _xyz;

        m_SoundMaker.UpdateYPRXYZ(yawPitchRoll, AccelerationXYZ);
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    private void OnConnectionEvent(bool _success)
    {
        Debug.Log(_success.ToString());
    }
}
