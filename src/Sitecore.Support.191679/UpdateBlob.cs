using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.localhost;
using Sitecore.Sharepoint.Data.Providers;
using Sitecore.Sharepoint.ObjectModel.Entities.Items;
using Sitecore.Sharepoint.Pipelines.ProcessIntegrationItem;
using BaseItem = Sitecore.Sharepoint.ObjectModel.Entities.Items.BaseItem;

namespace Sitecore.Support.Sharepoint.Pipelines.ProcessIntegrationItem
{
  public class UpdateBlob
  {
    // Methods
    public virtual void Process(ProcessIntegrationItemArgs args)
    {
      Assert.ArgumentNotNull(args, "args");
      Assert.IsNotNull(args.Options, "args.Options");
      if (args.Options.ScheduledBlobTransfer || !args.SynchContext.IntegrationConfigData.ScheduledBlobTransfer)
      {
        this.Process(args.IntegrationItem, args.SourceSharepointItem, args.SynchContext);
      }
    }

    protected void Process(Item integrationItem, BaseItem sourceSharepointItem, SynchContext synchContext)
    {
      Assert.ArgumentNotNull(integrationItem, "integrationItem");
      Assert.ArgumentNotNull(sourceSharepointItem, "sourceSharepointItem");
      Assert.ArgumentNotNull(synchContext, "synchContext");
      DocumentItem sourceSharepointDocumentItem = sourceSharepointItem as DocumentItem;
      if (sourceSharepointDocumentItem != null)
      {
        Field innerField = integrationItem.Fields["__Modified"];
        if ((innerField == null) || (DateUtil.ToServerTime(new DateField(innerField).DateTime) != DateUtil.ToServerTime(System.Convert.ToDateTime(sourceSharepointDocumentItem["ows_Modified"]).ToUniversalTime())))
        {
          IntegrationItemProvider.UpdateBlob(integrationItem, sourceSharepointDocumentItem, synchContext);
        }
      }
    }
  }
}