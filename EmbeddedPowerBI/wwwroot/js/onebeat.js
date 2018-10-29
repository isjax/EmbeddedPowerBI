﻿    // Read embed application token from Model
    var accessToken = "@Model.Input.EmbedToken.Token";

    // Read embed URL from Model
    var embedUrl = "@Html.Raw(Model.Input.EmbedUrl)";

    // Read report Id from Model
    var embedReportId = "@Model.Input.Id";

    // Get models. models contains enums that can be used.
    var models = window['powerbi-client'].models;

    // Embed configuration used to describe the what and how to embed.
    // This object is used when calling powerbi.embed.
    // This also includes settings and options such as filters.
    // You can find more information at https://github.com/Microsoft/PowerBI-JavaScript/wiki/Embed-Configuration-Details.
    var config = {
        type: 'report',
    tokenType: models.TokenType.Embed,
    accessToken: accessToken,
    embedUrl: embedUrl,
    id: embedReportId,
    permissions: models.Permissions.All,
        settings: {
        layoutType: models.LayoutType.Custom,
            customLayout: {
        displayOpton: models.DisplayOption.FitToWidth
},
filterPaneEnabled: false,
navContentPaneEnabled: true
}
};

// Get a reference to the embedded report HTML element
var reportContainer = $('#reportContainer')[0];
// Embed the report and display it within the div container.
var report = powerbi.embed(reportContainer, config);
