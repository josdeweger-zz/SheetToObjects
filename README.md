[![Build status](https://ci.appveyor.com/api/projects/status/xyh066af9kpkqpgm?svg=true)](https://ci.appveyor.com/project/josdeweger/sheettoobjects)

# What is SheetToObjects?
A simple library which aims to provide developers with an easy solution to map sheets (Google Sheets, Microsoft Excel, csv) to a model/POCO.

# Quickstart
Below is an example on how to get started. The steps are 1. Configure, 2. Get sheet data, 3. Do the mapping
```
var sheetMapper = new SheetMapper()
    .AddConfigFor<SomeModel>(cfg => cfg
        .AddColumn(column => column.WithHeader("First Name").IsRequired().MapTo(m => m.FirstName))
        .AddColumn(column => column.WithHeader("Middle Name").MapTo(m => m.MiddleName))
        .AddColumn(column => column.WithHeader("Last Name").IsRequired().MapTo(m => m.LastName))
        .AddColumn(column => column.WithHeader("Email").Matches("^\S+@\S+$").MapTo(m => m.Email))
        .AddColumn(column => column.WithHeader("Age").WithCustomRule<int>(age => age > 18 && age <= 67).MapTo(m => m.Email))
    );

var sheet = await _googleSheetProvider.GetAsync(mySheetId, "'My SheetName'!A1:H5", myApiKey);

var result = _sheetMapper.Map<MyModel>(sheet);
```

# Documentation
[For more information, check out the wiki](https://github.com/josdeweger/SheetToObjects/wiki)
