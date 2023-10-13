# Runs the post-deploy script on azd
Param(
    [string]
    [Parameter(Mandatory=$false)]
    $AzureEnvName = "",

    [switch]
    [Parameter(Mandatory=$false)]
    $Help
)

function Show-Usage {
    Write-Output "    This runs the post-deploy script on azd.

    Usage: $(Split-Path $MyInvocation.ScriptName -Leaf) ``
            [-AzureEnvName <azure env name>] ``

            [-Help]

    Options:
        -AzureEnvName  Azure environment name.

        -Help:          Show this message.
"

    Exit 0
}

# Show usage
$needHelp = ($Help -eq $true) -or ($AzureEnvName -eq "")
if ($needHelp -eq $true) {
    Show-Usage
    Exit 0
}

$rg = "rg-$AzureEnvName"
$fncapp = "fncapp-$AzureEnvName"
$evtgrd = "evtgrd-$AzureEnvName"

$functionKey = $(az functionapp keys list `
    -g $rg `
    -n $fncapp `
    --query "systemKeys.eventgrid_extension" -o tsv)

$functionAppId = $(az functionapp show `
    -g $rg `
    -n $fncapp `
    --query "id" -o tsv)

$payload = @(
    @{
        name = "{{EVENT_GRID_NAME}}-subscription-webhook-cloudeventschemav10";
        endpointType = "WebHook";
        endpointUrl = "https://{{FUNCTION_APP_NAME}}.azurewebsites.net/runtime/webhooks/EventGrid?functionName=ResizeImageEventGridTrigger&code={{FUNCTION_KEY}}";
        includedEventTypes = @(
            "Microsoft.Storage.BlobCreated",
            "Microsoft.Storage.BlobDeleted"
        );
        eventDeliverySchema = "CloudEventSchemaV1_0"
    },
    @{
        name = "{{EVENT_GRID_NAME}}-subscription-azurefunction-cloudeventschemav10";
        endpointType = "AzureFunction";
        resourceId = "{{FUNCTION_APP_RESOURCE_ID}}/functions/ResizeImageEventGridTrigger";
        includedEventTypes = @(
            "Microsoft.Storage.BlobCreated",
            "Microsoft.Storage.BlobDeleted"
        );
        eventDeliverySchema = "CloudEventSchemaV1_0"
    }
) | ConvertTo-Json -Depth 100

$payload = $payload -replace "{{EVENT_GRID_NAME}}", $evtgrd
$payload = $payload -replace "{{FUNCTION_APP_NAME}}", $fncapp
$payload = $payload -replace "{{FUNCTION_KEY}}", $functionKey
$payload = $payload -replace "{{FUNCTION_APP_RESOURCE_ID}}", $functionAppId
$payload | Out-File -FilePath ./infra/eventSubscriptions.json -Encoding utf8 -Force

$deployed = $(az deployment group create `
    -g $rg `
    -n EventGridSubscription `
    -f ../_infra/eventGridSystemTopicSubscription.bicep `
    -p name=$AzureEnvName `
    -p subscriptions="@./infra/eventSubscriptions.json")

Remove-Item -Path ./infra/eventSubscriptions.json -Force
