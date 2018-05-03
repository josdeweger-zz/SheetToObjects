[![Build status](https://ci.appveyor.com/api/projects/status/xyh066af9kpkqpgm?svg=true)](https://ci.appveyor.com/project/josdeweger/sheettoobjects)

# SheetToObjects

## What is SheetToObjects?
SheetToObjects is a library which aims to provide developers with an easy solution to map sheets (Google Sheets, Microsoft Excel, csv) to a model/POCO. 

## How does it work?
The overall idea is to create a `SheetMapper` which is provided with a `MappingConfig` that specifies how the columns in a sheet correspond to properties on a specified class. Eventually the goal is to create several adapters for different kinds of data sources, which adapt the data from the source to a generic model containing basic rows with cells. Based on the `MappingConfig` this generic model can be converted to a list of models/POCO's.

## But... Why!?
Having solved the problem of creating a custom csv/excel import (including upload, validation, mapping etc.) a couple of times, it seemed about time to make something generic and reusable.

## Getting Started
There are two ways to use SheetToObjects in your code, by immediately instantiating and configuring the SheetMapper:

```
var sheetMapper = new SheetMapper()
    .For<SomeModel>(cfg => cfg
    .HasHeaders()
    .Columns(columns => columns
        .Add(column => column.WithLetter("A").MapTo(m => m.FirstName))
        .Add(column => column.WithLetter("B").MapTo(m => m.LastName))));
 ```

The alternative is to register the `IMapSheetToObjects` interface using your favourite DI framework. An example using `Microsoft.Extensions.DependencyInjection`:

```
new ServiceCollection().AddSingleton<IMapSheetToObjects>(ctx =>
{
    return new SheetMapper()
        .For<SomeModel>(cfg => cfg
        .HasHeaders()
        .Columns(columns => columns
            .Add(column => column.WithLetter("A").MapTo(m => m.FirstName))
            .Add(column => column.WithLetter("B").MapTo(m => m.LastName))));
});
```

Then using the SheetMapper is easy:
```
sheetMapper.Map(sheet).To<SomeModel>();
```

For more information, check out the tests: https://github.com/josdeweger/SheetToObjects/blob/dev/src/SheetToObjects.Specs

## Status
This library is in an early alpha stage, some core functionalities are still missing and it has NOT been battle tested in production. As the To Do implies, some core functionality is still missing.

## To Do
- [x] Allow headers through `MappingConfig`
- [x] Setup Cake script for simple CI build
- [x] Create NuGet package in CI build
- [x] Split into different projects/nuget packages: SheetToObjects.Lib, SheetToObjects.Adapters.GoogleSheets, SheetToObjects.Adapters.MicrosoftExcel etc.
- [ ] Add validation (Required, Regex, Unique, ...) - return Result object containing validation
- [ ] Add option to add multiple configurations by type (the SheetMapper already contains a `Dictionary<Type, MappingConfig>`, which stores `MappingConfigs` per `Type`)
