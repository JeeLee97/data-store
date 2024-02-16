# DataStore - Class serialization and data store interface for Unity

DataStore is a lightweight class serialization interface to allow for easy unified interaction with classes that you want serialized in Unity. Includes useable example of local data store implementation. The system is easily extendable to be used with all kinds of other methods of storing data. The system is also flexible enough to be used in all kinds of frameworks including dependency injection frameworks.

# Table of Contents

- [Installation](#installation)
- [Quick Start](#quick-start)
- [Extending Functionality](#extending-functionality)
- [Contributing](#contributing)

# Installation

Currently the best way to include this package in your project is through the unity package manager. Add the package using the git URL of this repo: https://github.com/justinleemans/data-store.git

# Quick Start

You can use the included local data store straight out of the box. Just create an instance with a type that you want to serialize and you are done.

```c#
IDataStore<YourDataClass> dataStore = new LocalDataStore<YourDataClass>();
```

On this instance you can now call the available methods to handle your data or interact with the `Data` property itself. Note that `Load()` is not necessary to call when no value is supplied or set to true when creating your instance of `LocalDataStore` because it will be automatically loaded on initialization. You can of course call this method to override the data object with data that has been stored earlier.

```c#
bool success = dataStore.Save(); // Saves data to the store
bool success = dataStore.Load(); // Loades data from the store
bool success = dataStore.Delete(); // Deletes the data from the store
dataStore.Clear(true); // Clears and resets the data class, has optional overload to persist data to your data store

var bar = dataStore.Data.Foo;
dataStore.Data.Foo = bar;
```

Keep in mind that if you create multiple instances of a data store for the same data class this could result in these data stores having different results since saving on one will not be updated in another.

# Extending Functionality

If you want to store your data some other way than through the included local store you can do this by making your own data store extending from `DataStore.cs` and including the required functionality to store the data. You can take a look at the included `LocalDataStore.cs` for an example on how to implement functionality.

## Constructor

First we extend from `DataStore`. Note that `DataStore` has a generic type included which means we can use this to store any kind of class we want.

Also note that the base constructor requires a boolean value that indicates if the data should be attempted to be loaded once an instance of the class is created. This constructor can also be used to initialize some other required behaviour like getting an authentication key for access to a remote data store.

```c#
public class ExampleDataStore<T> : DataStore<T>
	where T : class, new()
{
	public ExampleDataStore(bool loadOnInitialize = true) : base(loadOnInitialize)
	{
	}
}
```

## Required data handling methods

Next we override the abstract methods which define the actions the data store should take. These methods have a boolean as parameter called `success` which needs to be set. The value of this is used to communicate back if the process was successfull. For example if you want to show a prompt when saving data has failed.

```c#
protected override void OnSave(out bool success)
{
	success = true;
}

protected override void OnLoad(out bool success)
{
	success = true;
}

protected override void OnDelete(out bool success)
{
	success = true;
}
```

You can make use of the `Data` property to set the data class which is of the generic type you set when creating an instance of this data store.

## Optional methods

Finally there is an optional virtual method that you can override called `OnClear`. This method handles resetting the serialized class to a clean state. In this method you can set the value of the `Data` property to anything what you consider a clean state and handle anything regarding resetting the data class.

```c#
protected override void OnClear()
{
}
```

# Contributing

Currently I have no set way for people to contribute to this project. If you have any suggestions regarding improving on this project you can make a ticket on the GitHub repository or contact me directly.
