﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.PowerShell.Commands
{
    using System.Linq;
    using System.Management.Automation;
    using Models.Analytics;
    using Models.Authentication;
    using PartnerCenter.Models;
    using PartnerCenter.Models.Analytics;

    /// <summary>
    /// Get partner licenses usage information aggregated to include all customers from Partner Center.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "PartnerLicenseUsageInfo")]
    [OutputType(typeof(PSPartnerLicensesUsageInsight))]
    public class GetPartnerLicenseUsageInfo : PartnerAsyncCmdlet
    {
        /// <summary>
        /// Executes the operations associated with the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            Scheduler.RunTask(async () =>
            {
                IPartner partner = await PartnerSession.Instance.ClientFactory.CreatePartnerOperationsAsync(CorrelationId, CancellationToken).ConfigureAwait(false);
                ResourceCollection<PartnerLicensesUsageInsights> insights = await partner.Analytics.Licenses.Usage.GetAsync(CancellationToken).ConfigureAwait(false);

                WriteObject(insights.Items.Select(l => new PSPartnerLicensesUsageInsight(l)), true);
            }, true);
        }
    }
}