# Microservice

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

![alt text](http://www.thepollyproject.org/content/images/2016/10/Polly-Logo@2x.png)

### About this project
Ther are three web applications, each of which are meant to be treated as induvidual microservices. This project demonstrates communication between two microservices using Microsoft Azure Service Bus as message broker as well as two microservices communicating with each other using polly without any message in between.

- Communnication between Microservice A & C demonstrates use of Azure Service Bus.
- Commuinication between Microservice A & B demonstrates use of polly to implement cercuit breaker and resilient client.



## Tech

- Dotnet
- Azure
- Polly
- Swagger

## Plugins

| Nuget Package | Version |
| ------ | ------ |
| Azure.Messaging.ServiceBus | 7.8.1 |
| Microsoft.Azure.ServiceBus | 5.2.0 |
| Microsoft.Extensions.Http | 6.0.0 |
| Microsoft.Extensions.Http.Polly | 6.0.5 |
| Newtonsoft.Json | 13.0.1 |
| Swashbuckle.AspNetCore | 6.2.3 |

