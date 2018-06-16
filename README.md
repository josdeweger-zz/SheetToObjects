[![Build status](https://ci.appveyor.com/api/projects/status/xyh066af9kpkqpgm?svg=true)](https://ci.appveyor.com/project/josdeweger/sheettoobjects)

# SheetToObjects

## What is SheetToObjects?
A simple library which aims to provide developers with an easy solution to map sheets (Google Sheets, Microsoft Excel, csv) to a model/POCO. 

## How does it work?
The overall idea is to create a `SheetMapper` which can be configured HOW to map each row in a sheet to a model. Currently the following sheet types are supported:

- Comma Seperated Value files (.csv), via nuget pakage SheetToObjects.Adapters.Csv
- Google Sheets, via nuget pakage SheetToObjects.Adapters.GoogleSheets
- Microsoft Excel, via nuget pakage SheetToObjects.Adapters.MicrosoftExcel

## But... Why!?
Having solved the problem of creating a custom csv/excel import (including upload, validation, mapping etc.) a couple of times, it seemed about time to make something generic and reusable.

## Configuration
There are two ways to configure SheetToObjects. Via fluent configuration or attributes on the model you are trying to map to. It is possible to setup multiple configurations, as long as the type you are mapping to is different.

### Fluent Configuration
An example of configuring the `SheetMapper` using the fluent api:

```
var sheetMapper = new SheetMapper()
    .AddConfigFor<SomeModel>(cfg => cfg
        .AddColumn(column => column.WithHeader("First Name").IsRequired().MapTo(m => m.FirstName))
        .AddColumn(column => column.WithHeader("Last Name").IsRequired().MapTo(m => m.LastName)));
 ```
### Configuration using Attributes
Another way to map configuration to model is by adding DataAtributes to your model:

```
[SheetToObjectConfig(sheetHasHeaders:true)]
public class ToModel
{

	[MappingByHeader("StringValue")]
	[IsRequired]
	[Regex(@"/^[a-z]+[0-9_\/\s,.-]+$", true)]
	public string StringProperty { get; set; }
}
```

At the moment there is only one class attribute: SheetToObjectConfig. With this attribute u can set the default settings to map this model.

The following default validation attributes are available as attributes;
[IsRequired]
[Regex]

The model attributes can be overwritten by configuring a config for the model on the used SheetMapper

### More on configuration
There are multiple ways to map a column in the datasource to the model property

##### By Index
Use the .WithColumnIndex or [MappingByIndex] attribute to map the property based on Index. The index is 0-based

##### By Letter
Use the .WithColumnLetter() or [MappingByLetter] attribute to map the attribute base on "Excel-style" column naming. column "A" is the first column and "D" the fourth.

##### By ColumnName
When the datasource contains a first row with headers it's possible to map by name. Use the .WithHeader() or [MappingByHeader] to map by the name that is used on the first row

##### AutoMapping property
It's also possible to automap the properties based on their name without configuring anything. A header row is required for this feature, which name needs to match the name of the property you need that column to be mapped to. 
When u don't want the property to be mapped use the `[IgnorePropertyMapping]` attribute on the property.

For more information, check out the tests: https://github.com/josdeweger/SheetToObjects/blob/dev/src/SheetToObjects.Specs

## Doing the actual mapping
The actual mapping is easy, tell your instance of the `SheetMapper` to map a sheet to a `Type`:
```
MappingResult result = sheetMapper.Map<SomeModel>(sheet); //contains successfully parsed models and validation errors
```

## Dependency Injection
You have the option of creating and configuring a new `SheetMapper` instance every time you need one, but this might become tedious pretty fast. It will also decrease testability, 
so it might be a good idea to use the Dependency Injection Framework of your choice to register `SheetToObjects`. An example using `Microsoft.Extensions.DependencyInjection`:

```
new ServiceCollection().AddSingleton<IMapSheetToObjects>(ctx =>
{
    return new SheetMapper()
        .For<SomeModel>(cfg => cfg
        .Columns(columns => columns
            .Add(column => column.WithHeader("First Name").MapTo(m => m.FirstName))
            .Add(column => column.WithHeader("Last Name").MapTo(m => m.LastName))));
});
```
