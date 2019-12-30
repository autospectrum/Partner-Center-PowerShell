﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Store.PartnerCenter.PowerShell.Commands
{
    using System.Management.Automation;
    using System.Text.RegularExpressions;
    using Models.Authentication;
    using Models.ServiceCosts;
    using PartnerCenter.Models.ServiceCosts;

    [Cmdlet(VerbsCommon.Get, "PartnerCustomerServiceCostsSummary")]
    [OutputType(typeof(PSServiceCostsSummary))]
    public class GetPartnerCustomerServiceCostsSummary : PartnerAsyncCmdlet
    {
        /// <summary>
        /// Gets or sets the billing period.
        /// </summary>
        [Parameter(HelpMessage = "An indicator that represents the billing period.", Mandatory = true)]
        [ValidateSet(nameof(ServiceCostsBillingPeriod.MostRecent))]
        public ServiceCostsBillingPeriod BillingPeriod { get; set; }

        /// <summary>
        /// Gets or sets the required customer identifier.
        /// </summary>
        [Parameter(HelpMessage = "The identifier for the customer.", Mandatory = true)]
        [ValidatePattern(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", Options = RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        public string CustomerId { get; set; }

        /// <summary>
        /// Executes the operations associated with the cmdlet.
        /// </summary>
        public override void ExecuteCmdlet()
        {
            Scheduler.RunTask(async () =>
            {
                IPartner partner = await PartnerSession.Instance.ClientFactory.CreatePartnerOperationsAsync(CorrelationId, CancellationToken).ConfigureAwait(false);
                ServiceCostsSummary summary = await partner.Customers[CustomerId].ServiceCosts.ByBillingPeriod(BillingPeriod).Summary.GetAsync(CancellationToken).ConfigureAwait(false);

                WriteObject(new PSServiceCostsSummary(summary));
            }, true);
        }
    }
}