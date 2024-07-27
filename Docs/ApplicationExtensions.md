# ApplicationExtensions
## Configuring Application Startup in Windows Registry

There are several ways to configure an application to run on Windows startup, and one common method is by registering the application in the Windows Registry using the following key:

```
HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
```

In the **CodeArtEng.Extensions** library (version 1.5.0), a useful feature called `ApplicationExtensions` is introduced to help register or unregister a running application in the Windows Registry.

### Registering an Application for Startup

To add your application to startup, you can use the following method:

```csharp
ApplicationExtensions.AddApplicationToStartup();
```

This method is particularly handy when dealing with ClickOnce applications, as their installation paths can be dynamic.

### Removing an Application from Startup

If you want to remove your application from startup, use the following method:

```csharp
ApplicationExtensions.RemoveApplicationFromStartup();
```

### Handling ClickOnce Application Updates

When a ClickOnce application is updated to a newer version, its execution path may change. Consequently, the registry key's value needs to be updated to ensure that the auto-start functionality continues to work. Fortunately, with the **CodeArtEng.Extensions** package, you can simply call the method:

```csharp
ApplicationExtensions.CheckAndUpdateApplicationStartupPath();
```

This method will update the registry key if your application had been previously registered to run on Windows startup.
