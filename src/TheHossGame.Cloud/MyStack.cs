using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web.Inputs;

class MyStack : Stack
{
  private const string WebAppName = "todo-list";

  public MyStack()
  {
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("resourceGroup");

    var appServicePlan = new AppServicePlan("asp", new AppServicePlanArgs
    {
      ResourceGroupName = resourceGroup.Name,
      Kind = "App",
      Sku = new SkuDescriptionArgs
      {
        Capacity = 1,
        Family = "P",
        Name = "P1",
        Size = "P1",
        Tier = "Premium"
      }
    });

    var webApp = new WebApp(WebAppName, new WebAppArgs
    {
      ResourceGroupName = resourceGroup.Name,
      Location = appServicePlan.Location,
      ServerFarmId = appServicePlan.Id,
      HttpsOnly = true
    });

    this.PublishingUserName = GetWebAppPublishingKeys(resourceGroup, webApp);
    this.ResourceGroupName = resourceGroup.Name.Apply(resource => resource);
  }

  [Output]
  public Output<string> PublishingUserName { get; set; }

  [Output]
  public Output<string> ResourceGroupName { get; set; }

  private static Output<string> GetWebAppPublishingKeys(ResourceGroup resourceGroup, WebApp webApp)
  {
    return ListWebAppPublishingCredentials.Invoke(new ListWebAppPublishingCredentialsInvokeArgs
    {
      ResourceGroupName = resourceGroup.Name,
      Name = webApp.Name
    }).Apply(webApp => webApp.PublishingUserName);
  }
}
