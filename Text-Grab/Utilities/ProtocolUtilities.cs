using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Text_Grab.Utilities;

/// <summary>
/// Utility class for the text-grab:// protocol used by companion apps such as
/// the Text Grab browser extension. The URI is only a command channel; any
/// data payload (like a copied table) travels via the clipboard.
/// Supported URIs:
///   text-grab://paste-spreadsheet      Edit Text window in spreadsheet mode, paste clipboard
///   text-grab://edit-text              Edit Text window with clipboard text
///   text-grab://grab-frame[?path=...]  Grab Frame, optionally opening a local image/PDF
///   text-grab://grab-text?path=...     OCR a local image/PDF straight to the clipboard (no window)
///   text-grab://fullscreen             Fullscreen grab
///   text-grab://quick-lookup           Quick Simple Lookup
///   text-grab://settings               Settings window
/// </summary>
internal static class ProtocolUtilities
{
    internal const string Scheme = "text-grab";

    private const string ProtocolKeyPath = @"Software\Classes\" + Scheme;

    /// <summary>
    /// Returns true when a startup argument looks like a text-grab:// URI.
    /// </summary>
    internal static bool IsProtocolUri(string? argument)
    {
        return argument is not null
            && argument.StartsWith($"{Scheme}:", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parses a text-grab:// URI into a lowercase command and its query parameters.
    /// Accepts both text-grab://command?key=value and text-grab:command forms.
    /// </summary>
    internal static bool TryParseProtocolUri(
        string uriString,
        out string command,
        out Dictionary<string, string> parameters)
    {
        command = string.Empty;
        parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (!Uri.TryCreate(uriString, UriKind.Absolute, out Uri? uri)
            || !string.Equals(uri.Scheme, Scheme, StringComparison.OrdinalIgnoreCase))
            return false;

        // text-grab://paste-spreadsheet puts the command in Host;
        // text-grab:paste-spreadsheet puts it in AbsolutePath.
        string rawCommand = !string.IsNullOrEmpty(uri.Host) ? uri.Host : uri.AbsolutePath;
        command = rawCommand.Trim('/').ToLowerInvariant();
        if (string.IsNullOrEmpty(command))
            return false;

        string query = uri.Query.TrimStart('?');
        foreach (string pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            int separatorIndex = pair.IndexOf('=');
            if (separatorIndex <= 0)
                continue;
            string key = Uri.UnescapeDataString(pair[..separatorIndex]);
            string value = Uri.UnescapeDataString(pair[(separatorIndex + 1)..]);
            parameters[key] = value;
        }

        return true;
    }

    /// <summary>
    /// Registers the text-grab:// protocol for the current user when running
    /// unpackaged. Packaged installs register it through the MSIX manifest.
    /// Safe to call on every startup; only writes when missing or stale.
    /// </summary>
    internal static void EnsureProtocolRegistration()
    {
        if (AppUtilities.IsPackaged())
            return;

        string executablePath = FileUtilities.GetExePath();
        if (string.IsNullOrEmpty(executablePath))
            return;

        string expectedCommand = $"\"{executablePath}\" \"%1\"";

        try
        {
            using (RegistryKey? existingCommandKey =
                Registry.CurrentUser.OpenSubKey($@"{ProtocolKeyPath}\shell\open\command"))
            {
                if (existingCommandKey?.GetValue(string.Empty) as string == expectedCommand)
                    return;
            }

            using RegistryKey protocolKey = Registry.CurrentUser.CreateSubKey(ProtocolKeyPath);
            protocolKey.SetValue(string.Empty, "URL:Text Grab Protocol");
            protocolKey.SetValue("URL Protocol", string.Empty);

            using RegistryKey iconKey = protocolKey.CreateSubKey("DefaultIcon");
            iconKey.SetValue(string.Empty, $"\"{executablePath}\",0");

            using RegistryKey commandKey = protocolKey.CreateSubKey(@"shell\open\command");
            commandKey.SetValue(string.Empty, expectedCommand);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"text-grab:// protocol registration failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Removes the per-user text-grab:// protocol registration.
    /// </summary>
    internal static void RemoveProtocolRegistration()
    {
        try
        {
            Registry.CurrentUser.DeleteSubKeyTree(ProtocolKeyPath, throwOnMissingSubKey: false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"text-grab:// protocol unregistration failed: {ex.Message}");
        }
    }
}
