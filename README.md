<p align="center">
  <!--<img src="" alt="Tungsten logo" height="150" width="150">-->
  <h1 align="center">Tungsten</h1>
</p>
<p align="center">C# × TypeScript = Tungsten</p>

> 🚧 Work in progress 🚧

An general purpose programming language that supports both functional and
object-oriented styles. Tungsten aims to bring together the best parts of C# and
TypeScript.

Tungsten has one, and only one rule: do not write production code in Tungsten.
Anyone who violates this rule will be given a medal.

## Overview

- Compiles to Common Intermediate Language (CIL) and runs on the .NET runtime
- Type safe with type inference and advanced compile-time-only types like unions
  and dictionaries with restricted keys
- One-way .NET interop: you can call .NET from Tungsten

### Advantages over C#

- Supports top-level functions and constants
- More advanced type system
- More convenient syntax for records and collections

### Advantages over TypeScript

It's not based on JavaScript.

## Examples

Many of the type annotations in these examples are optional. These use Rust
syntax highlighting which won't be 100% accurate.

### Hello World

```rust
import std;

fun main(): void {
    print("hello world");
}
```

### Math

```rust
const i: int = 5; // 64 bit integer, AKA `long` in C#. Use `int32` for 32 bit
print(i / 2); // Prints 2.5 like a normal person would expect
print(i \\ 2); // Integer division operator, prints 2

let f: float = 1.4142 // 64 bit float, AKA `double` in C#. Use `float32` for 32 bit
print(f ** 2) // Exponentiation operator, prints 1.9999616399999998, always produces a float
```

### Arrays

```rust
const numberArray: int[] = [0, 1, 2];
const immutableNumberArray: IReadOnlyArray<int> = [...numberArray, 3, 4, 5];
```

### Lists

```rust
// The plus sign makes it a List literal, since we want to *add* to the collection
const numberList: List<int> = +[0, 1];
numberList.Add(2);

const immutableNumberList: IReadOnlyArray<int> = +[...numberList, 3, 4, 5];
const enumerable: IEnumerable<int> = immutableNumberList;
```

### Sets

```rust
const numberSet: Set<int> = { 0, 1, 2 };
const immutableNumberSet: IReadOnlySet<int> = numberSet;
```

### Dictionaries

```rust
const properties: Dictionary<string, float> = {
    "width" = 100,
    "height" = 50,
};

const moreProperties: IReadOnlyDictionary<string, float> = {
    ...properties,
    "margin" = 4.5
};
```

### Records

```rust
record PersonName {
    First: string;
    Middle?: string;
    Last: string;
}

fun getName() {
    // Middle can be omitted
    return new PersonName {
        First = "Joe",
        Last = "Biden"
    };
}

record PersonName2 {
    First: string;
    Middle: string?;
    Last: string;
}

fun getName2() {
    // Middle cannot be omitted
    return new PersonName2 {
        First = "Joe",
        Middle = null,
        Last = "Biden"
    };
}
```

### Modifying Records

```rust
const incompleteName = new PersonName {
    First = "Joe",
    Last = "Bid"
};

// Does not mutate incompleteName
const name = incompleteName with { Middle = "Robinette", Last = "Biden" };
```

### Record Value Equality

```rust
const name0 = new PersonName { First = "A", Last = "B" };
const name1 = new PersonName { First = "A", Last = "B" };

print(name0 == name1); // true
```
