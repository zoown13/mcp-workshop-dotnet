param name string

param subscriptions array = [
  {
    name: 'evtgrd-${name}-subscription-{0}-{1}'
    endpointType: 'WebHook'
    endpointUrl: 'https://random-7071.location.devtunnels.ms/runtime/webhooks/EventGrid?functionName={functionName}'
    includedEventTypes: [
      'Microsoft.Resources.ResourceWriteSuccess'
      'Microsoft.Resources.ResourceDeleteSuccess'
    ]
    eventDeliverySchema: 'CloudEventSchemaV1_0'
  }
]

var eventgrid = {
  name: 'evtgrd-${name}-systemtopic'
  subscriptions: subscriptions
}

resource evtgrdtopic 'Microsoft.EventGrid/systemTopics@2023-06-01-preview' existing = {
  name: eventgrid.name
}

resource evtgrdsubscriptionwebhook 'Microsoft.EventGrid/systemTopics/eventSubscriptions@2023-06-01-preview' = [for sub in eventgrid.subscriptions: if (sub.endpointType == 'WebHook') {
  name: format(sub.name, replace(toLower(sub.endpointType), '_', ''), replace(toLower(sub.eventDeliverySchema), '_', ''))
  parent: evtgrdtopic
  properties: {
    destination: {
      endpointType: sub.endpointType
      properties: {
        endpointUrl: sub.endpointUrl
      }
    }
    filter: {
      includedEventTypes: sub.includedEventTypes
    }
    eventDeliverySchema: sub.eventDeliverySchema
  }
}]

resource evtgrdsubscriptionfunction 'Microsoft.EventGrid/systemTopics/eventSubscriptions@2023-06-01-preview' = [for sub in eventgrid.subscriptions: if (sub.endpointType == 'AzureFunction') {
  name: format(sub.name, replace(toLower(sub.endpointType), '_', ''), replace(toLower(sub.eventDeliverySchema), '_', ''))
  parent: evtgrdtopic
  properties: {
    destination: {
      endpointType: sub.endpointType
      properties: {
        resourceId: sub.resourceId
      }
    }
    filter: {
      includedEventTypes: sub.includedEventTypes
    }
    eventDeliverySchema: sub.eventDeliverySchema
  }
}]
