using Microservice_A.Models;
using Microservice_A.ServiceClient;
using Microservice_A.Services;
using Microsoft.Azure.ServiceBus;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<IAPIClientFactory<WeatherModel>, APIClientFactory<WeatherModel>>().AddPolicyHandler(GetRetryPolicy()).AddPolicyHandler(GetCircuitBreakerPolicy());
builder.Services.AddSingleton<IAzureServiceBusPublisherClient, AzureServiceBusPublisherClient>();

// To send messages to ques
builder.Services.AddSingleton<IQueueClient>(x =>
    new QueueClient(builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
    builder.Configuration.GetSection("ServiceBus").GetValue<string>("QueueName"))
);

// To send messages to topics
builder.Services.AddSingleton<ITopicClient>(x =>
    new TopicClient(builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
    builder.Configuration.GetSection("ServiceBus").GetValue<string>("TopicName"))
);

// To recieve messages from ques
builder.Services.AddSingleton<ISubscriptionClient>(x =>
    new SubscriptionClient(
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("TopicName"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("SubscriptionName")
    )
);

// My client that subscribes to azure service bus topics to recieve messages from topics
builder.Services.AddHostedService<AzureServiceBusSubscriptionClient>();

IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.IsSuccessStatusCode != true).WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
