param name string
param location string = resourceGroup().location

param tags object = {}

param topicType string
param source string

var eventgrid = {
  name: 'evtgrd-${name}-systemtopic'
  location: location
  tags: tags
  topicType: topicType
  source: source
}

resource evtgrdtopic 'Microsoft.EventGrid/systemTopics@2023-06-01-preview' = {
  name: eventgrid.name
  location: eventgrid.location
  properties: {
    source: eventgrid.source
    topicType: eventgrid.topicType
  }
}
