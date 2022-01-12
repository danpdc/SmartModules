![Build status](https://github.com/danpdc/SmartModules/actions/workflows/dotnet.yml/badge.svg)

# SmartModules for .Net 6 minimal API

SmartModules is a thin library that helps you to modularize your .NET 6 minimal APIs by introducing the concept of auto-registering modules. Your module classes just need to implement the `IModule` interface and all your endpoints will be registered automatically at startup. This helps you to keep your `Program.cs` file as clean as possible and it also opens the door to easily transition to a vertical slices type of architecture. 

## Getting started

1. Install the Codewrinkles.MinimalApiSmartModules NuGet package

      Package Manager: `Install-Package Codewrinkles.MinimalApi.SmartModules -Version 0.1.3`
  
      DotNet CLI: `dotnet add package Codewrinkles.MinimalApi.SmartModules --version 0.1.3`
  
2. Create a new .NET 6 minimal API project.In the `Program.cs` file:
      - Add smart module services:
      ```csharp
      builder.Services.AddSmartModules(typeof(Program));
      ```
      - Add SmartModule endpoints and middleware:
      ```csharp
      app.UseSmartModules();
      ```
3. Create a new class that inherits `SmartModule`. Here is where you can register all your module endpoints.Here's an example for such a class that uses MediatR. You can however use regular lambdas in your endpoint registration. Everything that is supported in .NET 6 regarding endpoint registration is also supported by SmartModules.As part of inherting the SmartModule class you will need to implement the abstract method `public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder endpoints)`.
      ```csharp
      public class AuthorModule : SmartModule
    {
        private readonly ILogger<AuthorModule> _logger;
        public AuthorModule(ILogger<AuthorModule> logger)
        {
            _logger = logger;
        }
        public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/authors", async (IMediator mediator) 
                => await GetAllAuthors(mediator))
                .WithName("GetAllAuthors")
                .WithDisplayName("Authors")
                .WithTags("Authors")
                .Produces<List<AuthorGetDto>>()
                .Produces(500);

            return endpoints;
        }
        private async Task<IResult> GetAllAuthors(IMediator mediator)
        {
            var authors = await mediator.Send(new GetAllAuthorsQuery());
            return Results.Ok(authors);
        }

    }
      ```
 4. Run your minimal API and test the endpoint. 

## Endpoint definitions
When working with nested resources and complex object representations, the module class will probably get very crowded. In this scenarios, you can split your endpoint registrations in different endpoint definition classes and wire them up in your module class. An endpoint definition class should just implement the `IEndpointDefinition` marker interface. This is just a marker interface and you are free to design how exactly you would like to do the registrations. Maybe you want to do it in different methods and add your won logic to it. SmartModules is here to help! It's not opinionated. Here's an example of an endpoint definition class:

```csharp
public class BlogReadsEndpoint : IEndpointDefinition
    {
        private readonly ILogger<BlogReadsEndpoint> _logger;
        public BlogReadsEndpoint(ILogger<BlogReadsEndpoint> logger)
        {
            _logger = logger;
        }

        public IEndpointRouteBuilder RegisterRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/blogs", async (IMediator mediator) => await GetAllBlogs(mediator))
                .WithDisplayName("Blogs")
                .WithName("Get all blogs")
                .Produces<List<Blog>>()
                .Produces(500);
            _logger.LogInformation("Added endpoint: /api/blogs");
            return endpoints;
        }

        private async Task<IResult> GetAllBlogs(IMediator mediator)
        {
            var result = await mediator.Send(new GetAllBlogs());
            return Results.Ok(result);
        }

        public record Blog
        {
            public int Id { get; init; }
            public string Name { get; init; }
            public string Description { get; init; }    
            public DateTime DateCreated { get; init; }
            public Author Owner { get; init; }
        }

        public record Author
        {
            public string Name { get; init; }
            public string Bio { get; init; }
        }
    }
```
You can get the endpoint definition class into your module through dependency injection and then execute the endpoint registration methods the way you see fit!

```csharp
public class BlogsModule : IModule
    {
        private readonly ILogger<BlogsModule> _logger;
        private readonly BlogReadsEndpoint _blogReadsEndpoint;
        private readonly BlogsWritesEndpoint _blogWritesEndpoint;
        public BlogsModule(ILogger<BlogsModule> logger, BlogReadsEndpoint blogReads,
            BlogsWritesEndpoint blogWrites)
        {
            _logger = logger;
            _blogReadsEndpoint = blogReads;
            _blogWritesEndpoint = blogWrites;
        }
        public IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder endpoints)
        {
            _blogReadsEndpoint.RegisterRoutes(endpoints);
            _blogWritesEndpoint.RegisterRoutes(endpoints);
            return endpoints;
        }
    }
```

## Async modules
There were some requests for the possibility to add modules in async way. Reason is that there might be cases when you want to perform some async work in the module registration implementation. To achieve this, we have also added the `SmartAsyncModule` base class. Just make sure that you are using this async base class if you need to register smart async modules. 

```csharp
public class AsyncTestModule : SmartAsyncModule
    {
        private readonly DummyService _dummyService;
        public AsyncTestModule(DummyService dummyService)
        {
            _dummyService = dummyService;
        }
        public override async Task<IEndpointRouteBuilder> MapEndpointDefinitionsAsync(IEndpointRouteBuilder app)
        {
            app.MapSmartHead("/api/async", () => "Response to HEAD method").WithName("HeadAsync").WithDisplayName("Sample tests");
            app.MapSmartOptions("/api/async", () => "Response to OPTIONS method").WithName("OptionsAsync").WithDisplayName("Sample tests");
            app.MapSmartPatch("/api/smartpatch", () => "Response from smart patch");
            await Task.Delay(100);
            return app;
        }
    }
```

## Dependency injection
SmartModules relies on the standard ASP.NET Core DI container. Modules and endpoint definitions are registered as transient services. It's important to note here that even if we'd add them as scoped, all the added services would de facto behave as singletons, as they are constructed during the startup phase of the application. It's in a way very similar to middleware. 

Therefore, in all your modules and endpoint definitions you can inject the services you want or need just like in regular controllers. 

If you need scoped services inside your modules or endpoint definitions, then inject an `IServiceProvider` instance, create a scope and resolve your service. Here's an example for that: 

```csharp
public class BlogsModule : IModule
    {
        private readonly IServiceScopeFactory _serviceScopeFactory; 
        public BlogsModule(ILogger<BlogsModule> logger, BlogReadsEndpoint blogReads,
            BlogsWritesEndpoint blogWrites, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder endpoints)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ThoughtfulDbContext>();
            var blogs = context.Blogs.ToList();

            return endpoints;
        }
    }
```
## Support for HEAD and OPTIONS HTTP methods
HEAD and OPTIONS HTTP methods are not supported out of the box via a `app.MapHead` construct. It's true that you can add them with `pp.MapMethods()`, but to keeps things consistent, SmartModules brings out of the box support for these HTTP methods. 

```csharp
public class TestModule : IModule
    {
        public IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapHead("/api", () => "Response to HEAD method").WithName("Head").WithDisplayName("Sample tests");
            app.MapOptions("/api",() => "Response to OPTIONS method").WithName("Options").WithDisplayName("Sample tests");
            return app;
        }
    }
```

## Smart endpoint registration
You can use the standard way of registering endpoints that is specific for .NET 6 minimal API. However, I encourage you to use the SmartModule extensions methods that we have created for this purpose. We want to continue developing further features like action filters (which are not supported by default in .NET 6 minimal API) and you'll not be able to use these futurea features if you're not using our provided extension methods.

```csharp
public override IEndpointRouteBuilder MapEndpointDefinitions(IEndpointRouteBuilder app)
        {
            app.MapSmartGet("/api/smart", () => "Return from smart GET")
                .WithDisplayName("Smart endpoints")
                .Produces(200)
                .Produces<string>();
            app.MapSmartPost("/api/smart", () => "Return from smart POST")
                .WithDisplayName("Smart endpoints");
            app.MapSmartPut("/api/smart", () => "Return from smart PUT")
                .WithDisplayName("Smart endpoints");
            app.MapSmartDelete("/api/smart", () => "Return from smart DELETE")
                .WithDisplayName("Smart endpoints");
            app.MapSmartPatch("/api/smart", () => "Return from smart PATCH")
                .WithDisplayName("Smart endpoints");
            app.MapSmartHead("/api/smart", () => "Return from smart HEAD")
                .WithDisplayName("Smart endpoints");
            app.MapSmartOptions("/api/smart", () => "Return from smart OPTIONS")
                .WithDisplayName("Smart endpoints");
            return app;
        }
    }
```


## Samples
This is a [getting started](https://github.com/danpdc/cwk.MinimalApis.SmartModules/tree/main/Sample) test application. 
And here is a more [advanced](https://github.com/danpdc/ThoughtFul) sample that implements more complex modules with endpoint definitions, a vertical slices architecture leveraging the power of CQRS through the MediatR library. 

## Your feedback
This is for now a fairly small library with basic functionalities. I have some ideas on how the extend it but this is the part where I count on your feedback. Please don't hesitate to open issues for new feature requests, questions. 

Further, if you want to contribute, just get in touch. 
