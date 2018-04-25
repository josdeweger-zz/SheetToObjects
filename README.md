# SheetToObjects

## What is SheetToObjects?
SheetToObjects is a library which aims to provide developers with an easy solution to map sheets (Google Sheets, Microsoft Excel, csv) to a model/POCO. 

## How does it work?
The overall idea is to create a `SheetMapper` which is provided with a `MappingConfig` that specifies how the columns in a sheet correspond to properties on a specified class. Eventually the goal is to create several adapters for different kinds of data sources, which adapt the data from the source to a generic model containing basic rows with cells. Based on the `MappingConfig` this generic model can be converted to a list of models/POCO's.

## But... Why!?
Having solved the problem of creating a custom csv/excel import (including upload, validation, mapping etc.) a couple of times, it seemed about time to make something generic and reusable.

## Getting Started
There are two ways to use SheetToObjects in your code, by immediately instantiating and configuring the SheetMapper through a static Create method:

```
var sheetMapper = SheetMapper.Create(cfg => cfg
                    .For<MyNumberNameModel>()
                    .Column("A").MapTo(m => m.Number)
                    .Column("B").MapTo(m => m.Name)
                    .Build())
 ```

The alternative is to register the `IMapSheetToObjects` interface using your favourite DI framework. An example using `Microsoft.Extensions.DependencyInjection`:

```
new ServiceCollection().AddSingleton<IMapSheetToObjects>(ctx =>
{
    return SheetMapper.Create(cfg => cfg
        .For<MyNumberNameModel>()
        .Column("A").MapTo(m => m.Number)
        .Column("B").MapTo(m => m.Name)
        .Build());
});
```

## Status
This library is in an early alpha stage, some core functionalities are still missing and it has NOT been battle tested in production. As the To Do implies, some core functionality is still missing.

## To Do
- [ ] Add validation (Required, Regex, Unique, ...) - return Result object containing validation
- [ ] Allow headers through `MappingConfig`
- [ ] Add different mapping config types (CSV config will probably be different)
- [ ] Setup Cake script for simple CI build
- [ ] Create NuGet package in CI build
- [ ] Split into different repo's: SheetToObjects.Lib, SheetToObjects.Adapters.GoogleSheets, SheetToObjects.Adapters.Csv etc.
