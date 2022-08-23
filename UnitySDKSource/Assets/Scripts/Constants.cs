using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Headings and descriptions of different feature types.
/// </summary>
internal sealed class Constants
{
    internal const string Success = "Success";
    internal const string LoggedEvent = "Logged event";
    internal const string CustomEventsFailed = "Custom event logging failed with error";
    internal const string specialCharacterUsername = "specialCharacterUsername";

    #region Info Panel Details
    internal const string mikrosHeading = "MIKROS SSO";
    internal const string mikrosDescription = "Log into or Sign up for Mikros.";
    internal const string authHeading = "Authentication";
    internal const string authDescription = "Sign into Mikros account for sample app funtionality.";
    internal const string analyticsHeading = "Analytics";
    internal const string analyticsDescription = "Log and flush events on Mikros.";
    internal const string reputationHeading = "Reputation Scoring";
    internal const string reputationDescription = "Test various player bahaviors to view the changes in the reputation score.";
    internal const string crashHeading = "Crash Reporting";
    internal const string crashDescription = "Test Crash Reporting for Mikros.";
    internal const string purchaseHeading = "In-App Purchase";
    internal const string purchaseDescription = "Test In-App Purchase events for Mikros.";
    #endregion Info Panel Details
}
