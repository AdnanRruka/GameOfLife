# Coding Standards

Short rules for this solution. The goal is code that is easy to read, easy to test and easy to explain.

## 1. Structure

- Each task has two projects, plus one test project:
  - `X.Model`: all logic. No `Console` calls in here.
  - `X.ConsoleApp` (or a GUI project): reads input, calls the model, shows output and errors.
  - `X.Tests`: xUnit tests for `X.Model`.
- No interfaces, dependency injection or extra layers unless a real need shows up. The tasks are small, and extra layers only add code to read.
- One public class per file. The file name matches the class name.

## 2. Framework and project settings

- Target framework: `net10.0` (the only SDK installed, and an LTS version).
- `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>` in every project.
- No NuGet packages except the test packages (xUnit, test SDK).

## 3. Naming and style

Follow the standard Microsoft C# conventions:

| What | Style | Example |
|---|---|---|
| Classes, methods, properties | PascalCase | `Add`, `ToRoman` |
| Parameters, local variables | camelCase | `numbers`, `delimiter` |
| Private fields | _camelCase | `_cells` |
| Constants | PascalCase | `MaxNumber` |

- File-scoped namespaces: `namespace StringCalculator.Model;`
- Use `var` when the type is obvious from the right side, otherwise write the type.
- Names say what things are. No abbreviations like `num`, `tmp`, `str`.
- Replace magic numbers with a named constant: `const int MaxNumber = 1000;`
- Keep methods short. If a method needs a comment to explain a block, move that block into its own method with a good name.

## 4. Keep it simple

- Prefer plain loops, `if` and `string.Split` over clever solutions.
- Simple LINQ like `Where`, `Select` and `Sum` is fine. Avoid long LINQ chains that are hard to read.
- No regex if `Split`, `StartsWith` or `IndexOf` can do the job.
- Comments explain *why*, not *what*. No comments that repeat the code.

## 5. Error handling

- Validate input at the start of public methods (guard clauses).
- Invalid input throws an exception with a clear message that tells the user what is wrong:
  - `ArgumentNullException` for `null`
  - `ArgumentException` for invalid values (for example negative numbers, bad format)
  - `ArgumentOutOfRangeException` for values outside an allowed range
- The model throws, the UI catches. The ConsoleApp catches the exceptions it knows about, shows the message and lets the user try again. The app must never crash on bad input.
- Never use an empty `catch`. Never catch `Exception` in the model.

## 6. Tests

- Framework: xUnit with the built-in `Assert`. No extra assertion or mocking libraries.
- Test name: `Method_Scenario_ExpectedResult`, for example `Add_EmptyString_ReturnsZero`.
- Every test follows Arrange / Act / Assert.
- Use `[Theory]` with `[InlineData]` when the same rule is tested with several inputs.
- Every rule in the task has at least one test. Every error case has a test that checks the exception type.
- All tests must pass before a task is done.
