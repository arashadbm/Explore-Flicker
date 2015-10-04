Dependencies:

1-NewtonSoft.Json: installed through Nuget, PM> Install-Package Newtonsoft.Json
 ->Will be used to deserialize repsonses from Flickr.Json (Better Performance than internal Microsoft serializers)

2-Autofac: Installed through, PM> Install-Package Autofac
 ->Autofac Inversion of Control container used for dependency injection in ViewModel Locator