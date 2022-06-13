param (
    [string]
    [Parameter(Mandatory=$true)]
    $resourceGroupName,
    [string]
    [Parameter(Mandatory=$true)]
    $organization,
    [string]
    [Parameter(Mandatory=$true)]
    $repo,
    [string]
    [Parameter(Mandatory=$true)]
    $branch
)

## Values for The Hoss Game

# $resourceGroupName = 'resourceGroupc5d90805'
# $organization = 'davidcconrad'
# $repo = 'TheHossGame'
# $branch = 'main'

Connect-AzAccount

New-AzADApplication -DisplayName $repo
$app = Get-AzADApplication -DisplayName $repo
$clientId = $app.AppId
$appObjectId = $app.Id
New-AzADServicePrincipal -ApplicationId $clientId
$objectId = (Get-AzADServicePrincipal -DisplayName $repo).Id
New-AzRoleAssignment -ObjectId $objectId -RoleDefinitionName Contributor -ResourceGroupName $resourceGroupName
$subscriptionId = (Get-AzContext).Subscription.Id
$tenantId = (Get-AzContext).Subscription.TenantId

$uri = "https://graph.microsoft.com/beta/applications/$appObjectId/federatedIdentityCredentials"
# $payload = "{`"name`":`"$repo`",`"issuer`":`"https://token.actions.githubusercontent.com`",`"subject`":`"repo:$organization/$repo`:ref:refs/heads/$branch`",`"description`":`"Testing`",`"audiences`":[`"api://AzureADTokenExchange`"]}" 
$payload = "{`"name`":`"perEnvironment`",`"issuer`":`"https://token.actions.githubusercontent.com`",`"subject`":`"repo:$organization/$repo`:environment`:Dev`",`"description`":`"Testing`",`"audiences`":[`"api://AzureADTokenExchange`"]}" 

Invoke-AzRestMethod -Method POST -Uri $uri -Payload $payload

gh secret set -a actions AZURE_CLIENT_ID -b "$clientId"
gh secret set -a actions AZURE_TENANT_ID -b "$tenantId"
gh secret set -a actions AZURE_SUBSCRIPTION_ID -b "$subscriptionId" 	