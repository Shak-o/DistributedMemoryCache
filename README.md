# DistributedMemm: Distributed Memory Caching using RabbitMQ

## Introduction
DistributedMemm is a C# library designed to provide distributed memory caching capabilities for .NET applications. By leveraging the power of RabbitMQ, this library ensures seamless data synchronization between multiple applications. In the event of cache data loss, DistributedMemm provides data recovery features using MongoDB.

## Features

- **Distributed Caching**: Extend the benefits of in-memory caching across multiple applications or instances.
- **Data Synchronization**: Using RabbitMQ, DistributedMemm ensures that data changes are broadcast and synchronized across all connected applications.
- **Idempotency**: Ensures that operations are processed once and only once, preventing duplicate data and actions.
- **Data Recovery**: With the backup mechanism in MongoDB, applications can restore their memory cache on startup, ensuring minimal downtime and data loss.
- **Integrated API**: An additional API that consumes RabbitMQ events to maintain and update MongoDB backup data.

## Prerequisites

- .NET Core or .NET 6+
- RabbitMQ Server
- MongoDB (if persist option enabled)

## Installation

[Provide steps or code snippets to install and set up the library in a project]

## Quick Start

1. **Configuration**: Set up your RabbitMQ and MongoDB configurations.

    ```csharp
    var config = new DistriCacheConfig
    {
        RabbitMQConnectionString = "YOUR_RABBITMQ_CONNECTION_STRING",
        MongoDBConnectionString = "YOUR_MONGODB_CONNECTION_STRING"
    };
    ```

2. **Initialize DistributedMemm**:

    ```csharp
    private readonly IDistributedMemm _distributedMemm;
   
   public TestService(IDistributedMemm distributedMemm)
   {
        _distributedMemm = distributedMemm;
   }
    ```

3. **Usage**:

    ```csharp
    _distributedMemm.Add("key", "value");
    _distributedMemm.Add("key", new { Name = "jhon", SurName = "doe" });
    var value = _distributedMemm.Get("key");
    ```

## Data Recovery

In case application restarts our library will try to restore data by calling reserve api.

## Contributing

We welcome contributions! Please see our [CONTRIBUTING.md](link-to-contributing) for details.

## License

This project is licensed under [LICENSE NAME]. See the [LICENSE](link-to-license) file for details.