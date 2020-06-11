# Folder structure
This repository is structured in the following way
```
+---docs
+---pipelines
+---src
|   +---apps
|   +---data
|   +---features
+---tests
|   +---apps
|   +---data
|   +---features
```

More folder might be added in the future. 

## Solution structure

The solution structure in Visual Studio should reflect the folder structure. This is done so that files can easily be found. The solution is therefor setup in the following way:

```
+---pipelines
+---Solution Items
+---src
|   +---apps
|   +---data
|   +---features
```

Two exception in the above rule can be viewed. Tests project are seperated on disk but are not seperated in the solution. This is too keep the tests close to the code but in a way that can different coding rules (for example disabling the C#8 nullable feature) can be applied.

The second exception in the `Solution Items` folder which contains all the files inside the root folder of the project.