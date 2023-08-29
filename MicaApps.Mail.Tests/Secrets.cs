﻿namespace MicaApps.Mail.Tests;

// THIS FILE SHOULD BE FILLED BY GITHUB ACTION
// **DO NOT** COMMIT YOUR SECRETS TO ORIGIN
public static class Secrets
{
    public static ProtocolMailSettings ProtocolMailSettings =
        new()
        {
            Host = null,
            Port = 0,
            UseSsl = false,
            Username = false,
            Password = false
        };
}

public class ProtocolMailSettings
{
    public string Host;
    public int Port;
    public bool UseSsl;
    public bool Username;
    public bool Password;
}