Extensions:

1-Bing Maps sdk: Install Bing.Maps.vsix v 1.313.825.0, You will find it inside Extensions\Bing.Maps folder.
Site:https://visualstudiogallery.msdn.microsoft.com/224eb93a-ebc4-46ba-9be7-90ee777ad9e1

2-ReswCodeGen:Not nessecsary if you aren't going to modify resources.resw.
  Read NoteAboutReswCodeGen.txt for more info.

Dependencies:

1-NewtonSoft.Json: installed through Nuget, PM> Install-Package Newtonsoft.Json
 ->Will be used to deserialize repsonses from Flickr.Json (Better Performance than internal Microsoft serializers)

2-Autofac: Installed through, PM> Install-Package Autofac
 ->Autofac Inversion of Control container used for dependency injection in ViewModel Locator

