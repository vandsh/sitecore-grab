### What is it? ###
A utility to help you pull content from a server remotely.  It's like [Sitecore.Ship](https://github.com/kevinobee/Sitecore.Ship) but completely inversed.  I used Sitecore.Ship very much as my northstar on this, and I am sure the code will show that as well.

### What does it do? ###
It uses the Sitecore Serialization API to process requests and return the serialized item(s).  Security is managed thru a specified whitelist of servers that the GET commands are accepted from.

### What do I need it for? ###
TBH, not even sure at the moment.  My ultimate goal was to also include a proper Sitecore package that would allow a secondary server, lets say a QA or Dev server to issue syncs in the form of commands from a context menu or ribbon in the Sitecore shell.  Those requests would call out to the server running `Sitecore.Grab`, grab the serialized item(s) and then sync those items into the current content tree.  Right now this project contains the latter half of that vision, the service to return the serialized string.

### How do I use it ###
Should be as easy as:

1. Clone, download, whatever, just get these files into your local machine

1. Build the project on its own and add the `Sitecore.Grab.dll` or add all the source to your solution

1. Update your `web.config` by making the following changes _(all changes are also included in [web.config.transform](https://github.com/vandsh/sitecore-grab/blob/master/Sitecore.Grab/content/web.config.transform))_:
    * add ```<section name="nancyFx" type="Nancy.Hosting.Aspnet.NancyFxSection" />```
to your `<configSections>` node, 
    * add `<remove name="Nancy" /><add name="Nancy" verb="*" type="Nancy.Hosting.Aspnet.NancyHttpRequestHandler" path="/services/*" />` to the very bottom of your `<handlers>` node, 
    * add add `  <packageInstallation enabled="true" allowRemote="true" allowPackageStreaming="true" recordInstallationHistory="false">
    <Whitelist>
      <add name="local loopback" IP="127.0.0.1" />
    </Whitelist>
  </packageInstallation>` and `<nancyFx><bootstrapper assembly="Sitecore.Grab" type="Sitecore.Grab.Classes.Bases.DefaultBootstrapper, Sitecore.Grab" /></nancyFx>` to the very bottom of the `<configuration>` node in your `web.config`

1. Deploy all the things

### How do I use it? ###

1. ~~Sanity check: `http://<yourserver>/services/about`~~

1. issue a get request to `http://<yourserver>/services/package/create/items/{itemId}/{databaseName}/{boolGenerateSubItems}`

### Going forward... ###
I would like to create a well defined package to allow a secondary server to call out to a _master_ type server running the Grab service.  That package would include context menu items, ribbon commands, and some dataitems to define a configuration (what items to match on between the two servers etc). On top of that, what I would like to do is get this nearly production ready and into a NuGet package at some point in time.

