# C# Coding Style Guide

Guide that describes code styling practices used in Horizon Guide

## General Recommendation

If a style is not covered here, then follow [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) or [Google's C# Style Guide](https://google.github.io/styleguide/csharp-style.html). When in doubt, follow existing code base conventions.

## Naming Rules

### Code

- Names of classes, methods, enumerations, public fields, namespaces: PascalCase
- Names of local variables, parameters, including private and protected fields: camelCase
- Names of private and protected fields: _camelCase
- Interface names start with the letter 'I'

### Files

- Names of files and directories: PascalCase

## Organization

- Use modifiers in the following order: public protected internal private new abstract virtual override sealed static readonly extern unsafe volatile async

- Namespace 'using' declarations go at the top before namespaces

- 'using' imports are in alphabetical order, except System imports go first

## Commenting

- Place comments on a separate line, not at the end of a line of code
- Begin comment with an uppercase letter
- Do not end comments with a period
- Insert one space between comment delimiter (//) and the comment

```c#
// This is an example comment
```

## Implicitly Typed Local Variables

- Avoid implicit typing when possible, unless typing is obvious
  - Do not assume type is clear from method names

## Whitespace, Tab & Curly Braces

- Maximum of one statement per line
- Column limit: 150
- Each indent done by one tab
- No line break before opening brace
- No line break between closing brace and else
- Always use braces even when optional
- Space after if, for, while, etc.

```c#
if (true) {
    // Do stuff here
}
```

## Constants

- Variables and fields that can be made const should always be made const
- If const isn't possible, use readonly instead
- Use constants instead of magic numbers

## Folders and file locations

- Prefer a flat structure where possible

## Namespaces

- In general, namespaces should be no more than 2 levels deep
- Don't force file/folder layout to match namespaces
- 