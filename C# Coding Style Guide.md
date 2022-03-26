# C# Coding Style Guide

Guide that describes code styling practices used in Horizon Guide

## General Recommendation

If a practice is not covered here, then follow [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) or [Google's C# Style Guide](https://google.github.io/styleguide/csharp-style.html). When in doubt, follow existing code base conventions.

## Naming Rules

### Code

- Names of classes, methods, enumerations, public fields, namespaces: PascalCase
- Names of local variables, parameters, including private and protected fields: camelCase
- Interface names start with the letter 'I'

### Files

- Names of files and directories: PascalCase

## Organization

- Use modifiers in the following order: public protected internal private new abstract virtual override sealed static readonly extern unsafe volatile async

- Namespace 'using' declarations go at the top before namespaces

- 'using' imports are in alphabetical order, except System imports go first

## Curly Braces & Parentheses

- Leave one curly brace in-line and the other on a new line
- Leave a space between curly braces and parentheses, class names, etc.
- Leave a space between if-else keywords and the condition's starting parentheses

```c#
if (true) {
    // Do stuff here
}
```

## Commenting

- Place comments on a separate line, not at the end of a line of code
- Begin comment with an uppercase letter
- Do not end comments with a period
- Insert one space between comment delimiter (//) and the comment

```c#
// This is an example comment
```

## Implicitly typed local variables

- Avoid implicit typing when possible, unless typing is obvious
  - Do not assume type is clear from method names
