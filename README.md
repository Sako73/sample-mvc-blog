# cv-ex
Sample blog application for Cooper Vision

## Notes:

* To use the SQL client, copy the ".\src\SQLite\3_WebMvcApp\WebMvcApp.db" to a location on the hard drive, and update the **Web.config > configuration > connectionStrings > sqlite** key with the value, and update the **Web.config > configuration > appSettings > DataSource** value to "SQL".
* To use the Static client, update the **Web.config > configuration > appSettings > DataSource** value to "static".
* The project is using Ninject dependency injection.
* The project is using the Visual Studio Unit Tests, and the static data provider.
* The bootswatch "Flatly" theme is the base Bootstrap theme.
* Bootstrap is version 3 because version 4 does not support IE8.