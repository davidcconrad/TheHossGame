using System.Threading.Tasks;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Storage;
using Pulumi.AzureNative.Storage.Inputs;
using Pulumi.AzureNative.Web.Inputs;

class MyStack : Stack
{
  public MyStack()
  {
    // Create an Azure Resource Group
    var resourceGroup = new ResourceGroup("resourceGroup");

    var appService = new AppServicePlan("asp", new AppServicePlanArgs
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
  }

  private static StorageAccount CreateStorageAccount(ResourceGroup resourceGroup)
  {

    // Create an Azure resource (Storage Account)
    return new StorageAccount("sa", new StorageAccountArgs
    {
      ResourceGroupName = resourceGroup.Name,
      Sku = new SkuArgs
      {
        Name = SkuName.Standard_LRS
      },
      Kind = Kind.StorageV2
    });
  }

  [Output]
  public Output<string> PrimaryStorageKey { get; set; }

  [Output]
  public Output<string> StaticEndpoint { get; set; }

  private static async Task<string> GetStorageAccountPrimaryKey(string resourceGroupName, string accountName)
  {
    var accountKeys = await ListStorageAccountKeys.InvokeAsync(new ListStorageAccountKeysArgs
    {
      ResourceGroupName = resourceGroupName,
      AccountName = accountName
    });
    return accountKeys.Keys[0].Value;
  }
}
