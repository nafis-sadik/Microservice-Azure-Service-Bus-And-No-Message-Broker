using Microservice_C;
using Microservice_C.Services;
using Microsoft.Azure.ServiceBus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IAzureServiceBusClient, AzureServiceBusClient>();

builder.Services.AddSingleton<IQueueClient>(x =>
    new QueueClient(
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("QueueName")
    )
);

builder.Services.AddSingleton<ITopicClient>(x =>
    new TopicClient(
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("TopicName")
    )
);

builder.Services.AddSingleton<ISubscriptionClient>(x =>
    new SubscriptionClient(
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("ConnectionString"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("TopicName"),
        builder.Configuration.GetSection("ServiceBus").GetValue<string>("SubscriptionName")
    )
);

builder.Services.AddHostedService<ServiceBusSubscriptionClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();