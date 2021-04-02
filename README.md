# CoDAPI
A client for the CoD API, written in C#

### Example

```csharp
using System;
using CoDAPI;

class Program
{
    static async Task Main()
    {
      var auth = new Authenticate();
      auth.Login("email", "password");

      var API = new CoDAPI();
      API.Auth = (ref auth);
      
      var response = await API.Profile("v1", "mw", "uno", "HusKerrs", "wz");     
    }
}
