[![Build status](https://ci.appveyor.com/api/projects/status/xyh066af9kpkqpgm?svg=true)](https://ci.appveyor.com/project/josdeweger/sheettoobjects)

# SheetToObjects

## What is SheetToObjects?
SheetToObjects is a library which aims to provide developers with an easy solution to map sheets (Google Sheets, Microsoft Excel, csv) to a model/POCO. 

## How does it work?
The overall idea is to create a `SheetMapper` which is provided with a `MappingConfig` that specifies how the columns in a sheet correspond to properties on a specified class. Eventually the goal is to create several adapters for different kinds of data sources, which adapt the data from the source to a generic model containing basic rows with cells. Based on the `MappingConfig` this generic model can be converted to a list of models/POCO's.

## But... Why!?
Having solved the problem of creating a custom csv/excel import (including upload, validation, mapping etc.) a couple of times, it seemed about time to make something generic and reusable.

## Getting Started
There are multiple ways to use SheetToObjects in your code, by immediately instantiating and configuring the SheetMapper:

```
var sheetMapper = new SheetMapper()
    .For<SomeModel>(cfg => cfg
    .Columns(columns => columns
        .Add(column => column.WithHeader("First Name").MapTo(m => m.FirstName))
        .Add(column => column.WithHeader("Last Name").MapTo(m => m.LastName))));
 ```

An alternative is to register the `IMapSheetToObjects` interface using your favourite DI framework. An example using `Microsoft.Extensions.DependencyInjection`:

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

Another way to map configuration to model is by DataAtributes see:

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

## Column Mapping
There are multiple ways to map a column in the datasource to the model property

###### By Index
Use the .WithColumnIndex or [MappingByIndex] attribute to map the property based on Index. The index is 0-based

###### By Letter
Use the .WithColumnLetter() or [MappingByLetter] attribute to map the attribute base on "Excel-style" column naming. column "A" is the first column and "D" the fourth.

###### By ColumnName
When the datasource contains a first row with headers it's possible to map by name. Use the .WithHeader() or [MappingByHeader] to map by the name that is used on the first row

###### AutoMapping property
It's also possible to automap the properties based on their name without configuring anything. A headerrow is required for this feature. 
When u don't want to property to be mapped use the [IgnorePropertyMapping] attribute on the property.

## With this info using the SheetMapper is easy:
```
MappingResult result = sheetMapper.Map(sheet).To<SomeModel>(); //contains successfully parsed models and validation errors
```

For more information, check out the tests: https://github.com/josdeweger/SheetToObjects/blob/dev/src/SheetToObjects.Specs

## Status
This library is in an early alpha stage, some core functionalities are still missing and it has NOT been battle tested in production. As the To Do implies, some core functionality is still missing.

## To Do
- [x] Allow headers through `MappingConfig`
- [x] Setup Cake script for simple CI build
- [x] Create NuGet package in CI build
- [x] Split into different projects/nuget packages: SheetToObjects.Lib, SheetToObjects.Adapters.GoogleSheets, SheetToObjects.Adapters.MicrosoftExcel etc.
- [x] Add columns based on header instead of columnletter
- [x] Add method to csv adapter to accept stream
- [x] Return Result object containing successfully parsed models and parsing/validation messages
- [x] Required validation
- [x] Regex validation
- [ ] Configuration validation
- [ ] DateTime parsing
- [ ] Min/max validation
- [ ] Unique validation
- [ ] Multiple configurations by type (the SheetMapper already contains a `Dictionary<Type, MappingConfig>`, which stores `MappingConfigs` per `Type`)
- [ ] Nested objects support
- [ ] Add comments to MappingConfigBuilder and underlying methods for better Intellisense experience
- [ ] Base64 encoded string as input for CSV adapter
- [ ] Create Excel adapter
